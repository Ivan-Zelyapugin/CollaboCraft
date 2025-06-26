using CollaboCraft.Models.Auth;
using System.Security.Claims;

namespace CollaboCraft.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(IEnumerable<Claim> claims, int tokenExpiresAfterHours = 0);
        Task<AuthResponse> RefreshToken(string refreshToken);
    }
}
