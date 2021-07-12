using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
namespace ParlanceBackend.TranslationFiles {
    class QtTranslationFile {
        public static TranslationFile LoadFromFile(string FileName) {
            return LoadFromBytes(File.ReadAllBytes(FileName));
        }

        static TranslationFile LoadFromBytes(byte[] bytes) {
            //Create a translation file and read in the information
            XmlDocument document = new();
            string xml = Encoding.UTF8.GetString(bytes);
            document.LoadXml(xml);

            TranslationFile translationFile = new();
            translationFile.DestinationLanguage = document.GetElementsByTagName("TS").Item(0).Attributes.GetNamedItem("language").Value;

            List<Message> messages = new();
            XmlNodeList contexts = document.GetElementsByTagName("context");
            foreach (XmlNode context in contexts)
            {
                string contextName = "";
                foreach(XmlNode childNode in context.ChildNodes)
                {
                    if (childNode.Name == "name")
                        contextName = childNode.InnerText;
                    
                    if (childNode.Name == "message")
                    {
                        Message message = new();
                        List<Location> locations = new();
                        List<string> translations = new();
                        //bool isOverridden = false;
                        foreach (XmlNode messageMeta in childNode.ChildNodes)
                        {
                            switch(messageMeta.Name)
                            {
                                case "location":
                                    locations.Add(new Location
                                    {
                                        File = messageMeta.Attributes.GetNamedItem("filename").Value,
                                        Line = Convert.ToInt32(messageMeta.Attributes.GetNamedItem("line").Value)
                                    });
                                    break;
                                case "source":
                                    message.Source = messageMeta.InnerText;
                                    message.Key = messageMeta.InnerText;
                                    break;
                                case "translation":
                                    if (childNode.Attributes.GetNamedItem("numerus") != null)
                                    {
                                        foreach(XmlNode translation in messageMeta.ChildNodes)
                                        {
                                            translations.Add(translation.InnerText);
                                        }
                                    }
                                    else
                                    {
                                        translations.Add(messageMeta.InnerText);
                                    }
                                    break;
                                case "comment":
                                    message.Comment = messageMeta.InnerText;
                                    break;
                            }
                        }
                        message.Location = locations.ToArray();
                        message.Unfinished = childNode.Attributes.GetNamedItem("type")?.Value == "unfinished";
                        message.Translation = translations.ToArray();
                        message.Context = contextName;
                        messages.Add(message);
                    }
                }
            }
            translationFile.Messages = messages.ToArray();
            return translationFile;
        }
        
        public static byte[] Save(TranslationFile file) {
            throw new NotImplementedException();
        }
    }
}