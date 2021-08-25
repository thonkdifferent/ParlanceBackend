using System.ComponentModel.DataAnnotations;

namespace ParlanceBackend.Models
{
    public class AllowedLanguages
    {
        [Key]
        public ulong UserId { get; set; }
        public Language Language { get; set; }
    }

    public class AllowedLanguagesPublic
    {
        public string UserName { get; set; }
        public string Language { get; set; }
    }
}