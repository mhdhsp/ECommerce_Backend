using ECommerceBackend.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class OrderItemModel
{
    [Key]
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }
    [JsonIgnore]
    public OrderModel? Order { get; set; }

    public int ProductId { get; set; }
    public ProductModel? Product { get; set; }

    public int Quantity { get; set; }
}