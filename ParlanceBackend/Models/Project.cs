using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using LibGit2Sharp;
using ParlanceBackend.Misc;
using ParlanceBackend.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Diagnostics;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ParlanceBackend.Models
{
    public class ProjectPrivate
    {
        [Key]
        public string Name { get; set; }
        public string GitCloneUrl { get; set; }
        public string Slug => Utility.Slugify(Name);
        public string Branch { get; set; }

        public async Task Clone(IOptions<ParlanceConfiguration> configuration)
        {
            string repoLocation = Utility.GetDirectoryFromSlug(Slug, configuration);
            if (!Directory.Exists(repoLocation)) {//TODO: Better detection
                // Repository.Clone(GitCloneUrl, repoLocation);
                
                //Clone the repo
                Directory.CreateDirectory(repoLocation);
                using (Process gitProcess = new Process()) {
                    gitProcess.StartInfo.FileName = "git";
                    gitProcess.StartInfo.Arguments = $"clone {GitCloneUrl} --branch {Branch} {repoLocation}";
                    gitProcess.Start();

                    await gitProcess.WaitForExitAsync();

                    if (gitProcess.ExitCode != 0) {
                        Directory.Delete(repoLocation, true);
                        throw new Exception("Clone Failed");
                    }
                }
            }
        }

        public void Remove(IOptions<ParlanceConfiguration> configuration) {
            string repoLocation = Utility.GetDirectoryFromSlug(Slug, configuration);
            Directory.Delete(repoLocation, true);
        }

        public static Project ToPublicProject(ProjectPrivate project, IOptions<ParlanceConfiguration> configuration) =>
        new Project(configuration)
        {
            Name = project.Name,
            GitCloneUrl = project.GitCloneUrl,
            Branch = project.Branch
        };
    }
    
    public class Project
    {
        private string GitRepositoryPath;

        public Project() {}

        public Project(IOptions<ParlanceConfiguration> configuration) {
            SetConfiguration(configuration);
        }

        public void SetConfiguration(IOptions<ParlanceConfiguration> configuration) {
            this.GitRepositoryPath = configuration.Value.GitRepository;
        }

        [Key]
        public string Name { get; set; }
        public string GitCloneUrl { get; set; }
        public string Branch { get; set; }

        public JsonFile.Subproject[] Subprojects {
            get {
                string repoLocation = Utility.GetDirectoryFromSlug(Utility.Slugify(Name), this.GitRepositoryPath);
                string jsonFile = $"{repoLocation}/.parlance.json";
                if (!File.Exists(jsonFile)) return new JsonFile.Subproject[]{};

                JsonFile.Root spec = JsonSerializer.Deserialize<JsonFile.Root>(File.ReadAllText(jsonFile), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return spec.Subprojects.ToArray(); //.Select(x => x.Slug).ToArray();
            }
        }
    }

}
