using CollaboCraft.Models.Auth;
using System.Security.Claims;

namespace CollaboCraft.Services.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(IEnumerable<Claim> claims);
        Task<AuthResponse> RefreshToken(string refreshToken);
    }
}
