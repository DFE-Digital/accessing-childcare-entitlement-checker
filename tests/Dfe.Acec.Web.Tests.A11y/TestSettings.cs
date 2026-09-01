using System.Diagnostics.CodeAnalysis;

namespace Dfe.Acec.Web.Tests.A11y;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
internal sealed class TestSettings
{
    public string TestUrl { get; set; } = "http://localhost:5252/";
    public string BasicAuthPassword { get; set; } = string.Empty;
    public string Browser { get; set; } = "chromium";
    public bool Headless { get; set; } = true;
    public int SlowMo { get; set; }
    public string UserAgent { get; set; } = "playwright-a11y";
    public string[] Impacts { get; set; } = ["critical", "serious"];
    public bool HmrcIntegrationEnabled { get; set; } = true;
}
