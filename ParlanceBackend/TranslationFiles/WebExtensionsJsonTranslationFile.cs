using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ParlanceBackend.Models;

namespace ParlanceBackend.TranslationFiles {
    
    class WebExtensionsJsonTranslationFile : ITranslationFileFormat {
        class Translation
        {
            public class Placeholder
            {
                [JsonPropertyName("content")]
                public string Content { get; set; }
                
                [JsonPropertyName("example")]
                public string Example { get; set; }
            }
            
            [JsonPropertyName("message")]
            public string Message { get; set; }
            
            [JsonPropertyName("description")]
            public string Description { get; set; }
            
            [JsonPropertyName("placeholders")]
            public Dictionary<string, Placeholder> Placeholders { get; set; }
        }
        
        public async Task<TranslationFile> LoadFromBytes(byte[] bytes, byte[] baseFile) {
            //Create a translation file and read in the information

            var file = new TranslationFile();
            await using var byteStream = new MemoryStream(bytes);
            await using var baseByteStream = new MemoryStream(baseFile);
            // var jsonFile =  await JsonDocument.ParseAsync(byteStream);
            //
            // var file = new TranslationFile();
            //
            //
            //
            // file.Messages = jsonFile.RootElement.EnumerateObject().Select(obj =>
            // {
            //     
            // })
            var jsonFile = await JsonSerializer.DeserializeAsync<Dictionary<String, Translation>>(byteStream, new JsonSerializerOptions
            {
            });
            var baseJsonFile = await JsonSerializer.DeserializeAsync<Dictionary<String, Translation>>(baseByteStream, new JsonSerializerOptions
            {
            });
            
            file.Messages = jsonFile!.Select(message =>
            {
                return new Message
                {
                    Source = baseJsonFile![message.Key].Message,
                    Key = message.Key,
                    Comment = message.Value.Description,
                    Context = "DefaultContext",
                    Translation = new[]
                    {
                        message.Value.Message
                    }
                };
            }).ToArray();

            return file;
        }
        
        public async Task<byte[]> Save(TranslationFile file)
        {
            var dictionary = file.Messages.ToDictionary(message => message.Key, 
                message => new Translation()
                {
                    Message = message.Translation[0],
                    Description = message.Comment
                });

            await using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, dictionary, new JsonSerializerOptions
            {
                // DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                // PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            });

            return stream.ToArray();
        }

        public async Task Update(string fileName, string baseFileName, TranslationDelta delta)
        {
            var file = await LoadFromBytes(await File.ReadAllBytesAsync(fileName), await File.ReadAllBytesAsync(baseFileName));
            foreach (var message in file.Messages)
            {
                if (message.Source == delta.Key) message.Translation = delta.Translations;
            }

            await File.WriteAllBytesAsync(fileName, await Save(file));
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