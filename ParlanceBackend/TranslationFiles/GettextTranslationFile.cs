using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Karambolo.PO;
using ParlanceBackend.Models;

namespace ParlanceBackend.TranslationFiles {
    class GettextTranslationFile : ITranslationFileFormat {
        public TranslationFile LoadFromBytes(byte[] bytes) {
            //Create a translation file and read in the information
            return new TranslationFile();
        }

        public byte[] Save(TranslationFile file)
        {
            var stream = new MemoryStream();
            TextWriter writer = new StreamWriter(stream);
            
            //Write the headers
            writer.WriteLine("msgid \"\"");
            writer.WriteLine("msgstr \"\"");
            writer.WriteLine("\"X-Generator: Parlance\"");
            writer.WriteLine();
            
            //Write the messages
            foreach (var message in file.Messages)
            {
                var comments = message.Location?.Select(location => $"#: {location.File}:{location.Line}")
                    .ToList();
                if (message.Unfinished) comments.Add("#, fuzzy");
                if (comments != null) writer.WriteLine(string.Join(",\n", comments));
                
                writer.WriteLine($"msgctxt \"{message.Context}\"");
                writer.WriteLine($"msgid \"{message.Source}\"");
                if (message.Translation.Length == 1)
                {
                    writer.WriteLine($"msgstr \"{message.Translation[0]}\"");
                }
                else
                {
                    writer.WriteLine($"msgid_plural \"{message.Source}\"");
                    for (var i = 0; i < message.Translation.Length; i++)
                    {
                        writer.WriteLine($"msgstr[{i}] \"{message.Translation[i]}\"");
                    }
                }
                writer.WriteLine();
            }
            
            writer.Flush();
            stream.Flush();
            return stream.ToArray();
        }

        public void Update(string fileName, TranslationDelta delta)
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