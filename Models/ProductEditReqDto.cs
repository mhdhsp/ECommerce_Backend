using System.ComponentModel.DataAnnotations;

namespace ECommerceBackend.Models
{
    public class ProductEditReqDto
    {
        public string? PdtName { get; set; }
        public int? Price { get; set; }
        public string? Gender { get; set; }
        public string? Color { get; set; }
        [Required]
        public string? Image { get; set; }
        public string? Description { get; set; }

        public int? Stock { get; set; }
    }
}
