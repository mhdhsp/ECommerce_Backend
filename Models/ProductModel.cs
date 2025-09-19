using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ECommerceBackend.Models
{
    public class ProductModel
    {
        [Key]
        public int PdtId { get; set; }
        [Required]
        public string? PdtName { get; set; }
        public int? Price { get; set; }
        [Required]
        public string? Gender { get; set; }
        public string? Color { get; set; }
        [Required]
        public string? Image { get; set; }
        public string? Description { get; set; }
       
        public int? Stock { get; set; }
        public bool? Suspend { get; set; } = false;

        public ICollection<CartItemModel>? CartItem { get; set; }
        public ICollection<SizeModel>? Sizes { get; set; }
        public ICollection<WishListModel>? WishList { get; set; }

        public ICollection<OrderItemModel>? OrderItems { get; set; }

    }
}
