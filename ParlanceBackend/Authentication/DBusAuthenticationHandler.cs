using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using accounts.DBus;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ParlanceBackend.Services;
using Tmds.DBus;

namespace ParlanceBackend.Authentication
{
    public class DBusAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SchemeName = "DBusScheme";
        private readonly IOptions<ParlanceConfiguration> _parlanceConfiguration;
        private readonly AccountsService _accounts;
        
        public DBusAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IOptions<ParlanceConfiguration> parlanceConfiguration, AccountsService accounts) : base(options, logger, encoder, clock)
        {
            _parlanceConfiguration = parlanceConfiguration;
            _accounts = accounts;
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
            
                return AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(claims, SchemeName)), SchemeName));
            }
            catch (DBusException exception)
            {
                return AuthenticateResult.Fail(exception.Message);
            }
            // throw new System.NotImplementedException();
        }
    }
}