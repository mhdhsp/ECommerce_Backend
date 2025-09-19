using ECommerceBackend.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ECommerceBackend.DTO___Mapping
{
    public class CartItemResDto
    {
        public int itemId { get; set; }
        public int Pdtid { get; set; }
        public int Quantity { get; set; }
        public string? PdtName { get; set; }
        public int? Price { get; set; }
        public string? Gender { get; set; }
        public string? Color { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public int? Stock { get; set; }
    }
}
