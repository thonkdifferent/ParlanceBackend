using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ParlanceBackend.Misc;
using Microsoft.Extensions.Options;
using System.IO;
using System.Linq;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;

namespace ParlanceBackend.Models
{
    public class ProjectSpecification
    {
        public class Lang
        {
            public string Identifier {get; set;}
        }

        public class Subproject
        {
            public string parentProjName;
            private IOptions<ParlanceConfiguration> configuration;
            public void SetConfiguration(IOptions<ParlanceConfiguration> configuration) {
                this.configuration = configuration;
            }

            public string Name { get; set; }

            public string Type { get; set; }

            public string Path { get; set; }

            public string BaseLang { get; set; }

            public string Slug { get => Utility.Slugify(Name);}

            public Lang[] Languages { get {
                    var repoLocation = Utility.GetDirectoryFromSlug(Utility.Slugify(this.parentProjName), this.configuration.Value.GitRepository);

                    var matcher = new Matcher();
                    matcher.AddInclude(Path.Replace("{lang}", "*"));
                    var result = matcher.Execute(new DirectoryInfoWrapper(new DirectoryInfo(repoLocation)));

                    return result.Files.Select(file => new Lang {Identifier = System.IO.Path.GetFileNameWithoutExtension(file.Path)}).ToArray();
            }
            }
        }

        public class Root
        {
            public string Name { get; set; }

            private List<Subproject> subprojects;
            
            public List<Subproject> Subprojects { get => subprojects; set {
                foreach (Subproject subproj in value) {
                    subproj.parentProjName = Name;
                }
                subprojects = value;
            } }
        }
    }
}
