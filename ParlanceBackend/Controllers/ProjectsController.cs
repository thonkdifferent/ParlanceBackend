using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ParlanceBackend.Data;
using ParlanceBackend.Misc;
using ParlanceBackend.Models;
using ParlanceBackend.Services;
using ParlanceBackend.TranslationFiles;
using Microsoft.Extensions.Logging;
using ParlanceBackend.Authentication;

namespace ParlanceBackend.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ProjectContext _context; //database context
        private readonly IOptions<ParlanceConfiguration> _parlanceConfiguration;// app configuration
        private readonly GitService _git; //git functionality
        private readonly TranslationFileService _translationFile;//translation file manipulation
        private readonly ILogger<ProjectsController> _logger;
        private readonly IAuthorizationService _authorizationService;

        #region Error handling
        private void LogException(Exception ex)
        {
            _logger.LogError(ex.Message);
            _logger.LogTrace(ex.StackTrace);
            _logger.LogDebug(ex.Source);
        }
        private ContentResult InternalError(Exception ex)
        {
            LogException(ex);
            return new ContentResult()
            {
                Content = "Something went wrong",
                StatusCode = 500
            };
        }
        #endregion
        /// <summary>
        /// Initializes the controller
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="parlanceConfiguration">App configuration</param>
        /// <param name="gitService">Methods for Git</param>
        /// <param name="translationFileService">Translation file manipulation methods</param>
        /// <param name="logger">Logging functionality</param>
        public ProjectsController(ProjectContext context, IOptions<ParlanceConfiguration> parlanceConfiguration,
            GitService gitService, TranslationFileService translationFileService,
            ILogger<ProjectsController> logger, IAuthorizationService authorizationService)
        {
            _context = context;
            _parlanceConfiguration = parlanceConfiguration;
            _git = gitService;
            _translationFile = translationFileService;
            _logger = logger;
            _authorizationService = authorizationService;
        }

        // GET: api/Projects
        /// <summary>
        /// Gets all the projects
        /// </summary>
        /// <returns>A list of all the projects</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.Select(x => x.ToPublicProject(_parlanceConfiguration)).ToListAsync();
        }

        // GET: api/Projects/5
        /// <summary>
        /// Gets project by name
        /// </summary>
        /// <param name="name">Name of the project</param>
        /// <returns>The project requested</returns>
        [HttpGet("{name}")]
        public async Task<ActionResult<Project>> GetProject(string name)
        {
            var project = await _context.Projects.FindAsync(name);
            // if the project doesn't exist
            if (project == null)
            {
                return NotFound();
            }

            //populate internal fields
            return project.ToPublicProject(_parlanceConfiguration);
        }

        /// <summary>
        /// Gets the translation file in the format of your choosing
        /// </summary>
        /// <param name="name">Name of the project</param>
        /// <param name="subprojectSlug">The slug representation of the subproject name</param>
        /// <param name="language">What language</param>
        /// <param name="type">What language format shoult it return(Only .po works because E)</param>
        /// <returns></returns>
        [HttpGet("{name}/{subprojectSlug}/{language}/{type}")]
        public async Task<ActionResult> GetTranslationFile(string name, string subprojectSlug, string language, string type)
        {
            var projectInternal = await _context.Projects.FindAsync(name);//try get the project

            if (projectInternal == null)//if not found, say not found
            {
                return NotFound();
            }
            TranslationFile translationFile;
            try
            {//try get the translation file
                translationFile = _translationFile.TranslationFile(projectInternal, subprojectSlug, language);//grab the translation file in an universal format
            }
            catch (System.IO.FileNotFoundException)//if the subproject has not been found
            {
                return NotFound();
            }
            catch (ArgumentException e)
            {
                return new ContentResult
                {
                    Content = e.Message,
                    StatusCode = 406
                };
            }
            catch (Exception e)//if something else happened
            {
                return InternalError(e);
            }
            return File(GettextTranslationFile.Save(translationFile), "application/octet-stream",
                type);
        }

        /// <summary>
        /// Updates the translation file with a new information
        /// </summary>
        /// <param name="delta">New information</param>
        /// <param name="name">Name of project</param>
        /// <param name="subprojectSlug">Name of subproject</param>
        /// <param name="language">Language</param>
        /// <returns></returns>
        [HttpPost("{name}/{subprojectSlug}/{language}")]
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName)]
        public async Task<ActionResult> UpdateTranslationFile(TranslationDelta delta, string name, string subprojectSlug, string language)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, (name, language), ProjectsAuthorizationHandler.UpdateTranslationFile);

            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }
            
            var projectInternal = await _context.Projects.FindAsync(name);//find the project

            if (projectInternal == null)
            {
                return NotFound();
            }
            try//update the translation file
            {
                _translationFile.UpdateTranslationFile(delta, projectInternal, subprojectSlug, language);
            }
            catch (Exception e)
            {
                return InternalError(e);
            }
            
            return NoContent();
        }

        [HttpGet("{name}/{subprojectSlug}/{language}/canWrite")]
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName)]
        public async Task<ActionResult> GetTranslationFilePermissions(string name, string subprojectSlug, string language)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, (name, language), ProjectsAuthorizationHandler.UpdateTranslationFile);

            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            return NoContent();
        }
        
        [HttpPost("{name}/{subprojectSlug}/{language}/create")]
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName)]
        public async Task<ActionResult> CreateNewTranslationLanguage(string name, string subprojectSlug, string language)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, (name, language), ProjectsAuthorizationHandler.UpdateTranslationFile);

            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }

            var projectInternal = await _context.Projects.FindAsync(name);
            await _translationFile.CreateTranslationFile(projectInternal, subprojectSlug, language);
            return NoContent();
        }

        #region Put method that has been comented since forever but we're not doing anything with it but it must stay because we may do something with it
        // // PUT: api/Projects/5
        // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // [HttpPut("{name}")]
        // public async Task<IActionResult> PutProject(string name, Project project)
        // {
        //     //TODO: Ensure the user is a superuser
        //     //For now, don't allow updating projects
        //     return BadRequest();

        //     if (name != project.Name)
        //     {
        //         return BadRequest();
        //     }

        //     _context.Entry(project).State = EntityState.Modified;

        //     try
        //     {
        //         await _context.SaveChangesAsync();
        //     }
        //     catch (DbUpdateConcurrencyException)
        //     {
        //         if (!ProjectExists(name))
        //         {
        //             return NotFound();
        //         }
        //         else
        //         {
        //             throw;
        //         }
        //     }

        //     return NoContent();
        // } 
        #endregion

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName)]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, default, ProjectsAuthorizationHandler.CreateNewProject);

            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }
            
            
            project.SetConfiguration(_parlanceConfiguration);
            try {
                ProjectPrivate projectPrivate = new()//populate with internal data
                {
                    Name = project.Name,
                    GitCloneUrl = project.GitCloneUrl,
                    Branch = project.Branch
                };

                await _git.Clone(projectPrivate);//do the clone command

                _context.Projects.Add(projectPrivate);
                await _context.SaveChangesAsync();//save the project to the database
                
                return CreatedAtAction("GetProject", new { name = project.Name }, project);
            } catch (Exception e) {

                return InternalError(e);
            }
        }

        // DELETE: api/Projects/5
        [HttpDelete("{name}")]
        [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName)]
        public async Task<IActionResult> DeleteProject(string name)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, default, ProjectsAuthorizationHandler.CreateNewProject);

            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }
            
            ProjectPrivate project = await _context.Projects.FindAsync(name);
            if (project == null)
            {
                return NotFound();
            }

            try
            {
                _git.Remove(project);//remove the project from the DB
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                return InternalError(e);
            }
            return NoContent();
        }

    }
}
