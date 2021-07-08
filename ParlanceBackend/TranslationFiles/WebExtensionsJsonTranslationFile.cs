using System.IO;
using System;
namespace ParlanceBackend.TranslationFiles {
    class WebExtensionsJsonTranslationFile {
        static TranslationFile LoadFromFile(string FileName) {
            return LoadFromBytes(File.ReadAllBytes(FileName));
        }

        static TranslationFile LoadFromBytes(byte[] bytes) {
            //Create a translation file and read in the information
            return new TranslationFile();
        }
        
        static byte[] Save(TranslationFile file) {
            throw new NotImplementedException();
        }
    }
}