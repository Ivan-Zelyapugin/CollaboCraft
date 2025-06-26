using CollaboCraft.DataAccess.Models;
using CollaboCraft.Models.User;

namespace CollaboCraft.Services.Mapper
{
    public static class UserMapper
    {
        public static User MapToDomain(this DbUser source)
        {
            return source == null
                ? default
                : new User
                {
                    Id = source.Id,
                    Role = (Role)source.Role,
                    Username = source.Username,
                    Email = source.Email,
                    Name = source.Name,
                    Surname = source.Surname
                };
        }

        public static List<User> MapToDomain(this List<DbUser> source)
        {
            return source == null ? [] : source.Select(x => x.MapToDomain()).ToList();
        }

        public static DbUser MapToDb(this User source)
        {
            return source == null
                ? default
                : new DbUser
                {
                    Id = source.Id,
                    Role = (int)source.Role,
                    Username = source.Username,
                    Email = source.Email,
                    Name = source.Name,
                    Surname = source.Surname
                };
        }

        public static DbUser MapToDb(this UpdateUserRequest source)
        {
            return source == null
                ? default
                : new DbUser
                {
                    Id = source.Id,
                    Username = source.Username,
                    Email = source.Email,
                    Name = source.Name,
                    Surname = source.Surname
                };
        }
    }
}
