using CollaboCraft.DataAccess.Dapper.Interfaces;
using CollaboCraft.DataAccess.Models;
using CollaboCraft.Models.Document;

namespace CollaboCraft.DataAccess.Repositories.Interfaces
{
    public interface IDocumentRepository
    {
        ITransaction BeginTransaction();
        Task<int> CreateDocument(DbDocument document, ITransaction transaction = null);
        Task DeleteDocument(int id, ITransaction transaction = null);
        Task<bool> IsDocumentExists(int id);
        Task<List<DbDocument>> GetDocumentsByUserId(int userId);
        Task<DocumentDetails> GetDocumentDetails(int id);
        Task UpdateDocumentName(int documentId, string newName, ITransaction transaction = null);
    }
}
