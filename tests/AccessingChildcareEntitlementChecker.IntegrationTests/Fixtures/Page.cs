using AngleSharp.Dom;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;

public record Page(string Url, IDocument Document, string? Token, string? Cookie)
{
    public string H1 => Document.QuerySelector("h1")?.TextContent.Trim() ?? throw new InvalidOperationException("No h1 found on page");
}
