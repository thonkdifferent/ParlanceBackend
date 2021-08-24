using System.IO;
using System;
namespace ParlanceBackend.TranslationFiles {
    class WebExtensionsJsonTranslationFile {
        public static TranslationFile LoadFromFile(string FileName) {
            return LoadFromBytes(File.ReadAllBytes(FileName));
        }

        static TranslationFile LoadFromBytes(byte[] bytes) {
            //Create a translation file and read in the information
            return new TranslationFile();
        }
        
        public static byte[] Save(TranslationFile file)
        {
            return new byte[]{};
        }
    }
}