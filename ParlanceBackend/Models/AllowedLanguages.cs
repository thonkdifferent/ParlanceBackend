using System.ComponentModel.DataAnnotations;

namespace ParlanceBackend.Models
{
    public class AllowedLanguages
    {
        [Key]
        public ulong UserId { get; set; }
        public Language Language { get; set; }
    }
}