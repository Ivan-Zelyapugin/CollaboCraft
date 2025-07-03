using CollaboCraft.DataAccess.Models;
using CollaboCraft.DataAccess.Repositories.Interfaces;
using CollaboCraft.Models.Document;
using CollaboCraft.Models.Permission;
using CollaboCraft.Services.Exceptions;
using CollaboCraft.Services.Interfaces;
using CollaboCraft.Services.Mapper;

namespace CollaboCraft.Services
{
    public class DocumentService(IDocumentRepository documentRepository, IUserRepository userRepository, IDocumentParticipantRepository documentParticipantRepository) : IDocumentService
    {
        public async Task<Document> CreateDocument(CreateDocumentRequest request)
        {
            request.UserIds.Add(request.CreatorId);
            foreach (var id in request.UserIds)
            {
                if (!await userRepository.IsUserExistsById(id))
                {
                    throw new UserNotFoundException(id);
                }
            }

            // создаем чат и добавляем в него пользователей
            using var transaction = documentRepository.BeginTransaction();
            try
            {
                var dbDocument = request.MapToDb();
                var documentId = await documentRepository.CreateDocument(request.MapToDb(), transaction);
                var documentParticipants = request.UserIds.Select(id => new DbDocumentParticipant
                {
                    UserId = id,
                    DocumentId = documentId,
                    Role = (int)(id == request.CreatorId ? DocumentRole.Creator : DocumentRole.User)
                }).ToList();
                await documentParticipantRepository.CreateDocumentParticipants(documentParticipants, transaction);

                transaction.Commit();
                dbDocument.Id = documentId;

                return dbDocument.MapToDomain();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task DeleteDocument(int documentId, int requestingUserId)
        {
            if (!await documentRepository.IsDocumentExists(documentId))
            {
                throw new DocumentNotFoundException(documentId);
            }

            var participant = await documentParticipantRepository.IsDocumentParticipantExists(requestingUserId, documentId);
            if (!participant)
            {
                throw new DocumentParticipantNotFoundException(requestingUserId, documentId);
            }

            using var transaction = documentRepository.BeginTransaction();
            try
            {
                await documentRepository.DeleteDocument(documentId, transaction);
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<List<Document>> GetDocumentsByUserId(int userId)
        {
            var dbDocuments = await documentRepository.GetDocumentsByUserId(userId);

            return dbDocuments.MapToDomain();
        }

        public async Task<DocumentDetails> GetDocumentDetails(int id)
        {
            var documentDetails = await documentRepository.GetDocumentDetails(id);

            if (documentDetails == null)
            {
                throw new DocumentNotFoundException(id);
            }

            return documentDetails;
        }

        public async Task AddUsersToDocument(AddUsersToDocumentRequest request)
        {
            if (!await documentRepository.IsDocumentExists(request.DocumentId))
            {
                throw new DocumentNotFoundException(request.DocumentId);
            }

            if (!await documentParticipantRepository.IsDocumentParticipantExists(request.RequestingUserId, request.DocumentId))
            {
                throw new DocumentParticipantNotFoundException(request.RequestingUserId, request.DocumentId);
            }

            foreach (var id in request.UserIds)
            {
                if (!await userRepository.IsUserExistsById(id))
                {
                    throw new UserNotFoundException(id);
                }

                if (await documentParticipantRepository.IsDocumentParticipantExists(id, request.DocumentId))
                {
                    throw new UserAlreadyInDocumentException(id, request.DocumentId);
                }
            }

            var documentParticipants = request.UserIds.Select(id => new DbDocumentParticipant
            {
                UserId = id,
                DocumentId = request.DocumentId,
                Role = (int)DocumentRole.User
            }).ToList();
            await documentParticipantRepository.CreateDocumentParticipants(documentParticipants);
        }
    }
}
