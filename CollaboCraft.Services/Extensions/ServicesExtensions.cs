using CollaboCraft.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CollaboCraft.Services.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<ITokenService, TokenService>()
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<IUserService, UserService>()
                .AddSingleton<IConnectionTracker, ConnectionTracker>()
                .AddScoped<IDocumentService, DocumentService>()
                .AddScoped<IBlockService, BlockService>()
                .AddScoped<IBlockImageService, BlockImageService>()
                .AddScoped<IDocumentParticipantService, DocumentParticipantService>()
                .AddScoped<IContactService, ContactService>();
        }
    }
}
