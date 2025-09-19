using ECommerceBackend.Data;
using ECommerceBackend.DTO___Mapping;
using ECommerceBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceBackend.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderModel>> GetAllOrders(int userId);
        Task<OrderModel?> OrderOneItem(int userId, int productId, int quantity);
        Task<OrderModel?> PlaceOrderForCart(int userId);
    }
    public class OrderService:IOrderService
    {
        private readonly AppDbContext _context;
        public OrderService(AppDbContext Context)
        {
            _context = Context;
        }

        public async Task<IEnumerable<OrderModel>> GetAllOrders(int userId)
        {
            var res = await _context.Orders.Include(s => s.OrderItems)
                .Where(x => x.UserId == userId).ToListAsync();
            return res;
        }


        public async Task<OrderModel?> OrderOneItem(int userId, int productId, int quantity)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.PdtId == productId);
            if (product == null || product.Stock < quantity) return null;

            var order = new OrderModel
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Amount = Convert.ToDecimal(product.Price * quantity),
                OrderItems = new List<OrderItemModel>
        {
            new OrderItemModel
            {
                ProductId = productId,
                Quantity = quantity
            }
        }
            };

            _context.Orders.Add(order);

            product.Stock -= quantity;

            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<OrderModel?> PlaceOrderForCart(int userId)
        {
            var cart = await _context.Cart
                .Include(c => c.CartItem)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItem.Any()) return null;

            var order = new OrderModel
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Amount = Convert.ToDecimal(cart.CartItem.Sum(ci => ci.Product != null ? ci.Product.Price * ci.Quantity : 0)),
                OrderItems = cart.CartItem.Select(ci => new OrderItemModel
                {
                    ProductId = ci.Pdtid,
                    Quantity = ci.Quantity
                }).ToList()
            };

            _context.Orders.Add(order);

            foreach (var i in cart.CartItem)
            {
                if (i.Product != null)
                {
                    i.Product.Stock -= i.Quantity;
                }
            }

            _context.CartItem.RemoveRange(cart.CartItem);

            await _context.SaveChangesAsync();
            return order;
        }


    }
}
