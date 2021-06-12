using System;

namespace MainService.ExceptionHandling
{
    public class HttpResponseException : Exception
    {
        public int Status { get; set; } = 500;

        public object Value { get; set; }

        public HttpResponseException(string message) : base(message)
        {
            Value = message;
        }
    }}