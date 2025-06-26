namespace CollaboCraft.Services.Exceptions
{
    public class ContactAlreadyExistsException(int userId, int friendId) : BadRequestException($"Пользователь с id {friendId} уже есть в контактах пользователя с id {userId}");
}
