using System.Text.Json.Serialization;

namespace CollaboCraft.Models.Document
{
    public class CreateDocumentRequest
    {
        public string Name { get; set; }
        public List<string> Usernames { get; set; }
        [JsonIgnore]
        public List<int> UserIds { get; set; } // Id пользователей, которых нужно добавить в документ.
        [JsonIgnore]
        public int CreatorId { get; set; }// Id создателя документа.
    }
}
