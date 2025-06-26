using CollaboCraft.DataAccess.Repositories.Interfaces;
using CollaboCraft.Models.Permission;
using CollaboCraft.Services.Interfaces;
using CollaboCraft.Services.Mapper;

namespace CollaboCraft.Services
{
    public class DocumentParticipantService(IDocumentParticipantRepository documentParticipantRepository) : IDocumentParticipantService
    {
        public async Task<List<DocumentParticipant>> GetDocumentParticipantsByUserId(int userId)
        {
            var dbDocumentParticipants = await documentParticipantRepository.GetDocumentParticipantsByUserId(userId);

            return dbDocumentParticipants.MapToDomain();
        }
    }
}
