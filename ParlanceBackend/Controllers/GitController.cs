using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ParlanceBackend.Authentication;
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
        private readonly IAuthorizationService _authorizationService;
        public GitController(ProjectContext context, IOptions<ParlanceConfiguration> parlanceConfiguration, GitService gitService, TranslationFileService translationFileService, IAuthorizationService authorizationService)
        {
            _context = context;
            _parlanceConfiguration = parlanceConfiguration;
            _git = gitService;
            _translationFile = translationFileService;
            _authorizationService = authorizationService;
        }

        [HttpPost("{name}/pull")]
        public async Task<ActionResult> PullGitRepository(string name)
        {
            //TODO: Check authorization
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
            //TODO: Check authorization
            var projectInternal = await _context.Projects.FindAsync(name);

            if (projectInternal == null)
            {
                return NotFound();
            }

            await _git.CommitAndPush(projectInternal);
            
            return NoContent();
        }

        [HttpPost("hook/github")]
        public async Task<ActionResult> ExecuteGithubWebhook(JsonElement jsonRequestData)
        {
            //We're only interested in push events
            // if (Request.Headers["X-GitHub-Event"] != "push") return NoContent();
            
            //Iterate through each project
            //Ensure that the clone URL is of the format git@github.com:{repository.full_name}.git
            //Ensure the ref is refs/heads/{branch}
            
            //If we find such a repository, trigger a pull.

            var branch = jsonRequestData.GetProperty("ref").ToString();
            if (branch is null) return BadRequest();
            
            if (!branch.StartsWith("refs/heads/")) return NoContent();
            branch = branch[11..];

            var repositoryName = jsonRequestData.GetProperty("repository").GetProperty("full_name").ToString();
            if (repositoryName is null) return BadRequest();

            var applicableProjects = await _context.Projects
                .Where(project => project.GitCloneUrl == $"git@github.com:{repositoryName}.git")
                .Where(project => project.Branch == branch).ToListAsync();

            foreach (var project in applicableProjects)
            {
                //Pull all matching repositories
                await PullGitRepository(project.Name);
            }
            
            return NoContent();
        }
    }
}