﻿namespace CollaboCraft.Models.Permission
{
    public class DocumentParticipantFull
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int DocumentId { get; set; }
        public DocumentRole Role { get; set; }
    }
}
