using CollaboCraft.Models.Permission;

namespace CollaboCraft.Models.Document
{
    public class UserDocumentDto
    {
        public Document Document { get; set; }
        public DocumentRole Role { get; set; }
    }
}
