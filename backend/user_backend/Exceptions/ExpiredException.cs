namespace user_backend.Exceptions;

public class ExpiredException : Exception
{
    public ExpiredException(string message) : base(message)
    {
        
    }
    
    public ExpiredException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}