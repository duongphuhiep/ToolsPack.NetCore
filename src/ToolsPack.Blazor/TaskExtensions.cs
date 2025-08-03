using Microsoft.Extensions.Logging;

namespace ToolsPack.Blazor;

public static class TaskExtensions
{
    public static async void FireAndForget(this Task task, ILogger? logger = null, bool continueOnCapturedContext = false)
    {
        try
        {
            await task.ConfigureAwait(continueOnCapturedContext);
        }
        catch (Exception ex)
        {
            if (logger is null)
                Console.WriteLine($"FireAndForget unhandled exception {ex}");
            else
                logger.LogError(ex, "FireAndForget unhandled exception");
        }
    }
}