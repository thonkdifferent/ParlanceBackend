using System.Linq;
using System.Threading.Tasks;
using accounts.DBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ParlanceBackend.Authentication;
using ParlanceBackend.Data;
using ParlanceBackend.Models;
using ParlanceBackend.Services;

namespace ParlanceBackend.Controllers
{
    [Route("api/permissions")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IOptions<ParlanceConfiguration> _parlanceConfiguration;
        private readonly ProjectContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly AccountsService _accounts;
        
        public PermissionsController(IOptions<ParlanceConfiguration> parlanceConfiguration, ProjectContext context, IAuthorizationService authorizationService, AccountsService accounts)
        {
            _parlanceConfiguration = parlanceConfiguration;
            _context = context;
            _authorizationService = authorizationService;
            _accounts = accounts;
        }

        [HttpPost("languages/{language}/{username}")]
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName, Roles = ProjectsAuthorizationHandler.ModifyPermissionsPermission)]
        public async Task<ActionResult> GrantLanguagePermission(string language, string username)
        {
            var languageObject = _context.Languages.SingleOrDefault(obj => obj.Identifier == language);
            if (languageObject is null) return NotFound();

            var userId = await _accounts.AccountsManager.UserIdByUsernameAsync(username);

            _context.AllowedLanguages.Add(new AllowedLanguages
            {
                Language = languageObject,
                UserId = userId
            });
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("languages/{language}/{username}")]
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName, Roles = ProjectsAuthorizationHandler.ModifyPermissionsPermission)]
        public async Task<ActionResult> DenyLanguagePermission(string language, string username)
        {
            var userId = await _accounts.AccountsManager.UserIdByUsernameAsync(username);

            var allowedLanguage = _context.AllowedLanguages.SingleOrDefault(
                obj => obj.Language.Identifier == language && obj.UserId == userId);
            if (allowedLanguage is null) return NotFound();

            _context.AllowedLanguages.Remove(allowedLanguage);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        [HttpGet("languages")]
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName, Roles = ProjectsAuthorizationHandler.ModifyPermissionsPermission)]
        public async Task<ActionResult<AllowedLanguagesPublic>> GetAllLanguagePermissions()
        {
            await _context.Languages.ToListAsync();
            // return Ok(await _context.AllowedLanguages.Where(x => true).ToListAsync());

            var retval = await Task.WhenAll(_context.AllowedLanguages.AsEnumerable().Select(async language =>
            {
                var userPath = await _accounts.AccountsManager.UserByIdAsync(language.UserId);
                var userProxy = _accounts.Bus.CreateProxy<IUser>("com.vicr123.accounts", userPath);
                
                return new AllowedLanguagesPublic
                {
                    Language = language.Language.Identifier,
                    UserName = await userProxy.GetUsernameAsync()
                };
            }));
            return Ok(retval);
        }
    }
}