using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ParlanceBackend.Models
{
    public class JsonFile
    {
        public class Subproject
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("type")]
            public string Type { get; set; }

            [JsonPropertyName("path")]
            public string Path { get; set; }
        }

        public class Root
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("subprojects")]
            public List<Subproject> Subprojects { get; set; }
        }
    }
}
