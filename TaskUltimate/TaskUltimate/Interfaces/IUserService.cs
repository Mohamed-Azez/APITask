using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskUltimate.Models;

namespace TaskUltimate.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUser(string userName);
        JwtSecurityToken CreateToken(List<Claim> authClaims);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
        string GenerateRefreshToken();
    }
}
