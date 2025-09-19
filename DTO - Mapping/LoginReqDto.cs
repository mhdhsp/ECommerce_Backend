using System.ComponentModel.DataAnnotations;

namespace ECommerceBackend.DTO___Mapping
{
    public class LoginReqDto
    {
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? PassWord { get; set; }
    }
}
