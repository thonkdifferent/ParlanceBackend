using System;
using System.ComponentModel.DataAnnotations;

namespace ParlanceBackend.Models
{
    public class Language : IEquatable<Language>
    {
        [Key]
        public string Identifier { get; set; }
        public string Name { get; set; }

        public bool Equals(Language other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Identifier == other.Identifier;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Language) obj);
        }

        public override int GetHashCode()
        {
            return (Identifier != null ? Identifier.GetHashCode() : 0);
        }
    }
}