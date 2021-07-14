using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ParlanceBackend.Models;

namespace ParlanceBackend.TranslationFiles {
    class QtTranslationFile {
        public static TranslationFile LoadFromFile(string FileName) {
            return ParseXml(File.ReadAllText(FileName));
        }

        static TranslationFile ParseXml(string xmlDoc) {
            //Create a translation file and read in the information
            XDocument file = XDocument.Parse(xmlDoc);
            IEnumerable<Message> messages = from msg in file.Descendants("message")
                                            select new Message
                                            {
                                                Source = (string)msg.Element("source"),
                                                Key = (string) msg.Element("source"),
                                                Context = (string)msg.Parent.Element("name"),
                                                Location = (from location in msg.Elements("location")
                                                           select new Location
                                                           {
                                                               File = (string)location.Attribute("filename"),
                                                               Line = (int)location.Attribute("line")

                                                           }).ToArray(),
                                                Translation = msg.Attribute("numerus")?.Value == "true" ? 
                                                                            (from nrtr in msg.Descendants("numerusform")
                                                                            select (string)nrtr).ToArray() :
                                                                            new string[1] {
                                                                                (string)msg.Element("translation")
                                                                            },
                                                Unfinished = (string) msg.Element("translation")?.Attribute("type") == "true",
                                                Comment = (string) msg.Element("comment")
                                            };
            return new TranslationFile
            {
                DestinationLanguage = (string)file.Element("TS").Attribute("language"),
                Messages = messages.ToArray()
            };
        }

        public static void Update(string fileName, TranslationDelta delta) {
            File.WriteAllText(fileName, UpdateXml(File.ReadAllText(fileName), delta));
        }

        public static string UpdateXml(string originalFile, TranslationDelta delta)
        {
            XDocument xmlDoc = XDocument.Parse(originalFile);
            var translation = (from tr in xmlDoc.Descendants()
                                  where (string)tr.Element("source") == delta.Key
                                  where (string)tr.Parent.Element("name") == delta.Context
                                  select tr.Element("translation")).First();
            //TODO: Make this nicer
            if(delta.Unfinished == true && translation.Attribute("type") == null)
            {
                translation.Add(new XAttribute("type", "unfinished"));
            }
            if(delta.Unfinished == false && (string)translation.Attribute("type") == "unfinished")
            {
                translation.RemoveAttributes();
            }
            if((string)translation.Parent.Attribute("type") == "numerus")
            {
                //TODO: this is the worst code I've done
                int trCount = translation.Elements("numerusform").Count();
                for (int i = 0; i < trCount; i++)
                {
                    try
                    {
                        translation.Elements("numerusform").ElementAt(i).Value = delta.Translations[i];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        continue;
                    }
                }
            }
            else
            {
                translation.Value = delta.Translations[0];
            }
            string modifiedXml;
            using (StringWriter sw = new StringWriter())
            {
                using (var writer = System.Xml.XmlWriter.Create(sw, new System.Xml.XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false),
                    Indent = true
                }))
                {
                    xmlDoc.Save(writer);
                }
                modifiedXml = sw.ToString();
            }
            return modifiedXml;
        }

        public static byte[] Save(TranslationFile file) {
            throw new NotImplementedException();
        }
    }
}