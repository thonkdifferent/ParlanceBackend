using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using ParlanceBackend.Models;

namespace ParlanceBackend.TranslationFiles {
    class QtTranslationFile : ITranslationFileFormat
    {
        /// <summary>
        /// Load from bytes
        /// </summary>
        /// <param name="bytes">The byte array with the XML document</param>
        /// <returns>The parsed translation file</returns>
        public TranslationFile LoadFromBytes(byte[] bytes)
        {
            return ParseXml(Encoding.UTF8.GetString(bytes));
        }
        
        /// <summary>
        /// Parses the Qt TS file from a string
        /// </summary>
        /// <param name="xmlDoc">The document as a string</param>
        /// <returns></returns>
        static TranslationFile ParseXml(string xmlDoc) {
            //read the information
            XDocument file = XDocument.Parse(xmlDoc);
            IEnumerable<Message> messages = from msg in file.Descendants("message")//get all the messages
                                            select new Message
                                            {
                                                Source = (string)msg.Element("source"),//source and key are the same in Qt
                                                Key = (string) msg.Element("source"),
                                                Context = (string)msg.Parent.Element("name"),//the context is specified before the messages
                                                Location = (from location in msg.Elements("location")//there can be multiple locations
                                                           select new Location
                                                           {
                                                               File = (string)location.Attribute("filename"),//set the file name
                                                               Line = (int)location.Attribute("line")//and line number

                                                           }).ToArray(),
                                                Translation = msg.Attribute("numerus")?.Value == "true" ? /*if the message is marked as numerus, then the
                                                                                                        translations are going to be put
                                                                                                         like this
                                                                                                        <translation>
                                                                                                             <numerusform>text</numerusform>
                                                                                                             <numerusform>text2</numerusform>
                                                                                                             <numerusform>text3</numerusform>
                                                                                                        </translation>
                                                                                                         otherwise like this
                                                                                                        <translation>text</translation>*/
                                                                            msg.Descendants("numerusform").Select(nrtr => (string)nrtr).ToArray() :
                                                                            new string[1] {
                                                                                (string)msg.Element("translation")//otherwise just get the value
                                                                            },
                                                Unfinished = (string) msg.Element("translation")?.Attribute("type") == "unfinished", // if there is on the translation the type attribute set to unfinished then it's unfinished
                                                Comment = (string) msg.Element("comment") //get the comment if there are any
                                            };
            return new TranslationFile //pack the collected data
            {
                DestinationLanguage = (string)file.Element("TS").Attribute("language"),
                Messages = messages.ToArray()
            };
        }
        /// <summary>
        /// Update a Qt TS file
        /// </summary>
        /// <param name="fileName">File path</param>
        /// <param name="delta">New information</param>
        public void Update(string fileName, TranslationDelta delta) {
            File.WriteAllText(fileName, UpdateXml(File.ReadAllText(fileName), delta));
        }

        public string TransformLanguageName(string languageName)
        {
            var intermediary = languageName.Replace("-", "_");
            var parts = intermediary.Split("_");
            if (parts.Length > 1) parts[1] = parts[1].ToUpper();
            return String.Join("_", parts);
        }

        /// <summary>
        /// Updates the Qt TS file with new data from the frontend
        /// </summary>
        /// <param name="originalFile">The contents of the original file as string</param>
        /// <param name="delta">The new translation information</param>
        /// <returns>Updated Qt TS file in XML form</returns>
        public static string UpdateXml(string originalFile, TranslationDelta delta)
        {
            XDocument xmlDoc = XDocument.Parse(originalFile);
            var translation = (from tr in xmlDoc.Descendants()
                                  where (string)tr.Element("source") == delta.Key //check if there are with the same key and context
                                  where (string)tr.Parent.Element("name") == delta.Context
                                  select tr.Element("translation")).Single(); //Grab the translation that needs to be changed

            //TODO: Make this nicer
            //Check to see if the translation was unfinished and if the delta marks it as finished
            if(delta.Unfinished == true && translation.Attribute("type") == null) 
            {
                translation.Add(new XAttribute("type", "unfinished"));
            }
            if(delta.Unfinished == false && (string)translation.Attribute("type") == "unfinished")
            {
                translation.RemoveAttributes();
            }
            //handle numerus translations
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
                    catch (IndexOutOfRangeException)//if the translation delta doesn't have the full thing(unsure about this
                                                    //but better safe than sorry)
                    {
                        continue;
                    }
                }
            }
            else //if it's not numerus, then the inner text can just be changed to the translation
            {
                translation.Value = delta.Translations[0];
            }
            string modifiedXml;
            using (StringWriter sw = new StringWriter())
            {
                using (var writer = System.Xml.XmlWriter.Create(sw, new System.Xml.XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false),//remove BOM
                    Indent = true//activate indenting
                }))
                {
                    xmlDoc.Save(writer);
                }
                modifiedXml = sw.ToString();
            }
            return modifiedXml;
        }

        public byte[] Save(TranslationFile file)
        {
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XDocumentType("TS", null, null, ""),
                new XElement("TS", 
                    new XAttribute("version", "2.1"),
                    new XAttribute("language", file.DestinationLanguage.Replace("-", "_")),
                    new XAttribute("sourceLanguage", "en_US"),
                    file.Messages.GroupBy(message => message.Context).Select(context => new XElement("context",
                            new XElement("name", context.Key),
                            context.Select(message => new XElement("message", 
                                    message.Location?.Select(location => new XElement("location",
                                        new XAttribute("filename", location.File),
                                        new XAttribute("line", location.Line))) ?? Enumerable.Empty<XElement>(),
                                    new XElement("source", message.Source),
                                    new XElement("translation",
                                        message.Unfinished ? new XAttribute("type", "unfinished") : Enumerable.Empty<XElement>(),
                                        message.Translation.Length == 1 ? message.Translation[0] : message.Translation.Select(translation => new XElement("numerusform", translation)))
                                ))
                        ))
                    )
                );

            return Encoding.UTF8.GetBytes(doc.ToString());
        }
    }
}
