using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ParlanceBackend.Misc;
using System.IO;
using System.Linq;
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

        public ProjectSpecification.Root SpecFile()
        {
            var repoLocation = Utility.GetDirectoryFromSlug(Utility.Slugify(Name), configuration.Value.GitRepository);
            var jsonFile = $"{repoLocation}/.parlance.json";
            if (!File.Exists(jsonFile)) return null;

            var rootObj = JsonSerializer.Deserialize<ProjectSpecification.Root>(File.ReadAllText(jsonFile), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var subprojects = rootObj.Subprojects.Select(subproject =>
            {
                subproject.parentProjName = Name;
                return subproject;
            }).ToList();
            
            rootObj.Subprojects = subprojects;
            
            return rootObj;
        }

        public ProjectSpecification.Subproject[] Subprojects {
            get
            {
                ProjectSpecification.Root spec = SpecFile();
                
                if (spec == null) return Array.Empty<ProjectSpecification.Subproject>();
                
                foreach (ProjectSpecification.Subproject subproj in spec.Subprojects) {
                    subproj.SetConfiguration(configuration);
                }

                return spec.Subprojects.ToArray(); //.Select(x => x.Slug).ToArray();
            }
        }
    }

}
