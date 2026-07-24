using System.Text.Json;

namespace AccessingChildcareEntitlementChecker.Web.Services;

public class JourneySession : IJourneySession
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string Key = "JourneyState";

    public JourneySession(IHttpContextAccessor accessor)
    {
        _httpContextAccessor = accessor;
    }

    public bool HasSession => _httpContextAccessor.HttpContext?
        .Session
        .TryGetValue(Key, out _) ?? false;

    public JourneyState Get()
    {
        var json = _httpContextAccessor.HttpContext?
            .Session
            .GetString(Key);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new JourneyState();
        }

        return JsonSerializer.Deserialize<JourneyState>(json) ?? new JourneyState();
    }

    public void Set(JourneyState journeyState)
    {
        var httpContext = _httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("No HttpContext available");

        journeyState.CorrelationId = Guid.NewGuid();

        var json = JsonSerializer.Serialize(journeyState);
        httpContext
            .Session
            .SetString(Key, json);
    }

    public void Clear()
    {
        _httpContextAccessor.HttpContext?.Session.Remove(Key);
    }
}
