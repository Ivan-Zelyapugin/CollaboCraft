using System.Text.Json.Serialization;

namespace CollaboCraft.Models.Block
{
    public class EditBlockRequest
    {
        public int Id { get; set; }
        public string EditedText { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public DateTime? EditedOn { get; set; }
    }
}
