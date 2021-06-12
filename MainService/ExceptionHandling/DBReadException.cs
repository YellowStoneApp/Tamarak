using System;

namespace MainService.ExceptionHandling
{
    public class DBReadException : Exception
    {
        public DBReadException(string message) : base(message) { }
    }
}