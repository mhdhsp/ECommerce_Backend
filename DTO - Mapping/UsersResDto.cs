using ECommerceBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace ECommerceBackend.DTO___Mapping
{
    public class UsersResDto
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }

        public bool Blocked { get; set; } = false;
        public string? Blocked_By { get; set; } = null;
        public DateTime? Blocked_On { get; set; } = null;
    }
}
