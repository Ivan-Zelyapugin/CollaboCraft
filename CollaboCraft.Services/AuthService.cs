using System.Security.Claims;
using CollaboCraft.Common;
using CollaboCraft.DataAccess.Models;
using CollaboCraft.DataAccess.Repositories.Interfaces;
using CollaboCraft.Models.Auth;
using CollaboCraft.Models.Auth.Interfaces;
using CollaboCraft.Models.User;
using CollaboCraft.Services.Exceptions;
using CollaboCraft.Services.Interfaces;


namespace CollaboCraft.Services
{
    public class AuthService(IUserRepository userRepository, ITokenService tokenService, IAuthSettings authSettings) : IAuthService
    {
        public async Task<AuthResponse> Register(RegisterModel registerModel)
        {
            if (string.IsNullOrWhiteSpace(registerModel.Email))
                throw new InvalidInputException("Email обязателен.");
            if (string.IsNullOrWhiteSpace(registerModel.Username))
                throw new InvalidInputException("Имя пользователя обязательно.");
            if (string.IsNullOrWhiteSpace(registerModel.Password))
                throw new InvalidInputException("Пароль обязателен.");
            if (string.IsNullOrWhiteSpace(registerModel.Name))
                throw new InvalidInputException("Имя обязательно.");
            if (string.IsNullOrWhiteSpace(registerModel.Surname))
                throw new InvalidInputException("Фамилия обязательна.");

            if (await userRepository.IsUserExistsByUsername(registerModel.Username))
            {
                throw new UsernameAlreadyTakenException(registerModel.Username);
            }

            if (await userRepository.IsUserExistsByEmail(registerModel.Email))
            {
                throw new EmailAlreadyTakenException(registerModel.Email);
            }

            var refreshToken = tokenService.CreateToken(new List<Claim>());
            var id = await userRepository.CreateUser(new DbUser
            {
                Role = (int)Role.User,
                Username = registerModel.Username,
                Email = registerModel.Email,
                Name = registerModel.Name,
                Surname = registerModel.Surname,
                PasswordHash = Hash.GetHash(registerModel.Password),
                RefreshToken = refreshToken,
                RefreshTokenExpiredAfter = DateTime.UtcNow.AddHours(authSettings.TokenExpiresAfterHours)
            });

            var claims = Jwt.GetClaims(id, (int)Role.User, registerModel.Email, registerModel.Username);
            var accessToken = tokenService.CreateToken(claims, 24);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponse> Login(LoginModel loginModel)
        {
            if (string.IsNullOrWhiteSpace(loginModel.Login))
                throw new InvalidInputException("Логин обязателен.");
            if (string.IsNullOrWhiteSpace(loginModel.Password))
                throw new InvalidInputException("Пароль обязателен.");

            var user = await userRepository.GetUser(loginModel.Login, Hash.GetHash(loginModel.Password));

            if (user == null)
            {
                throw new BadCredentialsException();
            }

            var refreshToken = tokenService.CreateToken(new List<Claim>());
            await userRepository.UpdateRefreshToken(user.Id, refreshToken, DateTime.UtcNow.AddHours(authSettings.TokenExpiresAfterHours));

            var claims = Jwt.GetClaims(user.Id, user.Role, user.Email, user.Username);
            var accessToken = tokenService.CreateToken(claims, 24);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task Logout(int userId)
        {
            if (!await userRepository.IsUserExistsById(userId))
            {
                throw new UserNotFoundException(userId);
            }

            // Инвалидируем refresh token
            await userRepository.UpdateRefreshToken(userId, null, null);
        }
    }
}
