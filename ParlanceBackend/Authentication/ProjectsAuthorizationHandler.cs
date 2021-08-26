using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ParlanceBackend.Data;
using ParlanceBackend.Models;
using ParlanceBackend.Services;

namespace ParlanceBackend.Authentication
{
    public class ProjectsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, (string? project, string? language)>
    {
        private readonly ProjectContext _context;
        private readonly IOptions<ParlanceConfiguration> _parlanceConfiguration;
        private readonly AccountsService _accounts;
        
        public const string UpdateTranslationFilePermission = "UpdateTranslationFile";
        public static OperationAuthorizationRequirement UpdateTranslationFile = new()
        {
            Name = UpdateTranslationFilePermission
        };
        
        public const string CreateNewProjectPermission = "CreateNewProject";
        public const string ModifyPermissionsPermission = "ModifyPermissions";
        public const string SuperuserPermission = "Superuser";

        public ProjectsAuthorizationHandler(ProjectContext context, IOptions<ParlanceConfiguration> parlanceConfiguration, AccountsService accounts)
        {
            _context = context;
            _parlanceConfiguration = parlanceConfiguration;
            _accounts = accounts;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement,
            (string project, string language) resource)
        {
            if (context.User.IsInRole(SuperuserPermission))
            {
                context.Succeed(requirement);
                return;
            }
            var userId = ulong.Parse(context.User.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier).Value);

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