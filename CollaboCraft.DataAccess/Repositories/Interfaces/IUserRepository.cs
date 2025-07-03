using CollaboCraft.DataAccess.Dapper.Interfaces;
using CollaboCraft.DataAccess.Models;

namespace CollaboCraft.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        ITransaction BeginTransaction();
        Task<bool> IsUserExistsById(int id);
        Task<bool> IsUserExistsByUsername(string username);
        Task<bool> IsUserExistsByEmail(string email);
        Task<int> CreateUser(DbUser user);
        Task<DbUser> GetUser(int id);
        Task<DbUser> GetUser(string login, string passwordHash);
        Task<DbUser> GetUserByRefreshToken(string refreshToken);
        Task<List<DbUser>> GetUsers();
        Task UpdateRefreshToken(int id, string? refreshToken, DateTime? refreshTokenExpiredAfter);
        Task UpdateUser(DbUser user);
        Task DeleteUser(int id);
        Task ChangePassword(int id, string passwordHash);
    }
}
