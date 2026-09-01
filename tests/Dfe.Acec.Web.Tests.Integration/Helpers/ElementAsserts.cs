using AngleSharp.Dom;
using JetBrains.Annotations;

namespace Dfe.Acec.Web.Tests.Integration.Helpers;

public static class ElementAsserts
{
    [PublicAPI]
    public static IElement AssertContainsText(this IElement element, string expectedText)
    {
        var actualText = element.TextContent.Trim();
        Assert.Contains(expectedText, actualText);
        return element;
    }
}
