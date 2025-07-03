using System.Text.Json.Serialization;

namespace CollaboCraft.Models.Document
{
    public class AddUsersToDocumentRequest
    {
        public int DocumentId { get; set; }
        public List<string> Usernames { get; set; }
        [JsonIgnore]
        public List<int> UserIds { get; set; }
        [JsonIgnore]
        public int RequestingUserId { get; set; }
    }
}
