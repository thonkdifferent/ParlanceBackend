using System.Collections.Generic;

namespace ParlanceBackend.Models
{
    public class ResetMethodsData
    {
        public string Username { get; set; }
    }

    public class ResetMethod
    {
        public string Type { get; set; }
        public IDictionary<string, object> Data { get; set; }
    }

    public class ResetData
    {
        public string Username { get; set; }
        public string ResetType { get; set; }
        public Dictionary<string, object> ResetProperties { get; set; }
    }
}