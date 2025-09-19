using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ECommerceBackend.Models
{
    public class CartItemModel
    {
        [Key]
        public int CartItemId { get; set; }
        [Required]
        public int CartId { get; set; }
        [Required]
        public int Pdtid { get; set; }
        public int Quantity { get; set; }
        [JsonIgnore]
        public CartModel? Cart { get; set; }
        [JsonIgnore]
        public ProductModel? Product { get; set; }
    }
}
