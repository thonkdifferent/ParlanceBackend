using System;
using System.ComponentModel.DataAnnotations;
using ParlanceBackend.Misc;
using System.IO;
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

        public Project ToPublicProject(IOptions<ParlanceConfiguration> configuration) =>
        new Project(configuration)
        {
            Name = Name,
            GitCloneUrl = GitCloneUrl,
            Branch = Branch
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

        public ProjectSpecification.Root specFile()
        {
            string repoLocation = Utility.GetDirectoryFromSlug(Utility.Slugify(Name), configuration.Value.GitRepository);
            string jsonFile = $"{repoLocation}/.parlance.json";
            if (!File.Exists(jsonFile)) return null;

            return JsonSerializer.Deserialize<ProjectSpecification.Root>(File.ReadAllText(jsonFile), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public ProjectSpecification.Subproject[] Subprojects {
            get
            {
                ProjectSpecification.Root spec = specFile();
                
                if (spec == null) return Array.Empty<ProjectSpecification.Subproject>();
                
                foreach (ProjectSpecification.Subproject subproj in spec.Subprojects) {
                    subproj.SetConfiguration(configuration);
                }

                return spec.Subprojects.ToArray(); //.Select(x => x.Slug).ToArray();
            }
        }
    }

}
