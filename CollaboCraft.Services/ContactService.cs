using CollaboCraft.DataAccess.Repositories.Interfaces;
using CollaboCraft.Models.Contact;
using CollaboCraft.Services.Exceptions;
using CollaboCraft.Services.Interfaces;
using CollaboCraft.Services.Mapper;

namespace CollaboCraft.Services
{
    public class ContactService(IContactRepository contactRepository, IUserRepository userRepository, IConnectionTracker connectionTracker) : IContactService
    {
        public async Task AddUserToContacts(AddUserToContactsRequest request)
        {
            if (request.FriendId == request.UserId)
            {
                throw new AddUserToContactsException();
            }

            if (!await userRepository.IsUserExistsById(request.FriendId))
            {
                throw new UserNotFoundException(request.FriendId);
            }

            if (await contactRepository.IsContactExists(request.UserId, request.FriendId))
            {
                throw new ContactAlreadyExistsException(request.UserId, request.FriendId);
            }

            await contactRepository.CreateContact(request.MapToDb());
        }

        public async Task<List<Contact>> GetContactsByUserId(int userId)
        {
            if (!await userRepository.IsUserExistsById(userId))
            {
                throw new UserNotFoundException(userId);
            }

            var dbContacts = await contactRepository.GetContactsByUserId(userId);

            return dbContacts.Select(dbContact => new Contact
            {
                UserId = userId,
                FriendId = dbContact.FriendId,
                IsOnline = connectionTracker.IsUserConnected(dbContact.FriendId)
            }).OrderByDescending(x => x.IsOnline).ToList();
        }
    }
}
