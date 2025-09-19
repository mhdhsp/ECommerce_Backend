using System.ComponentModel.DataAnnotations;

namespace ECommerceBackend.DTO___Mapping
{
    public class UserReqDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? PassWord { get; set; }
    }
}
