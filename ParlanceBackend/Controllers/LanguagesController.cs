using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParlanceBackend.Data;
using ParlanceBackend.Models;

namespace ParlanceBackend.Controllers
{
    [Route("api/languages")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private readonly ProjectContext _context; //database context

        public LanguagesController(ProjectContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<List<Language>> GetLanguages()
        {
            return await _context.Languages.ToListAsync();
        }

        [HttpGet("{identifier}")]
        public async Task<ActionResult<Language>> GetLanguageByIdentifier(string identifier)
        {
            try
            {
                return await _context.Languages.SingleAsync(language => language.Identifier.ToLower() == identifier.ToLower().Replace("_", "-"));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}