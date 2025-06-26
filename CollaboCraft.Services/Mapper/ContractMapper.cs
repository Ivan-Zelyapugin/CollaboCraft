using CollaboCraft.DataAccess.Models;
using CollaboCraft.Models.Contact;

namespace CollaboCraft.Services.Mapper
{
    public static class ContractMapper
    {
        public static DbContact MapToDb(this AddUserToContactsRequest source)
        {
            return source == null
                ? default
                : new DbContact
                {
                    UserId = source.UserId,
                    FriendId = source.FriendId
                };
        }
    }
}
