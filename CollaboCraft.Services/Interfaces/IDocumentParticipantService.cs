using CollaboCraft.Models.Permission;

namespace CollaboCraft.Services.Interfaces
{
    public interface IDocumentParticipantService
    {
        Task<List<DocumentParticipant>> GetDocumentParticipantsByUserId(int userId);
    }
}
