using CollaboCraft.DataAccess.Dapper.Interfaces;
using CollaboCraft.DataAccess.Models;

namespace CollaboCraft.DataAccess.Repositories.Interfaces
{
    public interface IContactRepository
    {
        ITransaction BeginTransaction();
        Task CreateContact(DbContact contact);
        Task<bool> IsContactExists(int userId, int friendId);
        Task<List<DbContact>> GetContactsByUserId(int userId);
    }
}
