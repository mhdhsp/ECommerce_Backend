using ECommerceBackend.CommonApi;
using ECommerceBackend.DTO___Mapping;
using ECommerceBackend.Models;
using ECommerceBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace ECommerceBackend.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        public OrderController(IOrderService Service)
        {
            _service = Service;
        }

        [HttpGet("GetAllOrders")]
        public async Task<ActionResult> GetAllOrders()
        {
            var UserIdObj = HttpContext.Items["UserId"];
            if (UserIdObj == null)
                return Unauthorized(new CommonResponse<string?>(401, "Unauthorized Access", null));

            int UserId = Convert.ToInt32(UserIdObj);
            var res = await _service.GetAllOrders(UserId);

            if (res == null || !res.Any())
                return NotFound(new CommonResponse<object?>(404, "No orders yet", null));

            return Ok(new CommonResponse<object?>(200, "Orders fetched successfully", res));
        }

        [HttpPost("OrderSingle")]
        public async Task<ActionResult> PlaceSingleOrder([FromBody] OrderReqDto dto)
        {
            var UserIdObj = HttpContext.Items["UserId"];
            if (UserIdObj == null)
                return Unauthorized(new CommonResponse<string?>(401, "Unauthorized Access", null));

            int UserId = Convert.ToInt32(UserIdObj);
            var res = await _service.OrderOneItem(UserId, dto.ProductId, dto.Quantity);

            if (res == null)
                return NotFound(new { message = "Product not found or insufficient stock" });

            return Ok(new { message = "Order placed successfully", order = res });
        }

        [HttpPost("OrderCart")]
        public async Task<ActionResult> PlaceCartOrder()
        {
            var UserIdObj = HttpContext.Items["UserId"];
            if (UserIdObj == null)
                return Unauthorized(new CommonResponse<string?>(401, "Unauthorized Access", null));

            int UserId = Convert.ToInt32(UserIdObj);
            var res = await _service.PlaceOrderForCart(UserId);

            if (res == null)
                return NotFound(new { message = "Cart is empty or not found" });

            return Ok(new { message = "Order placed successfully", order = res });
        }

    }
}
