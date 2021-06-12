namespace MainService.ReturnObjects
{
    public class InvalidRequest
    {
        public string Message { get; }
        
        public ResponseType ResponseType { get;  }
        
        public InvalidRequest(string message)
        {
            Message = message;
            ResponseType = ResponseType.InvalidParameters;
        }
    }
}