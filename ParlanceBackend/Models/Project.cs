using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ParlanceBackend.Models
{
    public class ProjectPrivate
    {
        [Key]
        public string Name { get; set; }
        public string GitCloneUrl { get; set; }
        public string Slug { get; set; }
        public string Branch { get; set; }


        public void Clone(string GitRepository) {
            //Clone the repository

            //Generate a slug from the project name
            Slug = Name.ToLower().Replace(" ", "-");
            Console.WriteLine($"Parlance Configuration Repository: {GitRepository}");

            string repoPath = RepositoryPath();
        }

        public string RepositoryPath() {
            return GitRepositoryForSlug(Slug);
        }
        
        static string GitRepositoryForSlug(string slug) {
            // Get something from the configuration


            string gitRepositoryDirectory = "{0}/Parlance"; //TODO: Replace with something from the site config
            string.Format(gitRepositoryDirectory, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

            if (!System.IO.Directory.Exists(gitRepositoryDirectory)) {
                System.IO.Directory.CreateDirectory(gitRepositoryDirectory);
            }

            gitRepositoryDirectory += "/" + slug;
            return gitRepositoryDirectory;
        }

        public static Project CreateProject(ProjectPrivate project) =>
        new Project
        {
            Name = project.Name,
            GitCloneUrl = project.GitCloneUrl,
            Branch = project.Branch
        };
    }

    public class Project
    {
        [Key]
        public string Name { get; set; }
        public string GitCloneUrl { get; set; }
        public string Branch { get; set; }

    }

}
