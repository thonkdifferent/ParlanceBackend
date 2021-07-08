using System;
using System.IO;

namespace ParlanceBackend.Misc
{
    public class Utility
    {
        public static string GetDirectoryFromSlug(string slug, string rootPath)
        {
            return $"{rootPath.Replace("{UserFolder}", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))}/repos/{slug}";
        }
    }
}