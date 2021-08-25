using System;
using System.IO;
using System.Threading.Tasks;
using ParlanceBackend.Models;

namespace ParlanceBackend.TranslationFiles
{
    interface ITranslationFileFormat
    {
        public Task<TranslationFile> LoadFromBytes(byte[] bytes, byte[] baseFile);
        public Task<byte[]> Save(TranslationFile file);
        public Task Update(string fileName, string baseFileName, TranslationDelta delta);
        public string TransformLanguageName(string languageName);
        public string NormaliseLanguageName(string languageName);
        

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

        public async Task<TranslationFile> LoadFromFile(string fileName, string baseFile)
        {
            return await LoadFromBytes(await File.ReadAllBytesAsync(fileName), await File.ReadAllBytesAsync(baseFile));
        }

        public async Task SaveToFile(string fileName, TranslationFile file)
        {
            await File.WriteAllBytesAsync(fileName, await Save(file));
        }
    }
}