using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskUltimate.Interfaces;
using TaskUltimate.Models;
using TaskUltimate.ViewModel;

namespace TaskUltimate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;
        public LoginController(UserManager<ApplicationUser> userManager, IUserService userService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userService = userService;
            _roleManager = roleManager;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([Required][FromBody] LoginDto data)
        {
            var user = await _userService.GetUser(data.username);

                if (user != null && await _userManager.CheckPasswordAsync(user, data.password))
                {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                JwtSecurityToken token = new JwtSecurityToken();
                new Task(() =>
                {
                    token = _userService.CreateToken(authClaims);
                }).Start();

                string refreshToken = _userService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    TokenExpiration = token.ValidTo,
                    RefreshToken = refreshToken,
                    RefreshTokenExpire = DateTime.Now.AddDays(7)
                });
            }
            return Unauthorized("Invalid username or password");
        }
      
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromForm] string username, [FromForm] string password)
        {
            var userExists = await _userManager.FindByNameAsync(username);
            if (userExists != null)
                return BadRequest("User already exists!");

            ApplicationUser user = new()
            {
                Email = username+"@gmail.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = username
            };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return BadRequest("User creation failed! Please check user details and try again.");

            return Ok("User created successfully!");
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromForm] string accessToken, [FromForm] string refreshToken)
        {
            // Check for ClaimsPrincipal
            var principal = _userService.GetPrincipalFromExpiredToken(accessToken);
            if (principal == null) return BadRequest("Invalid access token");

            // Check for user
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null) return BadRequest("User not found");

            // Generate new access token
            var newAccessToken = _userService.CreateToken(principal.Claims.ToList());

            return new ObjectResult(new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                AccessTokenExpiratin = newAccessToken.ValidTo,
                refreshToken = user.RefreshToken,
                RefreshTokenExpiratin = user.RefreshTokenExpiryTime
            });
        }
    }
}
