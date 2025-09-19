using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ECommerceBackend.Models
{
    public class WishListModel
    {
        [Key]
        public int ListId { get; set; }
        public int UserId { get; set; }
        public int PdtId { get; set; }

        [JsonIgnore]
        public ProductModel? Product { get; set; }
    }
}
