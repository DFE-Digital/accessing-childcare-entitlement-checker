using JetBrains.Annotations;

namespace Dfe.Acec.Web.Models.Analytics;

/// <summary>
/// Concrete Model for custom GA4 event: Navigation (e.g. forward/back buttons)
/// </summary>
[UsedImplicitly]
public sealed record NavigationEvent : GovEventPayload
{
    public override string EventName => "navigation";
}

