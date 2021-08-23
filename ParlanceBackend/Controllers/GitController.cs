using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ParlanceBackend.Data;
using ParlanceBackend.Services;

namespace ParlanceBackend.Controllers
{
    [Route("api/git")]
    [ApiController]
    public class GitController : ControllerBase
    {
        
        private readonly ProjectContext _context;
        private readonly IOptions<ParlanceConfiguration> _parlanceConfiguration;
        private readonly GitService _git;
        private readonly TranslationFileService _translationFile;
        public GitController(ProjectContext context, IOptions<ParlanceConfiguration> parlanceConfiguration, GitService gitService, TranslationFileService translationFileService)
        {
            _context = context;
            _parlanceConfiguration = parlanceConfiguration;
            _git = gitService;
            _translationFile = translationFileService;
        }

        [HttpPost("{name}/pull")]
        public async Task<ActionResult> PullGitRepository(string name)
        {
            var projectInternal = await _context.Projects.FindAsync(name);

            if (projectInternal == null)
            {
                return NotFound();
            }

            await _git.Pull(projectInternal);
            
            return NoContent();
        }

        [HttpPost("{name}/commitandpush")]
        public async Task<ActionResult> CommitAndPushGitRepository(string name)
        {
            var projectInternal = await _context.Projects.FindAsync(name);

            if (projectInternal == null)
            {
                return NotFound();
            }

            await _git.CommitAndPush(projectInternal);
            
            return NoContent();
        }

        [HttpPost("hook/github")]
        public async Task<ActionResult> ExecuteGithubWebhook()
        {
            //We're only interested in push events
            if (Request.Headers["X-GitHub-Event"] != "push") return NoContent();
            
            
            return NoContent();
        }
    }
}