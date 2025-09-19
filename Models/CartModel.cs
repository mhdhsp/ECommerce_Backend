using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ECommerceBackend.Models
{
    public class CartModel
    {
        [Key]
        public int CartId { get; set; }
        [Required]
        public int UserId { get; set; }
        public UserModel? User { get; set; }
        [JsonIgnore]
        public ICollection<CartItemModel>? CartItem { get; set; }

    }
}
