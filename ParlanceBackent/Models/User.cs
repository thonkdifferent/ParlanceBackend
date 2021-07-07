using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParlanceBackend.Models
{
    public class User
    {
        [Key]
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsSuperUser { get; set; }
        // public IEnumerable<Project> ProjectAdministrator { get; set; }
        public IEnumerable<string> Languages { get; set; }
        public IEnumerable<string> AuthenticationTokens { get; set; }
    }
}
