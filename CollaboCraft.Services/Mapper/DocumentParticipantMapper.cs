using CollaboCraft.DataAccess.Models;
using CollaboCraft.Models.Permission;

namespace CollaboCraft.Services.Mapper
{
    public static class DocumentParticipantMapper
    {
        public static DocumentParticipant MapToDomain(this DbDocumentParticipant source)
        {
            return source == null
                ? default
                : new DocumentParticipant
                {
                    UserId = source.UserId,
                    DocumentId = source.DocumentId,
                    Role = (DocumentRole)source.Role
                };
        }

        public static List<DocumentParticipant> MapToDomain(this List<DbDocumentParticipant> source)
        {
            return source == null ? [] : source.Select(x => x.MapToDomain()).ToList();
        }
    }
}
