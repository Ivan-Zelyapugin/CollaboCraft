using System.Text.Json.Serialization;

namespace CollaboCraft.Models.User
{
    public class ChangePasswordRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        [JsonIgnore]
        public string Login { get; set; }
    }
}
