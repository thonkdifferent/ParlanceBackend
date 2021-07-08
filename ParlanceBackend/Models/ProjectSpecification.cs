using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ParlanceBackend.Models
{
    public class JsonFile
    {
        public class Subproject
        {
            public string Name { get; set; }

            public string Type { get; set; }

            public string Path { get; set; }

            public string BaseLang { get; set; }
        }

        public class Root
        {
            public string Name { get; set; }

            [JsonPropertyName("subprojects")]
            public List<Subproject> Subprojects { get; set; }
        }
    }
}
