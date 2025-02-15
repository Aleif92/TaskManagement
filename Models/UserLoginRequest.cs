using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Models
{
    public class UserLoginRequest
    {
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }

    }
}
