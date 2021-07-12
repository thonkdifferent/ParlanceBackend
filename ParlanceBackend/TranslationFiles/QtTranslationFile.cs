using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using ParlanceBackend.Models;

namespace ParlanceBackend.TranslationFiles {
    class QtTranslationFile {
        public static TranslationFile LoadFromFile(string FileName) {
            return LoadFromBytes(File.ReadAllBytes(FileName));
        }

        private static XmlDocument ForEachMessageNode(byte[] bytes, Action<XmlNode, string, XmlDocument> callback)
        {
            XmlDocument document = new();
            string xml = Encoding.UTF8.GetString(bytes);
            document.LoadXml(xml);
            
            var contexts = document.GetElementsByTagName("context");
            foreach (XmlNode context in contexts)
            {
                var unprocessedNodes = new List<XmlNode>();
                
                var contextName = "";
                foreach(XmlNode childNode in context.ChildNodes)
                {
                    if (childNode.Name == "name")
                    {
                        contextName = childNode.InnerText;
                    }
                    else
                    {
                        unprocessedNodes.Add(childNode);
                        callback(childNode, contextName, document);
                    }
                }
                
                foreach (var node in unprocessedNodes)
                {
                    callback(node, contextName, document);
                }
            }

            return document;
        }

        static TranslationFile LoadFromBytes(byte[] bytes) {
            //Create a translation file and read in the information

            TranslationFile translationFile = new();
            List<Message> messages = new();
            
            XmlDocument document = ForEachMessageNode(bytes, (childNode, context, _) =>
            {
                if (childNode.Name != "message") return;
                
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
                message.Context = context;
                messages.Add(message);
            });
            
            translationFile.DestinationLanguage = document.GetElementsByTagName("TS").Item(0).Attributes.GetNamedItem("language").Value;
            translationFile.Messages = messages.ToArray();
            return translationFile;
        }
        
        public static void Update(string fileName, TranslationDelta delta) {
            File.WriteAllBytes(fileName, Update(File.ReadAllBytes(fileName), delta));
        }

        public static byte[] Update(byte[] originalFile, TranslationDelta delta)
        {
            XmlDocument document = ForEachMessageNode(originalFile, (node, context, document) =>
            {
                if (context != delta.Context) return;

                XmlNode translationsNode = null;
                foreach (XmlNode messageMeta in node.ChildNodes)
                {
                    switch(messageMeta.Name)
                    {
                        case "source":
                            if (messageMeta.InnerText != delta.Key) return;
                            break;
                        case "translation":
                            translationsNode = messageMeta;
                            break;
                        case "comment":
                            //TODO
                            // message.Comment = messageMeta.InnerText;
                            break;
                    }
                }

                if (translationsNode == null) return;

                //Update the translation file
                if (delta.Translations.Length == 1)
                {
                    translationsNode.InnerText = delta.Translations[0];
                }
                else
                {
                    translationsNode.RemoveAll();
                    foreach (string translation in delta.Translations)
                    {
                        XmlNode numNode = document.CreateNode(XmlNodeType.Element, "numerus", null);
                        numNode.InnerText = translation;
                        translationsNode.AppendChild(numNode);
                    }
                }
            });
            byte[] retData;
            using (MemoryStream stream = new())
            {
                using (var writer = XmlWriter.Create(stream, new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false)
                }))
                {
                    document.Save(writer);
                }
                stream.Flush();
                retData = stream.ToArray();
            }
            return retData;
        }
        
        public static byte[] Save(TranslationFile file) {
            throw new NotImplementedException();
        }
    }
}