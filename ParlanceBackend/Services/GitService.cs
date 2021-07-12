using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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
        private readonly IOptions<ParlanceConfiguration> _parlanceConfiguration;
        public GitService(IOptions<ParlanceConfiguration> parlanceConfiguration)
        {
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
            var repoLocation = GetDirectoryFromSlug(project.Slug);
            var repo = new Repository(repoLocation);
            
            var status = repo.RetrieveStatus();
            if (!status.IsDirty) return; //Nothing to do here!
            
            // foreach (var entry in status.Modified)
            // {
            //     repo.Index.Add(entry.FilePath);
            // }
            
            Commands.Stage(repo, "*");
            repo.Commit("Update Translations", Constants.GetSignature(), Constants.GetSignature());

            //Ensure that we are up to date
            await Pull(project);

            if (repo.Index.Conflicts.Any())
            {
                //Merge conficts!
                //TODO: Error handling
                repo.Rebase.Abort();
                return;
            }

            await Push(project);
        }
        
        public async Task Push(ProjectPrivate project)
        {
            var repoLocation = GetDirectoryFromSlug(project.Slug);
            
            // I HATE HATE HATE HATE :(
            using var gitProcess = new Process
            {
                StartInfo = {FileName = "git", Arguments = $"push", WorkingDirectory = repoLocation}
            };
            gitProcess.Start();

            await gitProcess.WaitForExitAsync();

            if (gitProcess.ExitCode != 0) {
                throw new Exception("Push Failed");
            }
        }
        
        public async Task Pull(ProjectPrivate project)
        {
            var repoLocation = GetDirectoryFromSlug(project.Slug);
            
            // I HATE HATE HATE HATE :(
            using var gitProcess = new Process
            {
                StartInfo = {FileName = "git", Arguments = $"pull --rebase", WorkingDirectory = repoLocation}
            };
            gitProcess.Start();

            await gitProcess.WaitForExitAsync();

            if (gitProcess.ExitCode != 0) {
                throw new Exception("Pull Failed");
            }
        }
        
        public void Remove(ProjectPrivate project) {
            var repoLocation = GetDirectoryFromSlug(project.Slug);
            Directory.Delete(repoLocation, true);
        }
        
        
    }
}