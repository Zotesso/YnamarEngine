using System.ComponentModel.DataAnnotations;

namespace YnamarServer.Database.Models
{
    internal class Account
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        public int CharId { get; set; }
        public bool CharGender { get; set; }
        public string PasswordHash { get; set; }
    }
}
