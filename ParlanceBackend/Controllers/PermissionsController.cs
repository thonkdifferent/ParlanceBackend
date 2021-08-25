using System.Linq;
using System.Threading.Tasks;
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
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName)]
        public async Task<ActionResult> GrantLanguagePermission(string language, string username)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, ("", ""), ProjectsAuthorizationHandler.ModifyPermissions);

            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

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
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName)]
        public async Task<ActionResult> DenyLanguagePermission(string language, string username)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, ("", ""), ProjectsAuthorizationHandler.ModifyPermissions);

            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            var userId = await _accounts.AccountsManager.UserIdByUsernameAsync(username);

            var allowedLanguage = _context.AllowedLanguages.SingleOrDefault(
                obj => obj.Language.Identifier == language && obj.UserId == userId);
            if (allowedLanguage is null) return NotFound();

            _context.AllowedLanguages.Remove(allowedLanguage);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        [HttpGet("languages")]
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName)]
        public async Task<ActionResult<AllowedLanguages>> GetAllLanguagePermissions()
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, ("", ""), ProjectsAuthorizationHandler.ModifyPermissions);

            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }
            
            return Ok(await _context.AllowedLanguages.ToListAsync());
        }
    }
}