using System.ComponentModel.DataAnnotations;

namespace ParlanceBackend.Models
{
    public class AllowedProjects
    {
        public ulong UserId { get; set; }
        public ProjectPrivate Project { get; set; }
    }
}