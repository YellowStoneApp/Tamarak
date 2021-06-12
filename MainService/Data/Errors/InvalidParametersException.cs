using System;

namespace MainService.Data.Errors
{
    public class InvalidParametersException : WriteConflictException
    {
        public InvalidParametersException(string message) : base(message)
        {
        }

        public InvalidParametersException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}