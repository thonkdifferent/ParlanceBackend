using System.IO;

namespace ParlanceBackend.TranslationFiles {
    class Location {
        public string File { get; set; }
        public int Line { get; set; }
    }
    class Translation
    {
        public bool Unfinished { get; set; }
        public string[] Content { get; set; }
    }
    class Message {
        public string Source { get; set; }

        public string Key { get; set; }

        public string Context { get; set; }
#nullable enable
        public Location[]? Location { get; set; }

        public string? Comment { get; set; }
#nullable disable

        public Translation Translation { get; set; }
    }

    class TranslationFile {
        public string DestinationLanguage { get; set; }
        public Message[] Messages { get; set; }
    }
}
