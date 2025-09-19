using ECommerceBackend.CommonApi;
using ECommerceBackend.Data;
using ECommerceBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceBackend.Services
{
    public interface IAppProductService
    {
        Task<ICollection<ProductModel>> GetAllProducts();
        Task<ProductModel?> GetById(int Id);
        Task<ICollection<ProductModel>> GetByGender(string Gender);
        Task<CommonResponse<ProductModel?>> EditProduct(int Id, ProductEditReqDto item);
    }
    public class AppProductService:IAppProductService
    {
        private readonly AppDbContext _context;
        public AppProductService(AppDbContext Context)
        {
            _context = Context;
        }

        public async Task<ICollection<ProductModel>> GetAllProducts()
        {
            var res =await  _context.Products.Include(p=>p.Sizes).ToListAsync();
            return res;
        }

        public  async Task<ProductModel?> GetById(int Id)
        {
            var res =   await _context.Products.Include(p => p.Sizes).FirstOrDefaultAsync(x => x.PdtId == Id);
            return res;
        }

        public async Task<ICollection<ProductModel>> GetByGender(string Gender)
        {
            var res = await _context.Products
                    .Where(x => x.Gender ==Gender.ToLower())
                     .Include(p => p.Sizes)
                     .ToListAsync();

            return res;
        }


        public async Task<CommonResponse<ProductModel?>> EditProduct(int Id,ProductEditReqDto item)
        {
            var product = await _context.Products.FindAsync(Id);
            if (product == null)
                return new CommonResponse<ProductModel?>(404, "Product not found", null);

            if(!string.IsNullOrEmpty(item.PdtName))
                product.PdtName = item.PdtName;

            if(item.Price.HasValue)
                product.Price = item.Price;

            if(!string.IsNullOrEmpty(item.Gender))
                product.Gender = item.Gender;

            if(!string.IsNullOrEmpty(item.Color))
                product.Color = item.Color;

            if(item.Stock.HasValue)
                product.Stock = item.Stock;

            if(!string.IsNullOrEmpty(item.Image))
                product.Image = item.Image;


            if (!string.IsNullOrEmpty(item.Description))
                product.Description = item.Description;

            await _context.SaveChangesAsync();

            return new CommonResponse<ProductModel?>(200, "Product updated succesfully", product);

        }
    }
}
