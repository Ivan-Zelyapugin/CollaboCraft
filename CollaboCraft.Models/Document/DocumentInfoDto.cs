namespace CollaboCraft.Models.Document
{
    public class DocumentInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatorUsername { get; set; }
        public List<DocumentUserDto> Users { get; set; }
    }

    public class DocumentUserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
