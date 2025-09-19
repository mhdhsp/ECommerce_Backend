using AutoMapper;
using ECommerceBackend.CommonApi;
using ECommerceBackend.DTO___Mapping;
using ECommerceBackend.Models;
using ECommerceBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceBackend.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _service;
        public UserController(IMapper mapper,IUserService Service)
        {
            _mapper = mapper;
            _service = Service;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] UserReqDto newUser)
        {
            var user = _mapper.Map<UserModel>(newUser);
            var res=await _service.RegisterUser(user);
            if (res == null)
                return BadRequest(new CommonResponse<string?>(400,"User didnt regiterd.Use may exist alrady",null));
            return Ok(new CommonResponse<Object>(200,"User registered ",newUser));
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginReqDto cred )
        {
            var res = await _service.Login(cred);
            if (res.exist == false)
                return BadRequest(new { message="User doesnt exist" });
            if (res.pass == false)
                return Unauthorized(new { message = "Password doesnt match" });
            return Ok(new {Token= res.tkn,UserName=res.User,Role=res.Role});
        }

        [HttpGet("GetAllUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAllUsers()
        {
            var res = await _service.GetAllUsers();
            if (res == null)
                return NotFound(new CommonResponse<string?>(404, "No users Found", null));
            return Ok(new CommonResponse<Object?>(200, "Succefully found users", res));
        }

        [HttpGet("GetSingleUser/{userId}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> GetSingleUser([FromRoute]int userId)
        {
            var res =await  _service.GetSingleUser(userId);
            return res.StatusCode switch
            {
                404 => NotFound(res.Message),
                200 => Ok(res),
                _ => BadRequest(res)
            };
        }

        [HttpPut("BlockUser")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult> ToggleBlockUser(int userId)
        {
            var adminIdObj = HttpContext.Items["UserId"];
            if (adminIdObj == null)
                return Unauthorized(new CommonResponse<string?>(401, "UnAuthorized User", null));
            int adminId = Convert.ToInt32(adminIdObj);

            var res = await _service.ToggleBlockUser(userId, adminId);
            return res.StatusCode switch
            {
                200 => Ok(res),
                403 => Forbid(res.Message),
                404 => NotFound(res.Message),
                _ => BadRequest(res)
            };


        }
    }
}
