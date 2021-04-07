namespace Core.ObisApiModels
{
    public class Authenticate
    {
        
        public class Request
        {
            public string Number { get; set; }
        
            public string Password { get; set; }
        }
        
        public class Response
        {
            public string AuthKey { get; set; }
        }
    }
}
