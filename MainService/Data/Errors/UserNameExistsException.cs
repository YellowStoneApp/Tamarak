namespace MainService.Data.Errors
{
    public class UserNameExistsException : WriteConflictException
    {
        public UserNameExistsException(string message) : base(message)
        {
        }
    }
}