using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Web.Tests.A11y.Pages;

public class LocationPageAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task LocationPageHasNoAccessibilityViolations()
    {
        if (Settings.FlagSkipLocationEnabled)
        {
            Assert.Skip("Location page is bypassed when FlagSkipLocation is enabled.");
        }

        await StartJourney();
        await ExpectPathAndQuery("/where-do-you-live");
        await EvaluatePage();
    }

    [Fact]
    public async Task LocationPageWithValidationErrorHasNoAccessibilityViolations()
    {
        if (Settings.FlagSkipLocationEnabled)
        {
            Assert.Skip("Location page is bypassed when FlagSkipLocation is enabled.");
        }

        await StartJourney();
        await ExpectPathAndQuery("/where-do-you-live");
        await Continue();
        await ExpectPathAndQuery("/where-do-you-live");
        await Expect(Page.Locator(".govuk-error-summary")).ToBeVisibleAsync();
        await EvaluatePage();
    }
}
