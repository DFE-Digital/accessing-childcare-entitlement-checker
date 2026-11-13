using System.Text.Json.Serialization;

namespace Dfe.Acec.Web.Models.Analytics;

/// <summary>
/// Concrete Model for custom GA4 event: Form error
/// Based on Gov.Uk GA4 naming conventions, with sensible defaults
/// Reference: https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/?#form-events
/// </summary>
public sealed record FormErrorEvent : GovEventPayload
{
    public FormErrorEvent()
    {
        UiText = "Continue";
        Action = "error";
        Type = "contact";
    }
    public override string EventName => "form_error";

    [JsonPropertyName("outcome")]
    public required string Outcome { get; init; }

}
