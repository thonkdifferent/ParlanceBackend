using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using ParlanceBackend.Data;
using ParlanceBackend.Models;

namespace ParlanceBackend.Authentication
{
    
    
    public class ProjectsAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, (string project, string language)>
    {
        private readonly ProjectContext _context;
        
        public const string UpdateTranslationFilePermission = "UpdateTranslationFile";

        public static OperationAuthorizationRequirement UpdateTranslationFile = new()
        {
            Name = UpdateTranslationFilePermission
        };

        public ProjectsAuthorizationHandler(ProjectContext context)
        {
            _context = context;
        }
        
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement,
            (string project, string language) resource)
        {
            var userId = context.User.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            if (userId == "5903046" && requirement.Name == UpdateTranslationFilePermission)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}