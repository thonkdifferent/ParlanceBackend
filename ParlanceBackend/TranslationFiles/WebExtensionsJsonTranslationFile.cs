using System.IO;
using System;
using ParlanceBackend.Models;

namespace ParlanceBackend.TranslationFiles {
    class WebExtensionsJsonTranslationFile : ITranslationFileFormat {
        public TranslationFile LoadFromBytes(byte[] bytes) {
            //Create a translation file and read in the information
            return new TranslationFile();
        }
        
        public byte[] Save(TranslationFile file)
        {
            return new byte[]{};
        }

        public void Update(string fileName, TranslationDelta delta)
        {
            throw new NotImplementedException();
        }

        public string TransformLanguageName(string languageName)
        {
            return languageName;
        }
    }
}