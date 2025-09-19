using ECommerceBackend.DTO___Mapping;
using ECommerceBackend.Models;
using ECommerceBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace ECommerceBackend.Controllers
{
    [Authorize(Roles ="User")]
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _service;
        public WishListController(IWishListService Service)
        {
            _service = Service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishListItemResDto>>> GetAllItems(int UserId)
        {
            var res =await  _service.GetAllItems(UserId);
           if(!res.Any())
                return NotFound(new {message="No wishlist Items found"});
            return Ok(res);
        }
        [HttpPost]

        public async Task<ActionResult<WishListModel>>  AddToWishList(WishListItemReqDto itemDto)
        {
            var res =await  _service.AddToWishList(itemDto);
            if (res == null)
                return BadRequest(new { message = "item not added" });
            return CreatedAtAction(nameof(GetAllItems), new { Id = res.ListId }, res);
        }

        [HttpDelete]
        public async Task<ActionResult<WishListModel>> RemoveFromWishList(WishListItemReqDto itemDto)
        {
            var res =await _service.RemoveFromWishList(itemDto);
            if (res == null)
                return NotFound(new { message = "Item not Found" });
            return Ok(new { message = "Item deleted", data = res });
        }
    }
}
