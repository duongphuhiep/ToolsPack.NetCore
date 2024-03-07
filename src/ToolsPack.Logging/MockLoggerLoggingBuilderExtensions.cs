using Microsoft.Extensions.Logging;
using System;

namespace ToolsPack.Logging.Testing;

public static partial class MockLoggerLoggingBuilderExtensions
{
    /// <summary>
    /// Add MockLogger to the logging pipeline.
    /// </summary>
    /// <param name="builder">The Microsoft.Extensions.Logging.ILoggingBuilder to add logging provider to.</param>
    /// <param name="logger">The MockLogger logger which your unit test verify/spy on, it is usually created by your mock library (Moq, NSubstitute..)</param>
    /// <returns>Reference to the supplied builder.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static ILoggingBuilder AddMockLogger(this ILoggingBuilder builder, MockLogger logger)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }
        if (logger is null)
        {
            throw new ArgumentNullException(nameof(logger));
        }
        builder.AddProvider(new MockLoggerProvider(logger));
        return builder;
    }
}
