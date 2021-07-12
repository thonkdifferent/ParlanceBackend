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

        public Project ToPublicProject(IOptions<ParlanceConfiguration> configuration) =>
        new Project(configuration)
        {
            Name = this.Name,
            GitCloneUrl = this.GitCloneUrl,
            Branch = this.Branch
        };
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
