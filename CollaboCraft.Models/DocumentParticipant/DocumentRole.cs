using System.Text.Json.Serialization;

namespace CollaboCraft.Models.Permission
{
    [Flags]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DocumentRole
    {
        None = 0,
        User = 1,
        Viewer = 2,
        Editor = 3,
        Admin = 4,
        Creator = 5,
        All = Viewer | Editor | Admin | Creator | User
    }
}
