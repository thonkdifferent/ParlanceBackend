using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ParlanceBackend.Data;
using ParlanceBackend.Models;

namespace ParlanceBackend.Services
{
    public class GitPushService : IHostedService
    {
        private Timer timer;
        
        IServiceScopeFactory _scopeFactory;
        private readonly IOptions<ParlanceConfiguration> _parlanceConfiguration;
        private readonly GitService _git;
        private readonly TranslationFileService _translationFile;
        public GitPushService(IServiceScopeFactory scopeFactory, IOptions<ParlanceConfiguration> parlanceConfiguration, GitService gitService, TranslationFileService translationFileService)
        {
            // _context = context;
            _scopeFactory = scopeFactory;
            _parlanceConfiguration = parlanceConfiguration;
            _git = gitService;
            _translationFile = translationFileService;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(PerformGitPushOperations, null, TimeSpan.FromHours(1), TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        private void PerformGitPushOperations(object _)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ProjectContext>();
            
            foreach (var project in context.Projects)
            {
#pragma warning disable 4014
                _git.CommitAndPush(project);
#pragma warning restore 4014
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}