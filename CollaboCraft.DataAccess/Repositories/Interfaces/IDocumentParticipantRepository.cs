using CollaboCraft.DataAccess.Dapper.Interfaces;
using CollaboCraft.DataAccess.Models;

namespace CollaboCraft.DataAccess.Repositories.Interfaces
{
    public interface IDocumentParticipantRepository
    {
        ITransaction BeginTransaction();
        Task CreateDocumentParticipant(DbDocumentParticipant DocumentParticipant, ITransaction transaction = null);
        Task CreateDocumentParticipants(List<DbDocumentParticipant> DocumentParticipants, ITransaction transaction = null);
        Task<bool> IsDocumentParticipantExists(int userId, int DocumentId);
        Task<List<DbDocumentParticipant>> GetDocumentParticipantsByUserId(int userId);
        Task<int?> GetUserRoleInDocument(int userId, int documentId);
    }
}
