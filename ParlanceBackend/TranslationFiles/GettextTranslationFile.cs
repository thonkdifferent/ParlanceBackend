using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Karambolo.PO;
using ParlanceBackend.Models;

namespace ParlanceBackend.TranslationFiles {
    class GettextTranslationFile : ITranslationFileFormat {
        public async Task<TranslationFile> LoadFromBytes(byte[] bytes, byte[] baseFile) {
            //Create a translation file and read in the information
            return new TranslationFile();
        }

        public async Task<byte[]> Save(TranslationFile file)
        {
            var stream = new MemoryStream();
            TextWriter writer = new StreamWriter(stream);
            
            //Write the headers
            await writer.WriteLineAsync("msgid \"\"");
            await writer.WriteLineAsync("msgstr \"\"");
            await writer.WriteLineAsync("\"X-Generator: Parlance\"");
            await writer.WriteLineAsync();
            
            //Write the messages
            foreach (var message in file.Messages)
            {
                var comments = message.Location?.Select(location => $"#: {location.File}:{location.Line}")
                    .ToList();
                if (message.Unfinished) comments.Add("#, fuzzy");
                if (comments != null) writer.WriteLine(string.Join(",\n", comments));
                
                await writer.WriteLineAsync($"msgctxt \"{message.Context}\"");
                await writer.WriteLineAsync($"msgid \"{message.Source}\"");
                if (message.Translation.Length == 1)
                {
                    await writer.WriteLineAsync($"msgstr \"{message.Translation[0]}\"");
                }
                else
                {
                    await writer.WriteLineAsync($"msgid_plural \"{message.Source}\"");
                    for (var i = 0; i < message.Translation.Length; i++)
                    {
                        await writer.WriteLineAsync($"msgstr[{i}] \"{message.Translation[i]}\"");
                    }
                }
                await writer.WriteLineAsync();
            }
            
            await writer.FlushAsync();
            stream.Flush();
            return stream.ToArray();
        }

        public async Task Update(string fileName, string baseFileName, TranslationDelta delta)
        {
            throw new NotImplementedException();
        }

        public string TransformLanguageName(string languageName)
        {
            return languageName;
        }

        public string NormaliseLanguageName(string languageName)
        {
            return languageName;
        }
    }
}