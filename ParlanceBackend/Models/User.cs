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

    public class CreateUserData
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class TokenData
    {
        public string Token { get; set; }
        public string Error { get; set; }
    }

    public class ProvisionTokenData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string OtpToken { get; set; }
        public string NewPassword { get; set; }
    }

    public class UserInformationData
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public bool Verified { get; set; }
    }

    public class PermissionsData
    {
        public bool Superuser { get; set; }
        public List<Language> AllowedLanguages { get; set; }
        public List<Project> AllowedProjects { get; set; }
    }
}
