using ECommerceBackend.CommonApi;
using ECommerceBackend.Data;
using ECommerceBackend.DTO___Mapping;
using Microsoft.EntityFrameworkCore;

namespace ECommerceBackend.Services.DashBoard
{
    public class DashBoardService:IDashBoardService
    {
        private readonly AppDbContext _context;
        public DashBoardService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CommonResponse<DashBoardDto?>> GetDashBoardData()
        {

            try
            {
                int validUsers = await _context.Users.CountAsync(x => x.Blocked == false);
                int Invalidusers = await _context.Users.CountAsync(x => x.Blocked == true);

                var ValidPdts = await _context.Products.CountAsync(x => x.Suspend == false);
                var InValidPdts = await _context.Products.CountAsync(x => x.Suspend == true);

                var revenue = await _context.Orders.SumAsync(x => (decimal?)x.Amount) ?? 0;

                int Orders = await _context.Orders.CountAsync();

                var res = new DashBoardDto
                {
                    ValidUsers = validUsers,
                    InValidUsers = Invalidusers,
                    ValidProducts = ValidPdts,
                    InValidProducts = InValidPdts,
                    TotalRevenue = revenue,
                    TotalOrders = Orders
                };

                return new CommonResponse<DashBoardDto?>(200, "Dashboard data", res);
            }
            catch(Exception ex)
            {
                return new CommonResponse<DashBoardDto?>(500, $"Error:{ex.Message}", null);
            }


        }
    }
}
