using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;

public static class DocumentAsserts
{
    public static IDocument AssertBackLink(this IDocument document, string expectedLink)
    {
        var back = document.QuerySelector(".govuk-back-link") as IHtmlAnchorElement;
        Assert.NotNull(back);
        Assert.Contains(expectedLink, back.GetAttribute("href") ?? string.Empty);
        return document;
    }

    public static IDocument AssertRadioButtonCount(this IDocument document, int expectedCount)
    {
        var radios = document.QuerySelectorAll($"input[type=radio]");
        Assert.Equal(expectedCount, radios.Length);
        return document;
    }

    public static IDocument AssertHeader(this IDocument document, string expectedHeader)
    {
        var headerText = document.QuerySelector("h1")?.TextContent.Trim();
        Assert.Equal(expectedHeader, headerText);
        return document;
    }
}
