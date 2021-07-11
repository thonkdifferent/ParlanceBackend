using System;
using System.IO;

namespace ParlanceBackend.TranslationFiles {
    class GettextTranslationFile {
        public static TranslationFile LoadFromFile(string FileName) {
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