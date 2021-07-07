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

        public void Clone() {
            //Clone the repository

            //Generate a slug from the project name
            this.Slug = Name.ToLower().Replace(" ", "-");
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
