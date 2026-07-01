using AngleSharp.Dom;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;

public static class ElementAsserts
{
    public static IElement AssertContainsText(this IElement element, string expectedText)
    {
        var actualText = element.TextContent.Trim();
        Assert.Contains(expectedText, actualText);
        return element;
    }
}
