namespace MainService.Data.Errors
{
    public class IdentityKeyExistsException : WriteConflictException
    {
        public IdentityKeyExistsException(string message) : base(message) {}
        
    }
}