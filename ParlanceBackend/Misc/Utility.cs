using System;
using System.IO;

namespace ParlanceBackend.Misc
{
    public class Utility
    {
        public static string GetDirectoryFromSlug(string slug, string rootPath)
        {
            string gitRepositoryDirectory = $"{rootPath}/repos/{slug}";
            try
            {
                if (!Directory.Exists(gitRepositoryDirectory))
                {
                    Directory.CreateDirectory(gitRepositoryDirectory);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"Something went wrong {e.Message}.\n Stacktrace {e.StackTrace}");
            }
            
            return gitRepositoryDirectory;
        }
    }
}