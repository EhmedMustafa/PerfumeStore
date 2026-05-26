using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
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
    [EnableRateLimiting("auth")] // flood protection: IP başına dəqiqədə 10 cəhd
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

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registorDto) 
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

        // Google Sign-In — frontend ID token göndərir, biz JWT qaytarırıq
        public class GoogleLoginDto
        {
            public string Credential { get; set; }
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Credential))
                return BadRequest(new { Message = "Google credential boşdur" });

            var clientId = _configuration["Google:ClientId"];
            if (string.IsNullOrWhiteSpace(clientId))
                return StatusCode(500, new { Message = "Google Client ID server-də konfiqurasiya edilməyib" });

            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(dto.Credential, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { clientId }
                });
            }
            catch
            {
                return Unauthorized(new { Message = "Google token etibarsızdır" });
            }

            if (string.IsNullOrWhiteSpace(payload.Email))
                return BadRequest(new { Message = "Google hesabında email tapılmadı" });

            // Mövcud user-i email ilə tap, yoxdursa yarat
            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new AppUser
                {
                    UserName = payload.Email,
                    Email = payload.Email,
                    FirstName = payload.GivenName ?? "",
                    LastName = payload.FamilyName ?? "",
                    EmailConfirmed = payload.EmailVerified
                };
                // Şifrəsiz user — random güclü password (heç vaxt istifadə olunmur)
                var randomPwd = Guid.NewGuid().ToString("N") + "Aa1!";
                var createResult = await _userManager.CreateAsync(user, randomPwd);
                if (!createResult.Succeeded)
                    return BadRequest(new { Message = "Hesab yaradıla bilmədi", Errors = createResult.Errors });

                // Default "User" rolu
                if (await _roleManager.RoleExistsAsync("User"))
                    await _userManager.AddToRoleAsync(user, "User");
            }

            var token = await GenerateJwtToken(user);
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
            var user= await _userManager.FindByNameAsync(username);
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
