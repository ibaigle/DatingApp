using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOS;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        public readonly DataContext _context;
        public readonly ITokenService _token;
        public AccountController(DataContext context, ITokenService tokenService){
            _context=context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register (RegisterDto registerDto)
        {

            if (await UserExists(registerDto.Username)) return BadRequest(">Registered Username is taken!");
            

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                Username= registerDto.Username.ToLower(),
                PasswordHash= hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt= hmac.Key
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto{
                Username = user.Username,
                Token = _token.CreateToken(user)
            };
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user= await _context.Users.SingleOrDefaultAsync(x =>x.Username == loginDto.Username );
            if (user==null){return Unauthorized("Invalid username");}
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for(int i =0;  i< computeHash.Length; i++){
                if (computeHash[i] != user.PasswordHash[i]){
                    return Unauthorized("Invalid password");
                }
            }
            return new UserDto{
                Username = user.Username,
                Token = _token.CreateToken(user)
            };
        }
        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.Username == username.ToLower());
        }
    }
}