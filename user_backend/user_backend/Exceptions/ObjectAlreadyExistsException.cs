namespace user_backend.Exceptions;

public class ObjectAlreadyExistsException : Exception
{
    public ObjectAlreadyExistsException(string message) : base(message)
    {
        
    }
    
    public ObjectAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}