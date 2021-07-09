using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
namespace ParlanceBackend.TranslationFiles {
    class QtTranslationFile {
        static TranslationFile LoadFromFile(string FileName) {
            return LoadFromBytes(File.ReadAllBytes(FileName));
        }

        static TranslationFile LoadFromBytes(byte[] bytes) {
            //Create a translation file and read in the information
            XmlDocument document = new();
            string xml = Encoding.UTF8.GetString(bytes);
            document.Load(xml);

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
                        contextName = childNode.Value;
                    if (childNode.Name == "message")
                    {
                        Message message = new();
                        List<Location> locations = new();
                        List<string> translations = new();
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
                                    message.Source = messageMeta.Value;
                                    break;
                                case "translation":
                                    if (childNode.Attributes.GetNamedItem("numerous").Value == "yes")
                                    {
                                        foreach(XmlNode translation in messageMeta.ChildNodes)
                                        {
                                            translations.Add(translation.Value);
                                        }
                                    }
                                    else
                                    {
                                        translations.Add(messageMeta.Value);
                                    }
                                    break;

                            }
                        }
                        message.Location = locations.ToArray();
                        message.Translation = new Translation
                        {
                            Unfinished = childNode.Attributes.GetNamedItem("type").Value == "unfinished",
                            Content = translations.ToArray()
                        };
                        message.Context = contextName;
                        messages.Add(message);
                    }
                }
            }
            translationFile.Messages = messages.ToArray();
            return translationFile;
        }
        
        static byte[] Save(TranslationFile file) {
            throw new NotImplementedException();
        }
    }
}