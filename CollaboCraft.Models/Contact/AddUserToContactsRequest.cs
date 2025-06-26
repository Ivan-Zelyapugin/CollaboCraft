using System.Text.Json.Serialization;

namespace CollaboCraft.Models.Contact
{
    public class AddUserToContactsRequest
    {
        public int FriendId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
