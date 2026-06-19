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

    public static IDocument AssertCheckboxCount(this IDocument document, int expectedCount)
    {
        var checkboxes = document.QuerySelectorAll($"input[type=checkbox]");
        Assert.Equal(expectedCount, checkboxes.Length);
        return document;
    }

    public static IDocument AssertRadioButtonCount(this IDocument document, int expectedCount)
    {
        var radios = document.QuerySelectorAll($"input[type=radio]");
        Assert.Equal(expectedCount, radios.Length);
        return document;
    }

    public static IDocument AssertTextInput(this IDocument document)
    {
        var texts = document.QuerySelectorAll($"input[type=text]");
        Assert.Single(texts);
        return document;
    }

    public static IDocument AssertDateInput(this IDocument document)
    {
        var texts = document.QuerySelectorAll($".govuk-date-input");
        Assert.Single(texts);
        return document;
    }

    public static IDocument AssertHeader(this IDocument document, string expectedHeader)
    {
        var headerText = document.QuerySelector("h1")?.TextContent.Trim();
        Assert.Equal(expectedHeader, headerText);
        return document;
    }

    public static IDocument AssertValidationError(this IDocument document)
    {
        var errorMessage = document.QuerySelector(".govuk-error-message");
        Assert.NotNull(errorMessage);
        return document;
    }
}
