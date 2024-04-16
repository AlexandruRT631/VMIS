namespace user_backend.Exceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string message) : base(message)
    {
        
    }
    
    public UserAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}