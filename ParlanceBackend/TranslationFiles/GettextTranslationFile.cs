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
            // POCatalog catalog = new POCatalog();
            // catalog.Encoding = "UTF-8";
            // catalog.Language = file.DestinationLanguage;
            // catalog.Headers = new Dictionary<string, string>()
            // {
            //     {"X-Generator", "Parlance"}
            // };
            //
            // //Plurals?
            //
            // foreach (Message message in file.Messages)
            // {
            //     POKey key = new POKey(message.Source, message.Source + "_plural", message.Context);
            //     IPOEntry entry;
            //     if (message.Translation.Length == 1)
            //     {
            //         entry = new POSingularEntry(key)
            //         {
            //             Translation = message.Translation[0]
            //         };
            //     }
            //     else
            //     {
            //         entry = new POPluralEntry(key, message.Translation);
            //     }
            //
            //     entry.Comments = new POComment[]
            //     {
            //         new POReferenceComment()
            //         {
            //             References = message.Location
            //                 ?.Select(location => new POSourceReference(location.File, location.Line)).ToList()
            //         },
            //         new POExtractedComment()
            //         {
            //             Text = message.Comment
            //         }
            //     };
            //     
            //     catalog.Add(entry);
            // }
            //
            // MemoryStream stream = new MemoryStream();
            // TextWriter writer = new StreamWriter(stream);
            //
            // POGenerator generator = new POGenerator(new POGeneratorSettings()
            // {
            //
            // });
            // generator.Generate(writer, catalog);
            //
            // return stream.ToArray();

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
                List<String> comments = message.Location?.Select(location => $"#: {location.File}:{location.Line}")
                    .ToList();
                if (comments != null) writer.WriteLine(String.Join(",\n", comments));
                
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