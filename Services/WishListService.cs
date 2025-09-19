using AutoMapper;
using ECommerceBackend.Data;
using ECommerceBackend.DTO___Mapping;
using ECommerceBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceBackend.Services
{
    public interface IWishListService
    {
         Task<IEnumerable<WishListItemResDto>> GetAllItems(int userId);
        Task<WishListModel?> AddToWishList(WishListItemReqDto itemDto);
        Task<WishListModel?> RemoveFromWishList(WishListItemReqDto itemDto);
    }
    public class WishListService:IWishListService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public WishListService(AppDbContext Context,IMapper mapper)
        {
            _context = Context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WishListItemResDto>> GetAllItems(int userId)
        {
            var items = await  _context.WishList.Include(s => s.Product).Where(x => x.UserId == userId)
                .Select(d => new WishListItemResDto
                {
                    PdtName = d.Product.PdtName,
                    Price = d.Product.Price,
                    Gender = d.Product.Gender,
                    Color = d.Product.Color,
                    Image = d.Product.Image,
                    Description = d.Product.Description,
                    Stock = d.Product.Stock,
                    Suspend = d.Product.Suspend
                }).ToListAsync();
            return items ?? new List<WishListItemResDto>();
        }


        public async Task<WishListModel?> AddToWishList(WishListItemReqDto itemDto)
        {
            var item = _mapper.Map<WishListModel>(itemDto);
            var existing = await _context.WishList.FirstOrDefaultAsync(x => x.UserId == item.UserId && x.PdtId == item.PdtId);
            if (existing != null)
                return existing;
            await _context.WishList.AddAsync(item);
            await _context.SaveChangesAsync();
            return item ;
        }

        public async Task<WishListModel?> RemoveFromWishList(WishListItemReqDto itemDto)
        {
            var existing = await _context.WishList.FirstOrDefaultAsync(x => x.UserId == itemDto.UserId && x.PdtId == itemDto.PdtId);
            if (existing == null)
                return null;
             _context.WishList.Remove(existing);
            await _context.SaveChangesAsync();
            return existing;
        }

    }

}
