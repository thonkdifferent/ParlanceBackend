using System;
using ParlanceBackend.Models;

namespace ParlanceBackend.TranslationFiles
{
    public class TranslationFileFormat
    {
        public static TranslationFile LoadFromFile(string format, string fileName)
        {
            return format switch
            {
                "qt" => QtTranslationFile.LoadFromFile(fileName),
                "gettext" => GettextTranslationFile.LoadFromFile(fileName),
                "webext-json" => WebExtensionsJsonTranslationFile.LoadFromFile(fileName),
                _ => throw new ArgumentException("Unknown File Type")
            };
        }

        public static byte[] Save(String format, TranslationFile file)
        {
            return format switch
            {
                "qt" => QtTranslationFile.Save(file),
                "gettext" => GettextTranslationFile.Save(file),
                "webext-json" => WebExtensionsJsonTranslationFile.Save(file),
                _ => throw new ArgumentException("Unknown File Type")
            };
        }

        public static void Update(string format, string fileName, TranslationDelta delta)
        {
            switch (format)
            {
                case "qt":
                    QtTranslationFile.Update(fileName, delta);
                    break;
                default:
                    throw new ArgumentException("Unknown File Type");
            }
        }
    }
}