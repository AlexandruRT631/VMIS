namespace listing_backend.Exceptions;

public class InvalidArgumentException : Exception
{
    public InvalidArgumentException(string message) : base(message)
    {
        
    }
    
    public InvalidArgumentException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}