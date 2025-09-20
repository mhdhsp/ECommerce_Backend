using ECommerceBackend.CommonApi;
using ECommerceBackend.Data;
using ECommerceBackend.DTO___Mapping;
using ECommerceBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceBackend.Services
{
    public interface ICartService
    {
        Task<List<CartItemResDto>> GetAllCartItems(int userId);
        Task<CommonResponse<CartItemModel?>> AddToCart(CartItemReqDto item, int UserId);
        Task<CartItemModel?> DeleteCartItem(int Id);
        Task<CartItemModel?> UpdateQuantity(UpdateCartItem item);
        Task<bool> DealeteAllCartItems(int UserId);
    }
    public class CartService:ICartService
    {

        private readonly AppDbContext _context;
        public CartService(AppDbContext Context)
        {
            _context = Context;
        }
       
        public async Task<List<CartItemResDto>> GetAllCartItems(int userId)
        {
            var items = await _context.CartItem.
                Include(c=>c.Product)
                .Where(ci => ci.Cart.UserId == userId)
                .Select(x=>new CartItemResDto
                {
                    itemId=x.CartItemId,
                    Pdtid = x.Pdtid,
                    Quantity = x.Quantity,

                    PdtName = x.Product.PdtName,

                    Price = x.Product.Price,

                    Gender = x.Product.Gender,

                    Color = x.Product.Color,

                    Image = x.Product.Image,

                    Description = x.Product.Description,

                    Stock = x.Product.Stock
                })

                .ToListAsync();

            return items;
        }



        public async Task<CommonResponse<CartItemModel?>> AddToCart(CartItemReqDto item,int UserId)
        {
            var cart = await _context.Cart.FirstOrDefaultAsync(x => x.UserId == UserId);
            if (cart == null)
            {
                var newCart = new CartModel
                {
                    UserId = UserId
                };
                await _context.Cart.AddAsync(newCart);
                await _context.SaveChangesAsync();
                
            }
            cart = await _context.Cart.FirstOrDefaultAsync(x => x.UserId == UserId);
            var existing = await _context.CartItem.FirstOrDefaultAsync(x => x.CartId == cart.CartId && x.Pdtid == item.PdtId);
            if (existing != null)
                return new CommonResponse<CartItemModel?>(400, "item already exist in cart", null);
            var newItem = new CartItemModel
            {
                CartId = cart.CartId,
                Pdtid = item.PdtId,
                Quantity = item.Quantity
            };
            
            await _context.AddAsync(newItem);
            await _context.SaveChangesAsync();

            return new CommonResponse<CartItemModel?>(201,"item added succefuly",newItem);
            
        }


        public async Task<CartItemModel?> DeleteCartItem(int Id)
        {
            var existing =await  _context.CartItem.FirstAsync(x => x.CartItemId == Id);
            if (existing == null)
                return null;
             _context.CartItem.Remove(existing);
            await _context.SaveChangesAsync();
            return existing;

        }

        public async Task<CartItemModel?> UpdateQuantity(UpdateCartItem item)
        {
             var existing =await  _context.CartItem.FirstOrDefaultAsync(x => x.CartItemId == item.CartItemId);
            if (existing == null)
                return null;
            existing.Quantity = item.Quantity;
            await _context.SaveChangesAsync();
            return existing;
        }


        public async Task<bool> DealeteAllCartItems(int UserId)
        {
            var cart =await _context.Cart.FirstOrDefaultAsync(x => x.UserId == UserId);
            if (cart == null)
                return false;
             _context.Cart.Remove(cart);
           await  _context.SaveChangesAsync();
            return true;
            
        }
    }
}
