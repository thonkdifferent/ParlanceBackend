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
        
        public static string Parse(string input)
        {
            return input.Replace("{UserFolder}", Constants.USER_FOLDER)
                .Replace("{ConfigFolder}", Constants.CONFIGURATION_FOLDER)
                .Replace("{DocsFolder}", Constants.DOCUMENTS_FOLDER);
        }
    }
}