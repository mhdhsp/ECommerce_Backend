using ECommerceBackend.CommonApi;
using ECommerceBackend.DTO___Mapping;
using ECommerceBackend.Models;
using ECommerceBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBackend.Controllers
{

    
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IAppProductService _service;
        public ProductController(IAppProductService Service)
        {
            _service = Service;
        }


       
        [HttpGet("GetAvailProducts")]
        
        public async Task<ActionResult<ProductModel>> GetAllProducts([FromQuery(Name ="_limit")]int limit=0)
        {
            var res = await _service.GetAvailProducts();
            if (res == null)
                return NotFound(new { message = "No products found" });



            if (limit != 0)
                res=res.Take(limit).ToList();
            return Ok(res);
        }

        
        [HttpGet("GetById/{Id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductModel>> GetById([FromRoute]int Id)
        {
            var res = await _service.GetById(Id);
            if (res == null)
                return NotFound(new { message = $"Product with Id {Id} not found." });
            return Ok(res);
        }


        [HttpGet("GetByCategory/{Gender}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetByGender(string Gender)
        {
            var res = await _service.GetByGender(Gender);
            if (res == null)
                return NotFound();
            return Ok(res);
        }

        [HttpPatch("EditProduct/{Id}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult>  EditProduct(int Id, [FromBody]ProductEditReqDto item)
        {
            var res = await _service.EditProduct(Id, item);
            return res.StatusCode switch
            {
                404 => NotFound(res.Message),
                200 => Ok(res),
                _=>BadRequest(res)
            };
        }

        [HttpPost("AddNewProduct")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> AddNewProduct(NewProductReqDto NewItem)
        {
            var res = await _service.AddNewProduct(NewItem);
            return res.StatusCode switch
            {
                201 => CreatedAtAction(nameof(GetById), new { Id = res.Data.PdtId }, res),
                400 => BadRequest(res.Message),
                _ => BadRequest(res)
            };
        }


        [HttpPatch("ToggleSuspend/{Id}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> ToggleSuspend(int Id)
        {
            var res = await _service.ToggleSuspend(Id);
            return res.StatusCode switch
            {
                404 => NotFound(res),
                200 => Ok(res),
                _ => BadRequest(res)
            };
        }

        [HttpDelete("DeleteProduct/{Id}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> DeleteProduct(int Id)
        {
            var res = await _service.DeleteProduct(Id);
            return res.StatusCode switch
            {
                404 => NotFound(res),
                200 => Ok(res),
                _ => BadRequest(res)
            };
        }

        

        [HttpGet("GetAllProducts")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> GetAllPoducts()
        {
            var res = await _service.GetAllProducts();
            return res.StatusCode switch
            {
                200 => Ok(res),
                500 => StatusCode(500, res),
                _ => BadRequest(res)
            };
        }

    }
}
