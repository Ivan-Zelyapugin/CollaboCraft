namespace CollaboCraft.Models.BlockImage
{
    public class SendBlockImageRequest
    {
        public int BlockId { get; set; }
        public string Url { get; set; }
        public DateTime UploadedOn { get; set; }
        public int UserId { get; set; }

    }

    public class FileUpload
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string ContentBase64 { get; set; }
    }
}
