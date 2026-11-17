using JetBrains.Annotations;

namespace Dfe.Acec.Web.Models.Analytics;

/// <summary>
/// Concrete Model for custom GA4 event: Select Content (aka element interaction)
/// Reference: https://docs.data-community.publishing.service.gov.uk/analysis/govuk-ga4/understand-govuk-ga4/?#selectcontent-events
/// </summary>
[UsedImplicitly]
public sealed record ImpressionEvent : GovEventPayload
{
    public override string EventName => "view_item_list";

    public ImpressionEvent()
    {
        MetricValue = 1;
        MetricName = "impression";
    }
}
