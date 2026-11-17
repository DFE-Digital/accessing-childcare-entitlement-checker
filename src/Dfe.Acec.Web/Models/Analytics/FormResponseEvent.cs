using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Dfe.Acec.Web.Models.Analytics;

/// <summary>
/// Concrete Model for custom GA4 event: Form Response
/// Based on Gov.Uk GA4 naming conventions, with sensible defaults
/// Reference: https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/?#form-events
/// </summary>
[UsedImplicitly]
public sealed record FormResponseEvent : GovEventPayload
{

    public FormResponseEvent()
    {
        Action = "submit";
    }

    public override string EventName => "form_response";

    [JsonPropertyName("response")]
    public required string Response { get; init; }

}
