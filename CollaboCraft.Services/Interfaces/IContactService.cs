using CollaboCraft.Models.Contact;

namespace CollaboCraft.Services.Interfaces
{
    public interface IContactService
    {
        Task AddUserToContacts(AddUserToContactsRequest request);
        Task<List<Contact>> GetContactsByUserId(int userId);
    }
}
