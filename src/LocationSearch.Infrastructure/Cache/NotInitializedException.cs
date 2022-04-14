namespace LocationSearch.Infrastructure.Cache;

public class NotInitializedException : Exception
{
    public NotInitializedException(string message)
        : base(message)
    {
    }
}