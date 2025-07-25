﻿using CollaboCraft.Models.Permission;

namespace CollaboCraft.Services.Interfaces
{
    public interface IDocumentParticipantService
    {
        Task<List<DocumentParticipant>> GetDocumentParticipantsByUserId(int userId);
        Task ChangeUserRoleInDocument(int documentId, int userId, string newRole, int requestingUserId);
        Task RemoveUserFromDocument(int documentId, int userId, int requestingUserId);
    }
}
