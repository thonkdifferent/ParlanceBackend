using System;
using System.IO;
using Microsoft.Extensions.Options;

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
            return $"{GitRepositoryPath.Replace("{UserFolder}", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))}/repos/{slug}";
        }

        public static string Slugify(string name) {
            return name.ToLower().Replace(" ", "-")
            .Replace("(", "")
            .Replace(")", "");
        }
    }
}