using CollaboCraft.Common;
using CollaboCraft.DataAccess.Models;
using CollaboCraft.DataAccess.Repositories.Interfaces;
using CollaboCraft.Models.Auth;
using CollaboCraft.Models.Auth.Interfaces;
using CollaboCraft.Models.User;
using CollaboCraft.Services.Exceptions;
using CollaboCraft.Services.Interfaces;
using System.Security.Claims;

namespace CollaboCraft.Services
{
    public class AuthService(IUserRepository userRepository, ITokenService tokenService, IAuthSettings authSettings) : IAuthService
    {
        public async Task<AuthResponse> Register(RegisterModel registerModel)
        {
            if (string.IsNullOrWhiteSpace(registerModel.Email) ||
                string.IsNullOrWhiteSpace(registerModel.Username) ||
                string.IsNullOrWhiteSpace(registerModel.Password) ||
                string.IsNullOrWhiteSpace(registerModel.Name) ||
                string.IsNullOrWhiteSpace(registerModel.Surname))
            {
                throw new InvalidInputException("Все поля обязательны.");
            }

            if (await userRepository.IsUserExistsByUsername(registerModel.Username))
                throw new UsernameAlreadyTakenException(registerModel.Username);

            if (await userRepository.IsUserExistsByEmail(registerModel.Email))
                throw new EmailAlreadyTakenException(registerModel.Email);

            var refreshToken = TokenService.GenerateRefreshToken();
            var userId = await userRepository.CreateUser(new DbUser
            {
                Role = (int)Role.User,
                Username = registerModel.Username,
                Email = registerModel.Email,
                Name = registerModel.Name,
                Surname = registerModel.Surname,
                PasswordHash = Hash.GetHash(registerModel.Password),
                RefreshToken = refreshToken,
                RefreshTokenExpiredAfter = DateTime.UtcNow.AddDays(authSettings.RefreshTokenExpiresInDays)
            });

            var claims = Jwt.GetClaims(userId, (int)Role.User, registerModel.Email, registerModel.Username);
            var accessToken = tokenService.CreateAccessToken(claims);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponse> Login(LoginModel loginModel)
        {
            if (string.IsNullOrWhiteSpace(loginModel.Login) || string.IsNullOrWhiteSpace(loginModel.Password))
                throw new InvalidInputException("Логин и пароль обязательны.");

            var user = await userRepository.GetUser(loginModel.Login, Hash.GetHash(loginModel.Password));
            if (user == null)
                throw new BadCredentialsException();

            var refreshToken = TokenService.GenerateRefreshToken();
            await userRepository.UpdateRefreshToken(
                user.Id,
                refreshToken,
                DateTime.UtcNow.AddDays(authSettings.RefreshTokenExpiresInDays)
            );

            var claims = Jwt.GetClaims(user.Id, user.Role, user.Email, user.Username);
            var accessToken = tokenService.CreateAccessToken(claims);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task Logout(int userId)
        {
            if (!await userRepository.IsUserExistsById(userId))
                throw new UserNotFoundException(userId);

            await userRepository.UpdateRefreshToken(userId, null, null); // инвалидируем refresh
        }
    }
}
