using System.ComponentModel.DataAnnotations;

namespace JWTCreation.Models
{
    public class DataModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
