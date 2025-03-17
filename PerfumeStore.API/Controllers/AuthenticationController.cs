using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PerfumeStore.Application.Dtos;
using PerfumeStore.Application.Dtos.UserDtos;
using PerfumeStore.Domain.Entities.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PerfumeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("registor")]
        public async Task<IActionResult> Registor([FromBody] RegisterDto registorDto) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            var user = new AppUser
            {
                UserName = registorDto.UserName,
                Email = registorDto.Email,
                FirstName = registorDto.FirstName,
                LastName = registorDto.LastName,
            };

            var result= await _userManager.CreateAsync(user,registorDto.Password);
            if (!result.Succeeded) 
            {
                return BadRequest(result.Errors);
            }
            await _userManager.AddToRoleAsync(user, "User");
            return Ok(new { Message= "İstifadəçi uğurla yaradıldı!" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto) 
        {
            var user =await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Unauthorized(new { Message = "İstifadəçi adı və ya şifrə yanlışdır!" });
            var token= await GenerateJwtToken(user);

            return Ok(new { Token = token });
        }




        private async Task<string> GenerateJwtToken(AppUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
       
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetUserProfile() 
        {
            var username = User.Identity.Name;//Token-dən istifadəçi adı (User.Identity.Name) tapılır və bazadan axtarılır.
            var user= await _userManager.FindByIdAsync(username);
            if (user == null)  return NotFound(new { Message = "İstifadəçi tapılmadı!" });
            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName
            });
        }
    }

}
