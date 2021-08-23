using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Karambolo.PO;

namespace ParlanceBackend.TranslationFiles {
    class GettextTranslationFile {
        public static TranslationFile LoadFromFile(string FileName) {
            return LoadFromBytes(File.ReadAllBytes(FileName));
        }

        static TranslationFile LoadFromBytes(byte[] bytes) {
            //Create a translation file and read in the information
            return new TranslationFile();
        }

        public static byte[] Save(TranslationFile file)
        {
            MemoryStream stream = new MemoryStream();
            TextWriter writer = new StreamWriter(stream);
            
            //Write the headers
            writer.WriteLine("msgid \"\"");
            writer.WriteLine("msgstr \"\"");
            writer.WriteLine("\"X-Generator: Parlance\"");
            writer.WriteLine();
            
            //Write the messages
            foreach (Message message in file.Messages)
            {
                List<string> comments = message.Location?.Select(location => $"#: {location.File}:{location.Line}")
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
                    for (int i = 0; i < message.Translation.Length; i++)
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
    }
}