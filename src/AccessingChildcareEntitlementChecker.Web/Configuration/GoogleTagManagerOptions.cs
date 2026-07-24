namespace AccessingChildcareEntitlementChecker.Web.Configuration;

public sealed class GoogleTagManagerOptions
{
    public const string SectionName = "GoogleTagManager";
    public bool Enabled { get; init; }
    public string ContainerId { get; init; } = string.Empty;
}