using ECommerceBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace ECommerceBackend.DTO___Mapping
{
    public class NewProductReqDto
    {
        public string? PdtName { get; set; }
        public int? Price { get; set; }
        public string? Gender { get; set; }
        public string? Color { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }

        public int? Stock { get; set; }
    }
}
