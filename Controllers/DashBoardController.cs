using ECommerceBackend.Services.DashBoard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class DashBoardController : ControllerBase
    {
        private readonly IDashBoardService _service;
        public DashBoardController(IDashBoardService service)
        {
            _service = service;
        }

        [HttpGet("DashBoardData")]
        public async Task<ActionResult> GetDashBoradData()
        {
            var res = await _service.GetDashBoardData();
            return res.StatusCode switch
            {
                200 => Ok(res),
                500 => StatusCode(500, res),
                _ => BadRequest(res)
            };
        }
    }
}
