using Microsoft.Extensions.Logging;

namespace Dfe.Acec.Web.Tests.Integration.Helpers;

internal sealed class XunitLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => new XunitLogger(categoryName);
    public void Dispose() { }

    private sealed class XunitLogger(string categoryName) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Error;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            var output = TestContext.Current.TestOutputHelper;
            if (output is null) { return; }

            output.WriteLine($"[{logLevel}] {categoryName}: {formatter(state, exception)}");
            if (exception is not null)
            {
                output.WriteLine(exception.ToString());
            }
        }
    }
}
