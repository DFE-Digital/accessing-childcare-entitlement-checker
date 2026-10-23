using System.Text.Json.Serialization;

namespace Dfe.Acec.Web.Models.Analytics;

/// <summary>
/// Base model for all custom GA4 events
/// Based on Gov.Uk GA4 naming conventions
/// reference: https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/?#govuk-ga4-naming-conventions
/// </summary>
public abstract record GovEventPayload
{
    internal GovEventPayload() { }

    [JsonPropertyName("event")]
    public string Event => "gov_event";

    [JsonPropertyName("event_name")]
    public abstract string EventName { get; }

    [JsonPropertyName("type")]
    public required string Type { get; init; }

    [JsonPropertyName("action")]
    public required string Action { get; init; }

    [JsonPropertyName("ui_text")]
    public required string UiText { get; init; }

    [JsonPropertyName("tool_name")]
    public required string ToolName { get; init; } = "childcare_eligibility_checker";

    [JsonPropertyName("section")]
    public required string Section { get; init; }  // e.g "children details"

    [JsonPropertyName("metric_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? MetricName { get; init; }

    [JsonPropertyName("metric_value")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? MetricValue { get; init; }
}
