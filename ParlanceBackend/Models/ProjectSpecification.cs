using System.Collections.Generic;
using System.Text.Json.Serialization;
using ParlanceBackend.Misc;
using Microsoft.Extensions.Options;
using System.IO;

namespace ParlanceBackend.Models
{
    public class JsonFile
    {
        public class Lang
        {
            public string Identifier {get; set;}
        }

        public class Subproject
        {
            public string parentProjName;
            private ParlanceConfiguration configuration;
            public void SetConfiguration(ParlanceConfiguration configuration) {
                this.configuration = configuration;
            }

            public string Name { get; set; }

            public string Type { get; set; }

            public string Path { get; set; }

            public string BaseLang { get; set; }

            public string Slug { get => Utility.Slugify(Name);}

            public Lang[] Languages { get {
                string repoLocation = Utility.GetDirectoryFromSlug(Utility.Slugify(this.parentProjName), this.configuration.GitRepository);
                string fullSearchLocation = $"{repoLocation}/{Path}";

                string fileNamePattern = System.IO.Path.GetFileName(fullSearchLocation);

                DirectoryInfo parentDirectory = Directory.GetParent(fullSearchLocation);
                if (!parentDirectory.Exists) return System.Array.Empty<Lang>();

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

            private List<Subproject> SubprojectsPrivate;
            public List<Subproject> Subprojects { get => SubprojectsPrivate; set {
                foreach (Subproject subproj in value) {
                    subproj.parentProjName = Name;
                }
                SubprojectsPrivate = value;
            } }
        }
    }
}
