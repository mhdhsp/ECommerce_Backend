using System.ComponentModel.DataAnnotations;

namespace ECommerceBackend.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? PassWord { get; set; }
        public string? Role { get; set; } = "User";
        public CartModel? Cart { get; set; }

        public bool Blocked { get; set; } = false;
        public string? Blocked_By { get; set; } = null;
        public DateTime? Blocked_On { get; set; } = null;
    }
}
