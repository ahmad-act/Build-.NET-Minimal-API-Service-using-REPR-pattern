namespace BookInformationService.Util;

public static class ErrorLogException
{
    public static Exception ErrorLog(this Exception exception, ILogger<object> logger, string message = "")
    {
        if (logger == null)
        {
            throw new InvalidOperationException("Logger has not been initialized.");
        }

        logger?.LogError(exception, message);

        return exception;
    }
}

