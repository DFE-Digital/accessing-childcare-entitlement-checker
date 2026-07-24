namespace AccessingChildcareEntitlementChecker.A11yTests;

internal class TestSettings
{
    public string TestUrl { get; set; } = "http://localhost:5252/";
    public string BasicAuthPassword { get; set; } = string.Empty;
    public string Browser { get; set; } = "chromium";
    public bool Headless { get; set; } = true;
    public int SlowMo { get; set; } = 0;
    public string UserAgent { get; set; } = "playwright-a11y";
    public string[] Impacts { get; set; } = ["critical", "serious"];
}
