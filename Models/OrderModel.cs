using ECommerceBackend.Models;
using System.ComponentModel.DataAnnotations;

public class OrderModel
{
    [Key]
    public int OrderId { get; set; }

    public int UserId { get; set; }
    public UserModel? User { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal Amount { get; set; }

    public ICollection<OrderItemModel> OrderItems { get; set; } = new List<OrderItemModel>();
}