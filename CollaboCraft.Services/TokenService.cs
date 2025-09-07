using CollaboCraft.Common;
using CollaboCraft.DataAccess.Repositories.Interfaces;
using CollaboCraft.Models.Auth;
using CollaboCraft.Models.Auth.Interfaces;
using CollaboCraft.Services.Exceptions;
using CollaboCraft.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CollaboCraft.Services
{
    public class TokenService(IAuthSettings authSettings, IUserRepository userRepository) : ITokenService
    {
        public string CreateAccessToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.Key));

            var token = new JwtSecurityToken(
                issuer: authSettings.Issuer,
                audience: authSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(authSettings.AccessTokenExpiresInMinutes),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<AuthResponse> RefreshToken(string refreshToken)
        {
            var user = await userRepository.GetUserByRefreshToken(refreshToken);
            if (user == null)
                throw new RefreshTokenNotFoundException();

            if (user.RefreshTokenExpiredAfter < DateTime.UtcNow)
                throw new SecurityTokenExpiredException("Refresh token expired");

            // Создаем новый access token
            var claims = Jwt.GetClaims(user.Id, user.Role, user.Email, user.Username);
            var newAccessToken = CreateAccessToken(claims);

            string newRefreshToken = refreshToken; // по умолчанию оставляем старый
            var refreshThreshold = TimeSpan.FromDays(1); // обновлять, если до истечения <1 дня

            if (user.RefreshTokenExpiredAfter - DateTime.UtcNow < refreshThreshold)
            {
                newRefreshToken = GenerateRefreshToken();
                await userRepository.UpdateRefreshToken(
                    user.Id,
                    newRefreshToken,
                    DateTime.UtcNow.AddDays(authSettings.RefreshTokenExpiresInDays)
                );
            }

            return new AuthResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

    }
}
