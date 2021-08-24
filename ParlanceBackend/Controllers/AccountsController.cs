using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using accounts.DBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ParlanceBackend.Authentication;
using ParlanceBackend.Data;
using ParlanceBackend.Models;
using Tmds.DBus;

namespace ParlanceBackend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IOptions<ParlanceConfiguration> _parlanceConfiguration;
        private readonly ProjectContext _context;
        public AccountsController(IOptions<ParlanceConfiguration> parlanceConfiguration, ProjectContext context)
        {
            _parlanceConfiguration = parlanceConfiguration;
            _context = context;
        }
        
        private async Task<Connection> AccountsConnection()
        {
            var accountsConnection = new Connection(_parlanceConfiguration.Value.AccountsBus);
            await accountsConnection.ConnectAsync();
            return accountsConnection;
        }
        
        [HttpGet("whoami")]
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName)]
        public async Task<string> Whoami()
        {
            return User.Claims.Single(claim => claim.Type == ClaimTypes.Email).Value;
        }

        [HttpPost("create")]
        public async Task<ActionResult<TokenData>> CreateUser(CreateUserData createData)
        {
            var accountsConnection = await AccountsConnection();
            var managerProxy =
                accountsConnection.CreateProxy<IManager>("com.vicr123.accounts", "/com/vicr123/accounts");

            //In the event that any DBus methods throw an exception, return 500 Internal Server Error
            var newUserObjectPath = await managerProxy.CreateUserAsync(createData.Username, createData.Password, createData.Email);
            var token = await managerProxy.ProvisionTokenAsync(createData.Username, createData.Password, "Parlance", ImmutableDictionary<string, object>.Empty);

            var tokenData = new TokenData {Token = token};
            return tokenData;
        }

        [HttpPost("provisionToken")]
        public async Task<ActionResult<TokenData>> ProvisionToken(ProvisionTokenData provisionData)
        {
            var accountsConnection = await AccountsConnection();
            var managerProxy =
                accountsConnection.CreateProxy<IManager>("com.vicr123.accounts", "/com/vicr123/accounts");

            var extraOptions = new Dictionary<string, object>();
            if (provisionData.OtpToken is not null) extraOptions.Add("otpToken", provisionData.OtpToken);
            if (provisionData.NewPassword is not null) extraOptions.Add("newPassword", provisionData.NewPassword);

            try
            {
                var token = await managerProxy.ProvisionTokenAsync(provisionData.Username, provisionData.Password, "Parlance", extraOptions);
                
                var tokenData = new TokenData {Token = token};
                return tokenData;
            }
            catch (DBusException e)
            {
                string error;
                switch (e.ErrorName)
                {
                    case "com.vicr123.accounts.Error.NoAccount":
                    case "com.vicr123.accounts.Error.IncorrectPassword":
                        error = "incorrectCredentials";
                        break;
                    case "com.vicr123.accounts.Error.PasswordResetRequired":
                        error = "passwordResetRequired";
                        break;
                    case "com.vicr123.accounts.Error.DisabledAccount":
                        error = "disabledAccount";
                        break;
                    case "com.vicr123.accounts.Error.TwoFactorRequired":
                        if (provisionData.OtpToken is not null)
                        {
                            goto case "com.vicr123.accounts.Error.IncorrectPassword";
                        }
                        else
                        {
                            error = "twofactorRequired";
                        }
                        break;
                    default:
                        throw;
                }
                var tokenData = new TokenData {Error = error};
                return Unauthorized(tokenData);
            }
        }
        
        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName)]
        public async Task<ActionResult<UserInformationData>> GetUserInformation()
        {
            var userId = User.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            var accountsConnection = await AccountsConnection();
            var managerProxy =
                accountsConnection.CreateProxy<IManager>("com.vicr123.accounts", "/com/vicr123/accounts");

            var userObjectPath = await managerProxy.UserByIdAsync(ulong.Parse(userId));
            var userProxy = accountsConnection.CreateProxy<IUser>("com.vicr123.accounts", userObjectPath);

            UserInformationData information = new()
            {
                Username = await userProxy.GetUsernameAsync(),
                Email = await userProxy.GetEmailAsync(),
                Verified = await userProxy.GetVerifiedAsync()
            };
            return information;
        }

        [HttpGet("me/permissions")]
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName)]
        public async Task<ActionResult<PermissionsData>> GetUserPermissions()
        {
            var userId = User.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            var data = new PermissionsData
            {
                Superuser = await _context.Superusers.FindAsync(ulong.Parse(userId)) is not null,
                AllowedLanguages = await _context.AllowedLanguages.Where(permission => permission.UserId == ulong.Parse(userId)).Select(permission => permission.Language).ToListAsync()
            };
            return data;
        }
    }
}