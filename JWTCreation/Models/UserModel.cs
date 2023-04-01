using System.ComponentModel.DataAnnotations;

namespace JWTCreation.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
