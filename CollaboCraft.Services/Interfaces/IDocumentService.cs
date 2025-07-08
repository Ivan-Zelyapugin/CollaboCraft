using CollaboCraft.Models.Document;
using System;

namespace CollaboCraft.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<Document> CreateDocument(CreateDocumentRequest request);
        Task DeleteDocument(int documentId, int requestingUserId);
        Task<List<UserDocumentDto>> GetDocumentsByUserId(int userId);
        Task<DocumentDetails> GetDocumentDetails(int id);
        Task AddUsersToDocument(AddUsersToDocumentRequest request);
    }
}
