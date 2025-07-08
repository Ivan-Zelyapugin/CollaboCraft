using CollaboCraft.Common;
using CollaboCraft.DataAccess.Repositories.Interfaces;
using CollaboCraft.Models.User;
using CollaboCraft.Services.Exceptions;
using CollaboCraft.Services.Interfaces;
using CollaboCraft.Services.Mapper;

namespace CollaboCraft.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public async Task<User> GetUser(int id)
        {
            var user = await userRepository.GetUser(id);
            if (user == null)
            {
                throw new UserNotFoundException(id);
            }

            return user.MapToDomain();
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await userRepository.GetUsers();

            return users.MapToDomain();
        }

        public async Task UpdateUser(UpdateUserRequest request)
        {
            var existingUser = await userRepository.GetUser(request.Id);

            if (existingUser == null)
                throw new UserNotFoundException(request.Id);

            if (!string.IsNullOrEmpty(request.Username) && request.Username != existingUser.Username)
            {
                if (await userRepository.IsUserExistsByUsername(request.Username))
                    throw new UsernameAlreadyTakenException(request.Username);
            }

            if (!string.IsNullOrEmpty(request.Email) && request.Email != existingUser.Email)
            {
                if (await userRepository.IsUserExistsByEmail(request.Email))
                    throw new EmailAlreadyTakenException(request.Email);
            }

            await userRepository.UpdateUser(request.MapToDb());
        }

        public async Task DeleteUser(int id)
        {
            if (!await userRepository.IsUserExistsById(id))
            {
                throw new UserNotFoundException(id);
            }

            await userRepository.DeleteUser(id);
        }

        public async Task ChangePassword(ChangePasswordRequest request)
        {
            Console.WriteLine(request.Login);
            var user = await userRepository.GetUser(request.Login, Hash.GetHash(request.OldPassword));

            if (user == null)
            {
                throw new BadCredentialsException();
            }

            await userRepository.ChangePassword(user.Id, Hash.GetHash(request.NewPassword));
        }
    }
}
