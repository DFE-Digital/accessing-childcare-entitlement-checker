using AccessingChildcareEntitlementChecker.Web.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        {
            return new JourneyState();
        }

        return JsonSerializer.Deserialize<JourneyState>(json) ?? new JourneyState();
    }

    public void Set(JourneyState journeyState)
    {
        var httpContext = _httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("No HttpContext available");

        var json = JsonSerializer.Serialize(journeyState);
        httpContext
            .Session
            .SetString(Key, json);
    }
}
