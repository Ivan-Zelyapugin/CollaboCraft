namespace CollaboCraft.Services.Exceptions
{
    public class EditBlockException() : BadRequestException("Вы не можете редактировать чужой блок.");
}
