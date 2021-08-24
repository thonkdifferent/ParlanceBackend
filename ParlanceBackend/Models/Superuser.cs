using System.ComponentModel.DataAnnotations;

namespace ParlanceBackend.Models
{
    public class Superuser
    {
        [Key]
        public ulong UserId { get; set; }
    }
}