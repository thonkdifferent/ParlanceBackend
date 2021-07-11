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
using ParlanceBackend.TranslationFiles;

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

        public Project ToPublicProject(IOptions<ParlanceConfiguration> configuration) =>
        new Project(configuration)
        {
            Name = this.Name,
            GitCloneUrl = this.GitCloneUrl,
            Branch = this.Branch
        };

        public TranslationFile TranslationFile(IOptions<ParlanceConfiguration> configuration, string subproject, string language)
        {
            Project publicProj = ToPublicProject(configuration);

            JsonFile.Root spec = publicProj.specFile();
            foreach (JsonFile.Subproject subprojectObj in spec.Subprojects)
            {
                if (subprojectObj.Slug == subproject)
                {
                    subprojectObj.SetConfiguration(configuration);
                    
                    //We found the correct subproject
                    string fileNamePattern = Path.GetFileName(subprojectObj.Path);
                    if (fileNamePattern == null) return new();
                    
                    DirectoryInfo translationsDirectory = subprojectObj.GetParentDirectory();
                    string translationFileName = $"{translationsDirectory.FullName}/{fileNamePattern.Replace("{lang}", language)}";
                    
                    //TODO: maybe throw an error instead?
                    if (!File.Exists(translationFileName)) return new TranslationFile();

                    switch (subprojectObj.Type)
                    {
                        case "qt":
                            return QtTranslationFile.LoadFromFile(translationFileName);
                        case "gettext":
                            return GettextTranslationFile.LoadFromFile(translationFileName);
                        case "webext-json":
                            return WebExtensionsJsonTranslationFile.LoadFromFile(translationFileName);
                        default:
                            throw new Exception("Unknown File Type");
                    }
                }
            }
            
            return new();
        }
    }
    
    public class Project
    {
        IOptions<ParlanceConfiguration> configuration;
        public Project() {}

        public Project(IOptions<ParlanceConfiguration> configuration) {
            SetConfiguration(configuration);
        }

        public void SetConfiguration(IOptions<ParlanceConfiguration> configuration)
        {
            this.configuration = configuration;
        }

        public string Name { get; set; }
        public string GitCloneUrl { get; set; }
        public string Branch { get; set; }

        public JsonFile.Root specFile()
        {
            string repoLocation = Utility.GetDirectoryFromSlug(Utility.Slugify(Name), configuration.Value.GitRepository);
            string jsonFile = $"{repoLocation}/.parlance.json";
            if (!File.Exists(jsonFile)) return null;

            return JsonSerializer.Deserialize<JsonFile.Root>(File.ReadAllText(jsonFile), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public JsonFile.Subproject[] Subprojects {
            get
            {
                JsonFile.Root spec = specFile();
                
                if (spec == null) return Array.Empty<JsonFile.Subproject>();
                
                foreach (JsonFile.Subproject subproj in spec.Subprojects) {
                    subproj.SetConfiguration(configuration);
                }

                return spec.Subprojects.ToArray(); //.Select(x => x.Slug).ToArray();
            }
        }
    }

}
