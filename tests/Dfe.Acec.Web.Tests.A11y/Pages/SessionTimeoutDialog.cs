using System.Globalization;
using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Web.Tests.A11y.Pages;

public class SessionTimeoutDialogAccessibilityTests(ITestOutputHelper output) : JourneyPageBase(output)
{
    [Fact]
    public async Task SessionTimeoutDialogHasNoAccessibilityViolations()
    {
        await StartJourney();
        await ShowSessionTimeoutDialog();
        await EvaluatePage();
    }

    [Fact]
    public async Task SessionTimeoutDialogMovesFocusIntoTheDialog()
    {
        await StartJourney();
        await ShowSessionTimeoutDialog();

        await Expect(Page.Locator("#session-timeout-continue")).ToBeFocusedAsync();
    }

    private async Task ShowSessionTimeoutDialog()
    {
        await Page.Clock.InstallAsync();
        await Page.ReloadAsync();

        var dialog = Page.Locator("#session-timeout-dialog");
        var timeoutMinutes = int.Parse(await dialog.GetAttributeAsync("data-timeout-minutes") ?? string.Empty, CultureInfo.InvariantCulture);
        var warningMinutes = int.Parse(await dialog.GetAttributeAsync("data-warning-minutes") ?? string.Empty, CultureInfo.InvariantCulture);

        await Page.Clock.FastForwardAsync(((timeoutMinutes - warningMinutes) * 60 + 1) * 1000);
        await Expect(dialog).ToBeVisibleAsync();
    }
}
