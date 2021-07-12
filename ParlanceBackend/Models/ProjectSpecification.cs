using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ParlanceBackend.Misc;
using Microsoft.Extensions.Options;
using System.IO;

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

            public DirectoryInfo GetParentDirectory()
            {
                string repoLocation = Utility.GetDirectoryFromSlug(Utility.Slugify(this.parentProjName), this.configuration.Value.GitRepository);
                string fullSearchLocation = $"{repoLocation}/{Path}";

                return Directory.GetParent(fullSearchLocation);
            }

            public Lang[] Languages { get {
                string fileNamePattern = System.IO.Path.GetFileName(Path);
                if (fileNamePattern == null) return Array.Empty<Lang>();

                DirectoryInfo parentDirectory = GetParentDirectory();
                if (!parentDirectory.Exists) return Array.Empty<Lang>();

                List<Lang> langs = new List<Lang>();
                foreach (FileInfo file in parentDirectory.GetFiles(fileNamePattern.Replace("{lang}", "*"))) {
                    langs.Add(new Lang{Identifier=System.IO.Path.GetFileNameWithoutExtension(file.Name)});
                }

                return langs.ToArray();
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
