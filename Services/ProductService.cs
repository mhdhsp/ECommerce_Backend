using AutoMapper;
using ECommerceBackend.CommonApi;
using ECommerceBackend.Data;
using ECommerceBackend.DTO___Mapping;
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
        Task<CommonResponse<ProductModel?>> AddNewProduct(NewProductReqDto itemDto);
        Task<CommonResponse<ProductModel?>> ToggleSuspend(int Id);
        Task<CommonResponse<ProductModel?>> DeleteProduct(int Id);
    }
    public class AppProductService:IAppProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public AppProductService(AppDbContext Context,IMapper mapper)
        {
            _context = Context;
            _mapper = mapper;
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

        public async Task<CommonResponse<ProductModel?>> AddNewProduct(NewProductReqDto itemDto)
        {
            if (itemDto == null)
                return new CommonResponse<ProductModel?>(400, "Invalid process request", null);

            var item = _mapper.Map<ProductModel>(itemDto);
            await _context.AddAsync(item);
            await _context.SaveChangesAsync();
            return new CommonResponse<ProductModel?>(201, "product added succesfully", item);
        }

        public async Task<CommonResponse<ProductModel?>> ToggleSuspend(int Id)
        {
            var product =await  _context.Products.FindAsync(Id);
            if (product == null)
                return new CommonResponse<ProductModel?>(404, "product not found", null);
            if(product.Suspend==true)
            {
                product.Suspend = false;
                 await  _context.SaveChangesAsync();
                return new CommonResponse<ProductModel?>(200, "product Unsuspended", product);
            }
            product.Suspend = true;
            await _context.SaveChangesAsync();
            return new CommonResponse<ProductModel?>(200, "product suspended", product);
        }


        public async Task<CommonResponse<ProductModel?>> DeleteProduct(int Id)
        {
            var product =await  _context.Products.FindAsync(Id);
            if (product == null)
                return new CommonResponse<ProductModel?>(404, "Product not found", null);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return new CommonResponse<ProductModel?>(200, "product deletd succesfully", product);
        }
    }
}
