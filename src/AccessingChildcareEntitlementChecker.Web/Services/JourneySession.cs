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

    public JourneyState Get()
    {
        var json = _httpContextAccessor.HttpContext?
            .Session
            .GetString(Key);

        if (string.IsNullOrWhiteSpace(json))
            return new JourneyState();

        return JsonSerializer.Deserialize<JourneyState>(json)
               ?? new JourneyState();
    }

    public void Save(JourneyState state)
    {
        var json = JsonSerializer.Serialize(state);

        _httpContextAccessor.HttpContext?
            .Session
            .SetString(Key, json);
    }
}