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

        public async Task Pull(IOptions<ParlanceConfiguration> configuration)
        {
            string repoLocation = Utility.GetDirectoryFromSlug(Slug, configuration);
            // Repository repo = new Repository(repoLocation);
            // repo.Network.Fetch(repo.Network.Remotes["origin"].Url, new []{Branch});
            // repo.MergeFetchedRefs(Constants.GetSignature(), null);
            
            // I HATE HATE HATE HATE :(
            using (Process gitProcess = new Process()) {
                gitProcess.StartInfo.FileName = "git";
                gitProcess.StartInfo.Arguments = $"pull";
                gitProcess.StartInfo.WorkingDirectory = repoLocation;
                gitProcess.Start();

                await gitProcess.WaitForExitAsync();

                if (gitProcess.ExitCode != 0) {
                    Directory.Delete(repoLocation, true);
                    throw new Exception("Pull Failed");
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

        private JsonFile.Subproject FindSubproject(IOptions<ParlanceConfiguration> configuration, string subproject)
        {
            var publicProj = ToPublicProject(configuration);

            var spec = publicProj.specFile();

            var subprojectObj = spec.Subprojects.Find(sp => sp.Slug == subproject);
            if (subprojectObj == null) return null;
            
            subprojectObj.SetConfiguration(configuration);

            return subprojectObj;
        }

        private string TranslationFileFilename(JsonFile.Subproject subproject,
            string language)
        {
            var fileNamePattern = Path.GetFileName(subproject.Path);
            if (fileNamePattern == null) return null;
                
            var translationsDirectory = subproject.GetParentDirectory();
            var translationFileName = $"{translationsDirectory.FullName}/{fileNamePattern.Replace("{lang}", language)}";

            return translationFileName;
        }

        public void UpdateTranslationFile(TranslationDelta delta, IOptions<ParlanceConfiguration> configuration, string subproject, string language)
        {
            JsonFile.Subproject subprojectObj = FindSubproject(configuration, subproject);
            string translationFileName = TranslationFileFilename(FindSubproject(configuration, subproject), language);

            switch (subprojectObj.Type)
            {
                case "qt":
                    QtTranslationFile.Update(translationFileName, delta);
                    break;
                default:
                    throw new Exception("Unknown File Type");
            }
        }

        public TranslationFile TranslationFile(IOptions<ParlanceConfiguration> configuration, string subproject, string language)
        {
            JsonFile.Subproject subprojectObj = FindSubproject(configuration, subproject);
            string translationFileName = TranslationFileFilename(subprojectObj, language);
            
            //TODO: maybe throw an error instead?
            if (!File.Exists(translationFileName)) return new TranslationFile();

            return subprojectObj.Type switch
            {
                "qt" => QtTranslationFile.LoadFromFile(translationFileName),
                "gettext" => GettextTranslationFile.LoadFromFile(translationFileName),
                "webext-json" => WebExtensionsJsonTranslationFile.LoadFromFile(translationFileName),
                _ => throw new Exception("Unknown File Type")
            };
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
