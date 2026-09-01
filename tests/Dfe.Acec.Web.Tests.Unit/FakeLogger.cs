using Microsoft.Extensions.Logging;

namespace Dfe.Acec.Web.Tests.Unit;

public class FakeLogger<T> : ILogger<T>
{
    public List<string> Messages { get; } = [];
    public List<KeyValuePair<string, object>> Properties { get; } = [];

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        var message = formatter(state, exception);
        Messages.Add(message);

        if (state is IEnumerable<KeyValuePair<string, object>> props)
        {
            Properties.AddRange(props);
        }
    }
}
