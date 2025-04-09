using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace YnamarServer.Database.Models
{
    internal class Account
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public Character? Character { get; set; }
        public bool CharGender { get; set; }
        public string PasswordHash { get; set; }
    }
}
