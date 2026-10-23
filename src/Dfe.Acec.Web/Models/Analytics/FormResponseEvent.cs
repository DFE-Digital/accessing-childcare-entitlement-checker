using System.Text.Json.Serialization;

namespace Dfe.Acec.Web.Models.Analytics;

/// <summary>
/// Concrete Model for custom GA4 event: Form Response
/// Based on Gov.Uk GA4 naming conventions, with sensible defaults
/// Reference: https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/?#form-events
/// </summary>
public sealed record FormResponseEvent(string Action="submit",
                                        string UiText = "Continue") : GovEventPayload
{
    public override string EventName => "form_response";

    [JsonPropertyName("response")]
    public required string Response { get; init; }

}
