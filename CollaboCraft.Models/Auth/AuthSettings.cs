using CollaboCraft.Models.Auth.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CollaboCraft.Models.Auth
{
    public class AuthSettings(IConfiguration configuration) : IAuthSettings
    {
        public string Issuer => configuration.GetSection("Auth")["Issuer"];
        public string Audience => configuration.GetSection("Auth")["Audience"];
        public string Key => configuration.GetSection("Auth")["Key"];
        public int AccessTokenExpiresInMinutes => int.Parse(configuration.GetSection("Auth")["AccessTokenExpiresInMinutes"]);
        public int RefreshTokenExpiresInDays => int.Parse(configuration.GetSection("Auth")["RefreshTokenExpiresInDays"]);
    }
}
