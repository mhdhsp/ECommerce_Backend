using ECommerceBackend.CommonApi;
using ECommerceBackend.DTO___Mapping;

namespace ECommerceBackend.Services.DashBoard
{
    public interface IDashBoardService
    {
        Task<CommonResponse<DashBoardDto?>> GetDashBoardData();
    }
}
