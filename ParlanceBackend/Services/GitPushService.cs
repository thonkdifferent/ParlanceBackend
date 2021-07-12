using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ParlanceBackend.Data;
using ParlanceBackend.Models;

namespace ParlanceBackend.Services
{
    public class GitPushService : IHostedService
    {
        private Timer timer;
        
        private readonly ProjectContext _context;
        private readonly IOptions<ParlanceConfiguration> _parlanceConfiguration;
        public GitPushService(ProjectContext context, IOptions<ParlanceConfiguration> parlanceConfiguration)
        {
            _context = context;
            _parlanceConfiguration = parlanceConfiguration;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.timer = new Timer(PerformGitPushOperations, null, TimeSpan.FromHours(1), TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        private void PerformGitPushOperations(object _)
        {
            foreach (ProjectPrivate project in _context.Projects)
            {
                
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}