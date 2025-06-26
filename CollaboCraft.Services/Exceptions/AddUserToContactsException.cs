namespace CollaboCraft.Services.Exceptions
{
    public class AddUserToContactsException() : BadRequestException("Вы не можете добавить в контакты самого себя.");
}
