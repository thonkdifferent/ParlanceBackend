using System.IO;

namespace ParlanceBackend.TranslationFiles {
    public class Location {
        public string File { get; set; }
        public int Line { get; set; }
    }
    
    public class Message {
        public string Source { get; set; }

        public string Key { get; set; }

        public string Context { get; set; }
#nullable enable
        public Location[]? Location { get; set; }

        public string? Comment { get; set; }
#nullable disable

        public string[] Translation { get; set; }
        
        public bool Unfinished { get; set; }
    }

    public class TranslationFile {
        public string DestinationLanguage { get; set; }
        public Message[] Messages { get; set; }
    }
}
