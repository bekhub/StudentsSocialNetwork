namespace Core.ObisApiModels
{
    public class ChangePassword
    {
        public class Request
        {
            public string CurrentPassword { get; set; } 
            
            public string NewPassword { get; set; }
            
            public string RepeatNewPassword { get; set; }
        }
        
        public class Response
        {
            public string Code { get; set; }
            
            public string Message { get; set; }
        }
    }
}
