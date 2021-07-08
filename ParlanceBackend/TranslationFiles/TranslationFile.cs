using System.IO;

namespace ParlanceBackend.TranslationFiles {
    class Context {
        public string File { get; set; }

        public int Line { get; set; }
    }

    class Message {
        public string Source { get; set; }

        public string Key { get; set; }

        public string Context { get; set; }

        public string[] Translation { get; set; }
    }

    class TranslationFile {
        public string DestinationLanguage { get; set; }
        public Message[] messages { get; set; }
    }
}
