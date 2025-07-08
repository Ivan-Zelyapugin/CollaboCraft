namespace CollaboCraft.Services.Exceptions
{
    public class PermissionDeniedException : Exception
    {
        public PermissionDeniedException(string message) : base(message) { }
    }
}
