using System.Text.Json.Serialization;

namespace CollaboCraft.Models.User
{
    public class UpdateUserRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
