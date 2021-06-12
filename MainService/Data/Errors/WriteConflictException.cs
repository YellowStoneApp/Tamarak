using System;

namespace MainService.Data.Errors
{
    public class WriteConflictException : System.Exception
    {
        public WriteConflictException(string message) : base(message)
        { }
        
        public WriteConflictException(string message, Exception innerException) : base(message, innerException) { }
        
    }
}