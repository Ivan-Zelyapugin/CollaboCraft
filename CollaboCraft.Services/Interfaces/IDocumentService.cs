using CollaboCraft.Models.Document;
using System;

namespace CollaboCraft.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<Document> CreateDocument(CreateDocumentRequest request);
        Task<List<Document>> GetDocumentsByUserId(int userId);
        Task<DocumentDetails> GetDocumentDetails(int id);
        Task AddUsersToDocument(AddUsersToDocumentRequest request);
    }
}
