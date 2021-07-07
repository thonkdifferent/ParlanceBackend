using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ParlanceBackend.Data;
using ParlanceBackend.Misc;
using ParlanceBackend.Models;

namespace ParlanceBackend.Controllers
{
    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ProjectContext _context;
        private readonly IOptions<ParlanceConfiguration> _parlanceConfiguration;
        public ProjectsController(ProjectContext context, IOptions<ParlanceConfiguration> parlanceConfiguration)
        {
            _context = context;
            _parlanceConfiguration = parlanceConfiguration;
        }

        // GET: api/Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.Select(x => ProjectPrivate.ToPublicProject(x)).ToListAsync();
        }

        // GET: api/Projects/5
        [HttpGet("{name}")]
        public async Task<ActionResult<Project>> GetProject(string name)
        {
            var project = await _context.Projects.FindAsync(name);

            if (project == null)
            {
                return NotFound();
            }

            return ProjectPrivate.ToPublicProject(project);
        }

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

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            //TODO: Ensure the user is a superuser
            try {
                ProjectPrivate projectPrivate = new ProjectPrivate {
                    Name = project.Name,
                    GitCloneUrl = project.GitCloneUrl,
                    Branch = project.Branch
                };

                projectPrivate.Clone(_parlanceConfiguration.Value.GitRepository);

                _context.Projects.Add(projectPrivate);
                await _context.SaveChangesAsync();
                
                return CreatedAtAction("GetProject", new { name = project.Name }, project);
            } catch (Exception) {
                //TODO: better error???
                return BadRequest();
            }
        }

        // // DELETE: api/Projects/5
        // [HttpDelete("{name}")]
        // public async Task<IActionResult> DeleteProject(string name)
        // {
        //     //TODO: Ensure the user is a superuser
        //     //For now, don't allow deleting projects
        //     return BadRequest();
        //     var project = await _context.Projects.FindAsync(name);
        //     if (project == null)
        //     {
        //         return NotFound();
        //     }

        //     _context.Projects.Remove(project);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }

        private bool ProjectExists(string name)
        {
            return _context.Projects.Any(e => e.Name == name);
        }

    }
}
