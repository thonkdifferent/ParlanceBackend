using ParlanceBackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Resources;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace ParlanceBackend.TranslationFiles
{
    class ResxTranslationFile : ITranslationFileFormat
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
                               // suppressing since async needed to fulfill interface
        public async Task<TranslationFile> LoadFromBytes(byte[] bytes, byte[] baseFile) => ParseXML(Encoding.UTF8.GetString(bytes));
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        public string NormaliseLanguageName(string languageName) => languageName; // nothing needs to be done

        public Task<byte[]> Save(TranslationFile file)
        {
            throw new NotImplementedException();
        }

        public string TransformLanguageName(string languageName) => languageName; // nothing needs to be done

        public async Task Update(string fileName, string baseFileName, TranslationDelta delta)
        {
            await File.WriteAllTextAsync(fileName, UpdateXML(await File.ReadAllTextAsync(fileName), delta));
        }

        private TranslationFile ParseXML(string xmlDoc)
        {
            var file = XDocument.Parse(xmlDoc);
            var messages =
                from msg in file.Descendants("data")
                select new Message
                {
                    Source = (string)msg.Attribute("name"),
                    Key = (string)msg.Element("name"), // these should be the same

                    Context = msg.Descendants().Where(x => x.Name == "comment").First().Value,
                    Translation = new string[] { msg.Descendants().Where(x => x.Name == "value").First().Value },

                    Unfinished = string.IsNullOrWhiteSpace(msg.Descendants().Where(x => x.Name == "value").First().Value),
                    Comment = msg.Descendants().Where(x => x.Name == "comment").First().Value
                };
            return new TranslationFile
            {
                Messages = messages.ToArray()
            };
        }

        private string UpdateXML(string originalFile, TranslationDelta delta)
        {
            var xmlDoc = XDocument.Parse(originalFile);
            var translations = xmlDoc.Descendants("data")
                .Where(x => (string)x.Element("name") == delta.Key);
            
            foreach (var translation in translations)
            {
                translation.Descendants().Where(x => x.Name == "value").First().Value = delta.Translations[0];
            }

            string modifiedXml;
            using (var stringWriter = new StringWriter())
            {
                using (var writer = XmlWriter.Create(stringWriter, new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false),
                    Indent = true
                }))
                {
                    xmlDoc.Save(writer);
                }
                modifiedXml = stringWriter.ToString();
            }
            return modifiedXml;
        }
    }
}
