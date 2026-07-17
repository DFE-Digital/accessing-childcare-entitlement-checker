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

    public static IDocument AssertHeading(this IDocument document, string expectedHeading)
    {
        var headingText = document.QuerySelector("h1")?.TextContent.Trim();
        Assert.Equal(expectedHeading, headingText);
        return document;
    }

    public static IDocument AssertValidationError(this IDocument document)
    {
        var errorMessage = document.QuerySelector(".govuk-error-message");
        Assert.NotNull(errorMessage);
        return document;
    }

    public static IElement AssertResultsSection(this IDocument document, string childName)
    {
        var resultsSection = document
            .QuerySelectorAll(".app-results-section")
            .SingleOrDefault(s => s.QuerySelector(".results-name")?.TextContent.Trim() == childName);

        Assert.NotNull(resultsSection);
        return resultsSection;
    }

    public static IDocument AssertNavigationBar(this IDocument document)
    {
        var header = document.HeaderElement();
        var nav = header.QuerySelector("section.govuk-service-navigation");
        Assert.NotNull(nav);

        var serviceName = nav
            .QuerySelector(".govuk-service-navigation__service-name a");
        Assert.NotNull(serviceName);

        var href = serviceName.GetAttribute("href");
        Assert.Equal("/", href);

        return document;
    }

    public static IDocument AssertBetaBanner(this IDocument document)
    {
        var header = document.HeaderElement();
        var banner = header.QuerySelector(".govuk-phase-banner");
        Assert.NotNull(banner);

        var content = banner
            .QuerySelector(".govuk-phase-banner__content");
        Assert.NotNull(content);

        var tag = banner.QuerySelector(".govuk-tag");
        Assert.NotNull(tag);

        var text = tag.TextContent.Trim();
        Assert.Equal("Beta", text);

        var surveyLink = banner.QuerySelector(".govuk-phase-banner__text a");
        Assert.NotNull(surveyLink);

        var href = surveyLink.GetAttribute("href");
        const string ExpectedHref = "https://dferesearch.eu.qualtrics.com/jfe/preview/previewId/a14e158a-6737-4936-b6cc-9b495f8a4dea/SV_8eotBOVwAQbdP8y?Q_CHL=preview&Q_SurveyVersionID=current";
        Assert.Equal(ExpectedHref, href);

        return document;
    }

    public static IDocument AssertCookieBanner(this IDocument document)
    {
        var cookieBanner = document.QuerySelector(".govuk-cookie-banner");
        Assert.NotNull(cookieBanner);
        return document;
    }

    public static IDocument AssertNoCookieBanner(this IDocument document)
    {
        var cookieBanner = document.QuerySelector(".govuk-cookie-banner");
        Assert.Null(cookieBanner);
        return document;
    }

    public static IDocument AssertGroupHint(this IDocument document, string expectedHint)
    {
        var hint = document.QuerySelector(".govuk-fieldset > .govuk-hint");
        Assert.NotNull(hint);
        Assert.Equal(expectedHint, hint.TextContent?.Trim());
        return document;
    }

    private static IElement HeaderElement(this IDocument document)
    {
        var header = document.QuerySelector("body > header");
        Assert.NotNull(header);
        return header;
    }
}
