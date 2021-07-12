using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using LibGit2Sharp;
using Microsoft.Extensions.Options;
using ParlanceBackend.Data;
using ParlanceBackend.Misc;
using ParlanceBackend.Models;

namespace ParlanceBackend.Services
{
    public class GitService
    {
        private readonly ProjectContext _context;
        private readonly IOptions<ParlanceConfiguration> _parlanceConfiguration;
        public GitService(ProjectContext context, IOptions<ParlanceConfiguration> parlanceConfiguration)
        {
            _context = context;
            _parlanceConfiguration = parlanceConfiguration;
        }
        
        private static string Parse(string input)
        {
            return input.Replace("{UserFolder}", Constants.USER_FOLDER)
                .Replace("{ConfigFolder}", Constants.CONFIGURATION_FOLDER)
                .Replace("{DocsFolder}", Constants.DOCUMENTS_FOLDER);
        }

        private string GetDirectoryFromSlug(string slug)
        {
            return $"{Parse(_parlanceConfiguration.Value.GitRepository)}/repos/{slug}";
        }
        
        public async Task Clone(ProjectPrivate project)
        {
            var repoLocation = GetDirectoryFromSlug(project.Slug);
            if (!Directory.Exists(repoLocation)) {//TODO: Better detection
                // Repository.Clone(GitCloneUrl, repoLocation);
                
                //Clone the repo
                Directory.CreateDirectory(repoLocation);
                using var gitProcess = new Process
                {
                    StartInfo =
                    {
                        FileName = "git",
                        Arguments = $"clone {project.GitCloneUrl} --branch {project.Branch} {repoLocation}"
                    }
                };

                gitProcess.Start();

                await gitProcess.WaitForExitAsync();

                if (gitProcess.ExitCode != 0) {
                    Directory.Delete(repoLocation, true);
                    throw new Exception("Clone Failed");
                }
            }
        }
        
        public async Task CommitAndPush(ProjectPrivate project)
        {
            string repoLocation = GetDirectoryFromSlug(project.Slug);
        }
        
        public async Task Pull(ProjectPrivate project)
        {
            var repoLocation = GetDirectoryFromSlug(project.Slug);
            // Repository repo = new Repository(repoLocation);
            // repo.Network.Fetch(repo.Network.Remotes["origin"].Url, new []{Branch});
            // repo.MergeFetchedRefs(Constants.GetSignature(), null);
            
            // I HATE HATE HATE HATE :(
            using var gitProcess = new Process
            {
                StartInfo = {FileName = "git", Arguments = $"pull", WorkingDirectory = repoLocation}
            };
            gitProcess.Start();

            await gitProcess.WaitForExitAsync();

            if (gitProcess.ExitCode != 0) {
                Directory.Delete(repoLocation, true);
                throw new Exception("Pull Failed");
            }
        }
        
        public void Remove(ProjectPrivate project) {
            var repoLocation = GetDirectoryFromSlug(project.Slug);
            Directory.Delete(repoLocation, true);
        }
        
        
    }
}