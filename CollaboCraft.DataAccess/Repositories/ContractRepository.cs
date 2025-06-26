using CollaboCraft.DataAccess.Dapper.Interfaces;
using CollaboCraft.DataAccess.Dapper.Models;
using CollaboCraft.DataAccess.Models;
using CollaboCraft.DataAccess.Repositories.Interfaces;
using CollaboCraft.DataAccess.Repositories.Scripts;

namespace CollaboCraft.DataAccess.Repositories
{
    public class ContactRepository(IDapperContext<IDapperSettings> dapperContext) : IContactRepository
    {
        public ITransaction BeginTransaction()
        {
            return dapperContext.BeginTransaction();
        }

        public async Task CreateContact(DbContact contact)
        {
            await dapperContext.Command(new QueryObject(Sql.CreateContract, contact));
        }

        public async Task<bool> IsContactExists(int userId, int friendId)
        {
            return await dapperContext.FirstOrDefault<bool>(new QueryObject(Sql.IsContractExists, new { userId, friendId }));
        }

        public async Task<List<DbContact>> GetContactsByUserId(int userId)
        {
            return await dapperContext.ListOrEmpty<DbContact>(new QueryObject(Sql.GetContractsByUserId, new { userId }));
        }
    }
}
