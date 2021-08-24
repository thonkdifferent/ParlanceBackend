using System;
using System.IO;
using System.Threading.Tasks;
using ParlanceBackend.Models;

namespace ParlanceBackend.TranslationFiles
{
    interface ITranslationFileFormat
    {
        public TranslationFile LoadFromBytes(byte[] bytes);
        public byte[] Save(TranslationFile file);
        public void Update(string fileName, TranslationDelta delta);
        public string TransformLanguageName(string languageName);

        private static readonly QtTranslationFile QtLoader = new();
        private static readonly GettextTranslationFile GettextLoader = new();
        private static readonly WebExtensionsJsonTranslationFile WebextjsonLoader = new();

        static ITranslationFileFormat LoaderForFormat(string format) => format switch
        {
            "qt" => QtLoader,
            "gettext" => GettextLoader,
            "webext-json" => WebextjsonLoader,
            _ => throw new ArgumentException("Unknown File Type")
        };

        public async Task<TranslationFile> LoadFromFile(string fileName)
        {
            return LoadFromBytes(await File.ReadAllBytesAsync(fileName));
        }

        public async Task SaveToFile(string fileName, TranslationFile file)
        {
            await File.WriteAllBytesAsync(fileName, Save(file));
        }
    }
}