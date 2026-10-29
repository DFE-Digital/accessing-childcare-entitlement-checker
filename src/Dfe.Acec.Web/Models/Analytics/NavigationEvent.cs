namespace Dfe.Acec.Web.Models.Analytics;

/// <summary>
/// Concrete Model for custom GA4 event: Navigation (e.g. forward/back buttons)
/// </summary>
public sealed record NavigationEvent(string Type="link") : GovEventPayload
{
    public override string EventName => "navigation";
}
