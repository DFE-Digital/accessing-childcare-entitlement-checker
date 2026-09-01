using System.Text.Json;

namespace Dfe.Acec.Web.Services;

public class JourneySession(IHttpContextAccessor accessor) : IJourneySession
{
    private const string Key = "JourneyState";

    public bool HasSession => accessor.HttpContext?
        .Session
        .TryGetValue(Key, out _) ?? false;

    public JourneyState GetState()
    {
        var json = accessor.HttpContext?
            .Session
            .GetString(Key);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new JourneyState();
        }

        return JsonSerializer.Deserialize<JourneyState>(json) ?? new JourneyState();
    }

    public void SetState(JourneyState journeyState)
    {
        var httpContext = accessor.HttpContext
            ?? throw new InvalidOperationException("No HttpContext available");

        journeyState.CorrelationId = Guid.NewGuid();

        var json = JsonSerializer.Serialize(journeyState);
        httpContext
            .Session
            .SetString(Key, json);
    }
}
