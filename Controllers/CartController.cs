using AutoMapper;
using ECommerceBackend.CommonApi;
using ECommerceBackend.DTO___Mapping;
using ECommerceBackend.Models;
using ECommerceBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBackend.Controllers
{
    [Authorize(Roles = "User")]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;
        private readonly IMapper _mapper;
        public CartController(ICartService Service,IMapper mapper)
        {
            _service = Service;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CartItemModel>>> GetAllCartItems()
        {
            var  UserIdObj = HttpContext.Items["UserId"];
            if (UserIdObj == null)
                return Unauthorized(new CommonResponse<string?>(401, "UnAuthorized User", null));
            int UserId = Convert.ToInt32(UserIdObj);
            var res = await _service.GetAllCartItems(UserId);
            if (res == null|| !res.Any())
                return NotFound();
            return Ok(res);
        }

        [HttpPost("AddToCart")]
        public async  Task<ActionResult> AddToCart([FromBody] CartItemReqDto itemDto)
        {
            var UserIdObj = HttpContext.Items["UserId"];
            if (UserIdObj == null)
                return Unauthorized(new CommonResponse<string?>(401, "UnAuthorized User", null));
            int UserId = Convert.ToInt32(UserIdObj);
            var res = await _service.AddToCart(itemDto,UserId);
            return res.StatusCode switch
            {
                400 => NotFound(res),
                201 => CreatedAtAction(nameof(GetAllCartItems), new { Id = itemDto.PdtId }, res),
                _ => BadRequest(res)

            };
        }


        [HttpDelete("{ItemId}")]
        public async Task<ActionResult> DeleteCartItem( int  ItemId)
        {
            var res = await _service.DeleteCartItem(ItemId);
            if (res == null)
                return NotFound(new CommonResponse<string?>(404,"unable to delete",null));
            return Ok(new CommonResponse<Object?>(204,"ItemDeleted",res));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateQuantity(UpdateCartItem item)
        {
            var res = await _service.UpdateQuantity(item);
            if (res == null)
                return NotFound(new CommonResponse<string?>(404,"item not found",null));
            return Ok(new CommonResponse<Object?>(200, "Updated Succesfully", res));
        }

        [HttpDelete("DeleteAllItems")]
        public async Task<ActionResult> DeleteCart()
        {
            var userIdObj = HttpContext.Items["UserId"];
            if (userIdObj == null)
                return Unauthorized(new CommonResponse<string?>(401, "Unautherised user", null));
            int userId = Convert.ToInt32(userIdObj);
            var result = await _service.DealeteAllCartItems(userId);
            if (!result)
                return NotFound(new CommonResponse<string?>(404,"Cart not found",null));

            return Ok(new CommonResponse<string?>(200,"cart deleted",null));
        }
    }
}
