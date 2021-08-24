using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ParlanceBackend.Data;
using ParlanceBackend.Models;

namespace ParlanceBackend.Authentication
{
    
    
    public class ProjectsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, (string? project, string? language)>
    {
        private readonly ProjectContext _context;
        
        public const string UpdateTranslationFilePermission = "UpdateTranslationFile";
        public static OperationAuthorizationRequirement UpdateTranslationFile = new()
        {
            Name = UpdateTranslationFilePermission
        };
        
        public const string CreateNewProjectPermission = "CreateNewProject";
        public static OperationAuthorizationRequirement CreateNewProject = new()
        {
            Name = CreateNewProjectPermission
        };

        public ProjectsAuthorizationHandler(ProjectContext context)
        {
            _context = context;
        }

        private async Task<bool> IsSuperuser(string userId)
        {
            return await _context.Superusers.FindAsync(ulong.Parse(userId)) is not null;
        }
        
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement,
            (string project, string language) resource)
        {
            var userId = context.User.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            if (await IsSuperuser(userId))
            {
                context.Succeed(requirement);
                return;
            }

            switch (requirement.Name)
            {
                case UpdateTranslationFilePermission:
                    if (userId == "5903046")
                    {
                        context.Succeed(requirement);
                    }
                    break;
                case CreateNewProjectPermission:
                    //Nothing extra to check here, only superusers can create new projects
                    break;
            }
        }
    }
}