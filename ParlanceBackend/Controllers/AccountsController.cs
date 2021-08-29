using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
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
using ParlanceBackend.Services;
using Tmds.DBus;

namespace ParlanceBackend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IOptions<ParlanceConfiguration> _parlanceConfiguration;
        private readonly ProjectContext _context;
        private readonly AccountsService _accounts;
        public AccountsController(IOptions<ParlanceConfiguration> parlanceConfiguration, ProjectContext context, AccountsService accounts)
        {
            _parlanceConfiguration = parlanceConfiguration;
            _context = context;
            _accounts = accounts;
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
            
            //In the event that any DBus methods throw an exception, return 500 Internal Server Error
            var newUserObjectPath = await _accounts.AccountsManager.CreateUserAsync(createData.Username, createData.Password, createData.Email);
            var token = await _accounts.AccountsManager.ProvisionTokenAsync(createData.Username, createData.Password, "Parlance", ImmutableDictionary<string, object>.Empty);

            var tokenData = new TokenData {Token = token};
            return tokenData;
        }

        [HttpPost("provisionToken")]
        public async Task<ActionResult<TokenData>> ProvisionToken(ProvisionTokenData provisionData)
        {
            var extraOptions = new Dictionary<string, object>();
            if (provisionData.OtpToken is not null) extraOptions.Add("otpToken", provisionData.OtpToken);
            if (provisionData.NewPassword is not null) extraOptions.Add("newPassword", provisionData.NewPassword);

            try
            {
                var token = await _accounts.AccountsManager.ProvisionTokenAsync(provisionData.Username, provisionData.Password, "Parlance", extraOptions);
                
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
                    case "com.vicr123.accounts.Error.PasswordResetRequestRequired":
                        error = "passwordResetRequestRequired";
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

        [HttpPost("resetMethods")]
        public async Task<ActionResult<ResetMethod[]>> GetResetMethods(ResetMethodsData data)
        {
            try
            {
                var userId = await _accounts.AccountsManager.UserIdByUsernameAsync(data.Username);
                var userPath = await _accounts.AccountsManager.UserByIdAsync(userId);
                var resetProxy = _accounts.Bus.CreateProxy<IPasswordReset>("com.vicr123.accounts", userPath);

                var methods = await resetProxy.ResetMethodsAsync();
                return Ok(methods.Select(method => new ResetMethod()
                {
                    Type = method.Item1,
                    Data = method.Item2
                }));
            }
            catch (DBusException e)
            {
                switch (e.ErrorName)
                {
                    case "com.vicr123.accounts.Error.NoAccount":
                        return NotFound();
                    default:
                        throw;
                }
            }
        }

        [HttpPost("reset")]
        public async Task<ActionResult> PerformReset(ResetData data)
        {
            try
            {
                var userId = await _accounts.AccountsManager.UserIdByUsernameAsync(data.Username);
                var userPath = await _accounts.AccountsManager.UserByIdAsync(userId);
                var resetProxy = _accounts.Bus.CreateProxy<IPasswordReset>("com.vicr123.accounts", userPath);

                await resetProxy.ResetPasswordAsync(data.ResetType, data.ResetProperties.ToDictionary(item => item.Key,
                    item =>
                    {
                        if (item.Value is JsonElement el)
                        {
                            return el.ValueKind switch
                            {
                                JsonValueKind.String => el.GetString(),
                                JsonValueKind.Number => el.GetInt64(),
                                _ => null
                            };
                        }
                        else
                        {
                            return item.Value;
                        }
                    }));
                return NoContent();
            }
            catch (DBusException e)
            {
                switch (e.ErrorName)
                {
                    case "com.vicr123.accounts.Error.NoAccount":
                        return NotFound();
                    default:
                        throw;
                }
            }
        }
        
        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName)]
        public async Task<ActionResult<UserInformationData>> GetUserInformation()
        {
            var userId = User.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            var userObjectPath = await _accounts.AccountsManager.UserByIdAsync(ulong.Parse(userId));
            var userProxy = _accounts.Bus.CreateProxy<IUser>("com.vicr123.accounts", userObjectPath);

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
                Superuser = User.IsInRole(ProjectsAuthorizationHandler.SuperuserPermission),
                AllowedLanguages = await _context.AllowedLanguages.Where(permission => permission.UserId == ulong.Parse(userId)).Select(permission => permission.Language).ToListAsync()
            };
            return data;
        }
        
        
        [HttpPost("me/verify/resend")]
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName)]
        public async Task<ActionResult> ResendVerificationEmail()
        {
            var userId = User.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            
            var userObjectPath = await _accounts.AccountsManager.UserByIdAsync(ulong.Parse(userId));
            var userProxy = _accounts.Bus.CreateProxy<IUser>("com.vicr123.accounts", userObjectPath);

            await userProxy.ResendVerificationEmailAsync();

            return NoContent();
        }
        
        [HttpPost("me/verify")]
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName)]
        public async Task<ActionResult> VerifyEmailAddress(VerifyEmailAddressData data)
        {
            var userId = User.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            
            var userObjectPath = await _accounts.AccountsManager.UserByIdAsync(ulong.Parse(userId));
            var userProxy = _accounts.Bus.CreateProxy<IUser>("com.vicr123.accounts", userObjectPath);

            try
            {
                await userProxy.VerifyEmailAsync(data.VerificationCode);
            }
            catch (DBusException e)
            {
                if (e.ErrorName == "com.vicr123.accounts.Error.VerificationCodeIncorrect") return Unauthorized();
                throw;
            }

            return NoContent();
        }
        
        [HttpPost("me/username")]
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName)]
        public async Task<ActionResult> ChangeUsername(ChangeUsernameData data)
        {
            var userId = User.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            
            var userObjectPath = await _accounts.AccountsManager.UserByIdAsync(ulong.Parse(userId));
            var userProxy = _accounts.Bus.CreateProxy<IUser>("com.vicr123.accounts", userObjectPath);

            try
            {
                if (!await userProxy.VerifyPasswordAsync(data.CurrentPassword)) return Unauthorized();
                
                await userProxy.SetUsernameAsync(data.NewUsername);
            }
            catch (DBusException e)
            {
                if (e.ErrorName == "com.vicr123.accounts.Error.DisabledAccount") return Unauthorized();
                throw;
            }

            return NoContent();
        }
        
        [HttpPost("me/password")]
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName)]
        public async Task<ActionResult> ChangePassword(ChangePasswordData data)
        {
            var userId = User.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            
            var userObjectPath = await _accounts.AccountsManager.UserByIdAsync(ulong.Parse(userId));
            var userProxy = _accounts.Bus.CreateProxy<IUser>("com.vicr123.accounts", userObjectPath);

            try
            {
                if (!await userProxy.VerifyPasswordAsync(data.CurrentPassword)) return Unauthorized();
                
                await userProxy.SetPasswordAsync(data.NewPassword);
            }
            catch (DBusException e)
            {
                if (e.ErrorName == "com.vicr123.accounts.Error.DisabledAccount") return Unauthorized();
                throw;
            }

            return NoContent();
        }
        
        [HttpPost("me/email")]
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName)]
        public async Task<ActionResult> ChangeEmailAddress(ChangeEmailData data)
        {
            var userId = User.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
            
            var userObjectPath = await _accounts.AccountsManager.UserByIdAsync(ulong.Parse(userId));
            var userProxy = _accounts.Bus.CreateProxy<IUser>("com.vicr123.accounts", userObjectPath);

            try
            {
                if (!await userProxy.VerifyPasswordAsync(data.CurrentPassword)) return Unauthorized();
                
                await userProxy.SetEmailAsync(data.NewEmail);
            }
            catch (DBusException e)
            {
                if (e.ErrorName == "com.vicr123.accounts.Error.DisabledAccount") return Unauthorized();
                throw;
            }

            return NoContent();
        }
    }
}