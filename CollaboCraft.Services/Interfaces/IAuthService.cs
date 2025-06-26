using CollaboCraft.Models.Auth;

namespace CollaboCraft.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> Register(RegisterModel registerModel);
        Task<AuthResponse> Login(LoginModel loginModel);
    }
}
