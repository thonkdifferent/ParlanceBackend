using System;
using System.Linq;
using System.Threading.Tasks;
using accounts.DBus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParlanceBackend.Authentication;
using ParlanceBackend.Data;
using ParlanceBackend.Models;
using ParlanceBackend.Services;
using Tmds.DBus;

namespace ParlanceBackend.Controllers
{
    [Route("api/superusers")]
    [ApiController]
    [Authorize(AuthenticationSchemes = DBusAuthenticationHandler.SchemeName, Roles = ProjectsAuthorizationHandler.SuperuserPermission)]
    public class SuperusersController : ControllerBase
    {
        private readonly ProjectContext _context;
        private readonly AccountsService _accounts;

        public SuperusersController(ProjectContext context, AccountsService accounts)
        {
            _context = context;
            _accounts = accounts;
        }

        [HttpGet]
        public async Task<ActionResult<string[]>> GetSuperusers()
        {
            return await Task.WhenAll(_context.Superusers.AsEnumerable().Select(async user =>
            {
                var userPath = await _accounts.AccountsManager.UserByIdAsync(user.UserId);
                var userProxy = _accounts.Bus.CreateProxy<IUser>("com.vicr123.accounts", userPath);
                return await userProxy.GetUsernameAsync();
            }));
        }
        
        [HttpPost("{user}")]
        public async Task<ActionResult> AddSuperuser(string user)
        {
            try
            {
                var userId = await _accounts.AccountsManager.UserIdByUsernameAsync(user);

                _context.Superusers.Add(new Superuser
                {
                    UserId = userId
                });
                await _context.SaveChangesAsync();
                
                return NoContent();
            }
            catch (DBusException e)
            {
                if (e.ErrorName == "com.vicr123.accounts.Error.NoAccount")
                {
                    return NotFound();
                }

                throw;
            }
        }
        
        [HttpDelete("{user}")]
        public async Task<ActionResult> RemoveSuperuser(string user)
        {
            try
            {
                var userId = await _accounts.AccountsManager.UserIdByUsernameAsync(user);

                var superuser = _context.Superusers.Single(user => user.UserId == userId);

                _context.Superusers.Remove(superuser);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DBusException e)
            {
                if (e.ErrorName == "com.vicr123.accounts.Error.NoAccount")
                {
                    return NotFound();
                }

                throw;
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}