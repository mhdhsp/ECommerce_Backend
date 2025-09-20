using ECommerceBackend.Models;

namespace ECommerceBackend.DTO___Mapping
{
    public class AllPdtsResDto
    {
        public List<ProductModel> ValidProducts { get; set; } 
        public List<ProductModel> InValidProducts { get; set; }
    }
}
