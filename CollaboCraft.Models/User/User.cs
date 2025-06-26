using System.Data;

namespace CollaboCraft.Models.User
{
    public class User
    {
        public int Id { get; set; }
        public Role Role { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
