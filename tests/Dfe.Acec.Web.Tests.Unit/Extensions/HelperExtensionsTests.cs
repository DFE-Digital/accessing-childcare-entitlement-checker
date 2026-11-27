using Dfe.Acec.Web.Extensions;

namespace Dfe.Acec.Web.Tests.Unit.Extensions;

public class HelperExtensionsTests
{
    [Theory]
    [InlineData("<span>Save and continue</span>")]
    [InlineData("""Continue <span class="govuk-visually-hidden">to application details</span>""")]
    [InlineData("<strong>Important:</strong> Submit now")]
    [InlineData("<div>Block content</div>")]
    [InlineData("""<a href="/next">Next page</a>""")]
    [InlineData("""
                Line 1
                  <span class="govuk-caption-m">Subtext</span>
                """)]
    public void ContainsHtmlIsTrueForBasicHtmlUseCases(string input) => Assert.True(input.ContainsHtml());

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Save and continue")]
    [InlineData("Save &amp; continue")]
    [InlineData("Under 16 &lt; 18")]
    [InlineData("Value < 10")]
    [InlineData("Value > 5")]
    [InlineData("Step 1 -> Step 2")]
    [InlineData("http//path/to/somewhere")]
    [InlineData("""F:\path\to\resource""")]
    public void ContainsHtmlIsFalseForBasicUseCases(string input) => Assert.False(input.ContainsHtml());

}
