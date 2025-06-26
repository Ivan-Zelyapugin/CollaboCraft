namespace CollaboCraft.Services.Exceptions
{
    public class BlockNotFoundException(int id) : NotFoundException($"Блок с id {id} не найдено.");
}
