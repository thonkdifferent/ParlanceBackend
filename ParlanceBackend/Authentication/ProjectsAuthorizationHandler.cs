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
        
        public const string ModifyPermissionsPermission = "ModifyPermissions";
        public static OperationAuthorizationRequirement ModifyPermissions = new()
        {
            Name = ModifyPermissionsPermission
        };

        public ProjectsAuthorizationHandler(ProjectContext context)
        {
            _context = context;
        }

        private async Task<bool> IsSuperuser(ulong userId)
        {
            return await _context.Superusers.AnyAsync(user => user.UserId == userId);
        }
        
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement,
            (string project, string language) resource)
        {
            var userId = ulong.Parse(context.User.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier).Value);

            if (await IsSuperuser(userId))
            {
                context.Succeed(requirement);
                return;
            }

            switch (requirement.Name)
            {
                case UpdateTranslationFilePermission:
                {
                    if (_context.AllowedLanguages.Any(permission => permission.Language.Identifier == resource.language && permission.UserId == userId))
                    {
                        context.Succeed(requirement);
                    }
                    
                    //TODO: Project managers
                    break;
                }
                case CreateNewProjectPermission:
                case ModifyPermissionsPermission:
                    //Nothing extra to check here, only superusers can perform these actions
                    break;
            }
        }
    }
}