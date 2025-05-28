using System.Security.Claims;
using API.Entites;

namespace API.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
        string GenerateRefreshToken();
        Task<ClaimsPrincipal> GetPrincipalFromExpiredTokenAsync(string token);
    }
}
