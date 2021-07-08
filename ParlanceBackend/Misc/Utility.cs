using System;
using System.IO;
using Microsoft.Extensions.Options;
using ParlanceBackend;

namespace ParlanceBackend.Misc
{
    public class Utility
    {
        public static string GetDirectoryFromSlug(string slug, IOptions<ParlanceConfiguration> configuration)
        {
            return GetDirectoryFromSlug(slug, configuration.Value.GitRepository);
        }
        public static string GetDirectoryFromSlug(string slug, string GitRepositoryPath)
        {
            return $"{Parse(GitRepositoryPath)}/repos/{slug}";
        }

        public static string Slugify(string name) {
            return name.ToLower().Replace(" ", "-")
            .Replace("(", "")
            .Replace(")", "");
        }
        private static string Parse(string input)
        {
            bool startVarParse = false;
            string output = "";
            string varName = "";
            for(int i=0;i<input.Length;i++)
            {
                if(input[i]=='{' && !startVarParse)
                {
                    startVarParse = true;
                    continue;
                }
                if(startVarParse)
                {
                    if(input[i]=='}')
                    {
                        output += varName switch
                        {
                            "CONFIG_FOLDER" => Constants.CONFIGURATION_FOLDER,
                            "USER_FOLDER" => Constants.USER_FOLDER,
                            "DOCS_FOLDER" => Constants.DOCUMENTS_FOLDER,
                            _ => $"{{{varName}}}",
                        };
                        varName = "";
                        continue;
                    }
                    else
                        varName += input[i];
                }
                output += input[i];
            }
            return output;
        }
    }
}