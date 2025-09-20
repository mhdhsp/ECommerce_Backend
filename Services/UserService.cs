using AutoMapper;
using ECommerceBackend.CommonApi;
using ECommerceBackend.Data;
using ECommerceBackend.DTO___Mapping;
using ECommerceBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerceBackend.Services
{
    public interface IUserService
    {
        Task<UserModel?> RegisterUser(UserModel newUser);
        Task<CommonResponse<LoginResDto?>> Login(LoginReqDto cred);
        Task<IEnumerable<UsersResDto>> GetAllUsers();
        Task<CommonResponse<UsersResDto?>> ToggleBlockUser(int UserId, int adminId);
        Task<CommonResponse<UsersResDto?>> GetSingleUser(int UserId);
        Task<CommonResponse<UserCountResDto?>> CountOfUsers();
    }
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public UserService(AppDbContext context,IConfiguration Configuration,IMapper mapper)
        {
            _context = context;
            _configuration = Configuration;
            _mapper = mapper;
        }


        public async Task<UserModel?> RegisterUser(UserModel newUser)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(x => x.UserName == newUser.UserName);
            if(existing==null)
            {
                newUser.PassWord = BCrypt.Net.BCrypt.HashPassword(newUser.PassWord);
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return newUser;
            }
            Console.WriteLine("from register");
            return null;
        }


        public async Task<CommonResponse<LoginResDto?>> Login(LoginReqDto cred)
        {
            

            var existing = await _context.Users.FirstOrDefaultAsync(x => x.UserName == cred.UserName);
            if (existing == null)
                return new CommonResponse<LoginResDto?>(404, "User does nt exist", null);

            
            if (BCrypt.Net.BCrypt.Verify(cred.PassWord, existing.PassWord))
            {
                var jwtsetting = _configuration.GetSection("JwtSettings");
                var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtsetting["SecretKey"]));
                var creds = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256);
                var Claim = new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier,existing.UserId.ToString()),
                new Claim(ClaimTypes.Name,existing.UserName),
                new Claim(ClaimTypes.Role,existing.Role)
                };

                var tokenHandler = new JwtSecurityToken(
                        issuer: jwtsetting["Issuer"],
                        audience: jwtsetting["Audience"],
                        claims: Claim,
                        signingCredentials: creds
                    );

                var token = new JwtSecurityTokenHandler().WriteToken(tokenHandler);
                var result = new LoginResDto
                {
                    UserName = existing.UserName,
                    Role = existing.Role,
                    Token = token
                };
                return new CommonResponse<LoginResDto?>(200, "Succesfully logined",result);

            }

            

            return new CommonResponse<LoginResDto?>(400,"Incorrect Password",null);
        }

        public async Task<IEnumerable<UsersResDto>> GetAllUsers()
        {
            var users = await _context.Users.Where(x=>x.Role=="User").ToListAsync();
            var user = users.Select(x => new UsersResDto
            {
                UserId = x.UserId,
                UserName = x.UserName,
                Email = x.Email,
                Blocked = x.Blocked,
                Blocked_By = x.Blocked_By,
                Blocked_On = x.Blocked_On
            });
            return user;
        }

        public async Task<CommonResponse<UsersResDto?>> GetSingleUser(int UserId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == UserId);
            if (user == null)
                return new CommonResponse<UsersResDto?> (404, "User not found", null );
            return new CommonResponse<UsersResDto?>(200, "User found", _mapper.Map<UsersResDto>(user));
        }

        public async Task<CommonResponse<UsersResDto?>> ToggleBlockUser(int UserId,int adminId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == UserId);
            if (user == null)
                return new CommonResponse<UsersResDto?>(404, "user Not found", null);
            var admin = await _context.Users.FirstOrDefaultAsync(x => x.UserId == adminId && x.Role == "Admin");
            if(admin==null)
                return new CommonResponse<UsersResDto?>(403, "Only admin can do it ", null);

            if(user.Blocked)
            {
                user.Blocked = false;
                user.Blocked_By = null;
                user.Blocked_On = null;
                await _context.SaveChangesAsync();
                return new CommonResponse<UsersResDto?>(200, "User Unblocked", _mapper.Map<UsersResDto>(user));
            }


            user.Blocked = true;
            user.Blocked_By = admin.UserName;
            user.Blocked_On = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var result = _mapper.Map<UsersResDto>(user);
            return new CommonResponse<UsersResDto?>(200, "User blocked successfully", result);
        }

        public async Task<CommonResponse<UserCountResDto?>> CountOfUsers()
        {
            int validUsers = await _context.Users.CountAsync(x => x.Blocked == false);
            int Invalidusers = await _context.Users.CountAsync(x => x.Blocked == true);
            var result = new UserCountResDto
            {
                InValidUsers = Invalidusers,
                ValidUsers = validUsers
            };
            return new CommonResponse<UserCountResDto?>(200, "Count of users", result);
        }
    }
}
