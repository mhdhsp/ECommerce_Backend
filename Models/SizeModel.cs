using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ECommerceBackend.Models
{
    public class SizeModel
    {
        [Key]
        public int SizeId { get; set; }
        public string? Value { get; set; }
        public int PdtId { get; set; }
        [JsonIgnore]
        public ProductModel? Product { get; set; }
    }
}
