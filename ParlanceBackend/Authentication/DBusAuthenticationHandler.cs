using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using accounts.DBus;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ParlanceBackend.Data;
using ParlanceBackend.Services;
using Tmds.DBus;

namespace ParlanceBackend.Authentication
{
    public class DBusAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SchemeName = "DBusScheme";
        private readonly IOptions<ParlanceConfiguration> _parlanceConfiguration;
        private readonly AccountsService _accounts;
        private readonly ProjectContext _context;
        
        public DBusAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IOptions<ParlanceConfiguration> parlanceConfiguration, AccountsService accounts, ProjectContext context) : base(options, logger, encoder, clock)
        {
            _parlanceConfiguration = parlanceConfiguration;
            _accounts = accounts;
            _context = context;
        }
        
        private async Task<bool> IsSuperuser(ulong userId)
        {
            if (await _accounts.AccountsManager.UserIdByUsernameAsync(_parlanceConfiguration.Value.ForceSuperuserUsername) ==
                userId) return true;
            return await _context.Superusers.AnyAsync(user => user.UserId == userId);
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("No Authorization Header");
            }

            var authorization = Request.Headers["Authorization"];
            if (!authorization.ToString().StartsWith("Bearer "))
            {
                return AuthenticateResult.Fail("Authorization header does not contain Bearer token");
            }

            var token = authorization.ToString()[7..].Trim();
            
            //TODO: handle DBus errors in case token does not work
            try
            {
                var userPath = await _accounts.AccountsManager.UserForTokenAsync(token);
                var userProxy = _accounts.Bus.CreateProxy<IUser>("com.vicr123.accounts", userPath);
                var id = await userProxy.GetIdAsync();

                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Email, await userProxy.GetEmailAsync()),
                    new Claim(ClaimTypes.NameIdentifier, id.ToString())
                };

                List<string> roles = new();
                if (await IsSuperuser(id))
                {
                    roles.Add(ProjectsAuthorizationHandler.SuperuserPermission);
                    roles.Add(ProjectsAuthorizationHandler.CreateNewProjectPermission);
                    roles.Add(ProjectsAuthorizationHandler.ModifyPermissionsPermission);
                }
            
                return AuthenticateResult.Success(new AuthenticationTicket(new GenericPrincipal(new ClaimsIdentity(claims, SchemeName), roles.ToArray()), SchemeName));
            }
            catch (DBusException exception)
            {
                return AuthenticateResult.Fail(exception.Message);
            }
            // throw new System.NotImplementedException();
        }
    }
}