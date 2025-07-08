using CollaboCraft.DataAccess.Dapper.Interfaces;
using CollaboCraft.DataAccess.Dapper.Models;
using CollaboCraft.DataAccess.Models;
using CollaboCraft.DataAccess.Repositories.Interfaces;
using CollaboCraft.DataAccess.Repositories.Scripts;


namespace CollaboCraft.DataAccess.Repositories
{
    public class DocumentParticipantRepository(IDapperContext<IDapperSettings> dapperContext) : IDocumentParticipantRepository
    {
        public ITransaction BeginTransaction()
        {
            return dapperContext.BeginTransaction();
        }

        public async Task CreateDocumentParticipant(DbDocumentParticipant documentParticipant, ITransaction transaction = null)
        {
            await dapperContext.Command(new QueryObject(Sql.CreateDocumentParticipant, documentParticipant), transaction);
        }

        public async Task CreateDocumentParticipants(List<DbDocumentParticipant> documentParticipants, ITransaction transaction = null)
        {
            await dapperContext.Command(new QueryObject(Sql.CreateDocumentParticipant, documentParticipants), transaction);
        }

        public async Task<bool> IsDocumentParticipantExists(int userId, int documentId)
        {
            return await dapperContext.FirstOrDefault<bool>(new QueryObject(Sql.IsDocumentParticipantExists, new { userId, documentId }));
        }

        public async Task<List<DbDocumentParticipant>> GetDocumentParticipantsByUserId(int userId)
        {
            return await dapperContext.ListOrEmpty<DbDocumentParticipant>(new QueryObject(Sql.GetDocumentParticipantsByUserId, new { userId }));
        }

        public async Task<int?> GetUserRoleInDocument(int userId, int documentId)
        {
            return await dapperContext.FirstOrDefault<int?>(
                new QueryObject(Sql.GetUserRoleInDocument, new { userId, documentId })
            );
        }
    }
}
