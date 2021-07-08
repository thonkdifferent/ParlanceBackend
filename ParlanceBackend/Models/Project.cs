using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using LibGit2Sharp;
using ParlanceBackend.Misc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Diagnostics;

namespace ParlanceBackend.Models
{
    public class ProjectPrivate
    {
        [Key]
        public string Name { get; set; }
        public string GitCloneUrl { get; set; }
        public string Slug => Name.ToLower().Replace(" ", "-");
        public string Branch { get; set; }

        public async Task Clone(string GitRepositoryPath)
        {
            string repoLocation = Utility.GetDirectoryFromSlug(Slug, GitRepositoryPath);
            if (!Directory.Exists(repoLocation)) {//TODO: Better detection
                // Repository.Clone(GitCloneUrl, repoLocation);
                
                //Clone the repo
                Directory.CreateDirectory(repoLocation);
                using (Process gitProcess = new Process()) {
                    gitProcess.StartInfo.FileName = "git";
                    gitProcess.StartInfo.Arguments = $"clone {GitCloneUrl} {repoLocation}";
                    gitProcess.Start();

                    await gitProcess.WaitForExitAsync();

                    if (gitProcess.ExitCode != 0) {
                        Directory.Delete(repoLocation, true);
                        throw new Exception("Clone Failed");
                    }
                }
            }
        }

        public static Project ToPublicProject(ProjectPrivate project) =>
        new Project
        {
            Name = project.Name,
            GitCloneUrl = project.GitCloneUrl,
            Branch = project.Branch
        };
    }
    
    public class Project
    {
        [Key]
        public string Name { get; set; }
        public string GitCloneUrl { get; set; }
        public string Branch { get; set; }

    }

}
