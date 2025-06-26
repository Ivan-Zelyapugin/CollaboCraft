using System.Text.Json.Serialization;

namespace CollaboCraft.Models.User
{
    [Flags]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Role
    {
        None = 0,
        User = 1,
        Viewer = 2,
        Editor = 3,
        Admin = 4,
        All = Viewer | Editor | Admin | User
    }
}
