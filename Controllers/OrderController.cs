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
        [Authorize(Roles = "User")]
        public async Task<ActionResult> GetAllOrders()
        {
            var UserIdObj = HttpContext.Items["UserId"];
            if (UserIdObj == null)
                return Unauthorized(new CommonResponse<string?>(401, "Unauthorized Access", null));

            int UserId = Convert.ToInt32(UserIdObj);
            var res = await _service.GetAllOrders(UserId);

            return res.StatusCode switch
            {
                404 => NotFound(res),
                200 => Ok(res),
                _ => BadRequest(res)
            };
        }

        [HttpPost("OrderSingle")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult> PlaceSingleOrder([FromBody] OrderReqDto dto)
        {
            var UserIdObj = HttpContext.Items["UserId"];
            if (UserIdObj == null)
                return Unauthorized(new CommonResponse<string?>(401, "Unauthorized Access", null));

            int UserId = Convert.ToInt32(UserIdObj);
            var res = await _service.OrderOneItem(UserId, dto.ProductId, dto.Quantity);

            return res.StatusCode switch
            {
                404 => NotFound(res.Message),
                200 => Ok(res),
                _ => BadRequest(res)
            };
        }

        [HttpPost("OrderCart")]
        [Authorize(Roles = "User")]
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


        [HttpGet("TotalRevenew")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<CommonResponse<decimal?>>> GetTotalRevenue()
        {
            var res = await _service.GetTotalRevenue();

            return res.StatusCode switch
            {
                404 => NotFound(res),
                200 => Ok(res),
                _ => BadRequest(res)
            };
        }


    }
}
