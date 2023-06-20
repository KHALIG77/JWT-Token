using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shop.Api.DTOs.UsersDTOs;
using Shop.Core.Entities;

namespace Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
           _configuration = configuration;
        }
        //[HttpGet("roles")]
        //public async Task<IActionResult> CreateRole()
        //{
        //   await _roleManager.CreateAsync(new IdentityRole("Member"));
        //   await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    return Ok();
        //}
        //[HttpGet("admin")]
        //public async Task<IActionResult> CreateAdmin()
        //{
        //  var user = new AppUser
        //  {
        //      UserName = "Admin",
        //      FullName = "Super Admin",
        //      Email = "admin@mail.ru"

        //  };
        //    await _userManager.CreateAsync(user,"Admin123");
        //    await _userManager.AddToRoleAsync(user, "Admin");

        //    return Ok();

        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO loginDTO)
        {
            AppUser user=await _userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null)
            {
                return Unauthorized();
            }
            if(!await _userManager.CheckPasswordAsync(user, loginDTO.Password))
            {
                return Unauthorized();

            }
            var roles = await _userManager.GetRolesAsync(user);
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim("FullName",user.FullName)

            };
            claims.AddRange(roles.Select(x=> new Claim(ClaimTypes.Role,x)).ToList());
           
             
            var key =new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Security:Secret"]));
            var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(claims: claims,
                signingCredentials: creds,
                expires: DateTime.Now.AddDays(3),
                issuer: _configuration["Security:Issuer"],
                audience: _configuration["Security:Audience"]);

                var tokenStr=new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(tokenStr);
        }

    }
}
