using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace Dfe.Acec.Web.Tests.E2e.Steps;

[Binding]
internal sealed class SessionTimeoutSteps(IPage page)
{
    private const string DialogSelector = "#session-timeout-dialog";

    [Given("the browser clock is controlled")]
    public async Task GivenTheBrowserClockIsControlled()
    {
        await page.Clock.InstallAsync();
        await page.ReloadAsync();
    }

    [When("I wait until the session timeout warning is almost due")]
    public async Task WhenIWaitUntilTheSessionTimeoutWarningIsAlmostDue()
    {
        var (timeout, warning) = await GetTimingsAsync();
        await FastForwardAsync(timeout - warning - 1);
    }

    [When("I wait until the session timeout warning is due")]
    public async Task WhenIWaitUntilTheSessionTimeoutWarningIsDue()
    {
        var (timeout, warning) = await GetTimingsAsync();
        await FastForwardAsync(timeout - warning + 1);
    }

    [When("I wait until the session has timed out")]
    public async Task WhenIWaitUntilTheSessionHasTimedOut()
    {
        var (timeout, _) = await GetTimingsAsync();
        await FastForwardAsync(timeout + 1);
    }

    [When("I click Continue in the session timeout dialog")]
    public async Task WhenIClickContinueInTheSessionTimeoutDialog()
    {
        await page.Locator(DialogSelector)
            .GetByRole(AriaRole.Button, new() { Name = "Continue" })
            .ClickAsync();
    }

    [When("I press the escape key")]
    public async Task WhenIPressTheEscapeKey()
    {
        await page.Keyboard.PressAsync("Escape");
    }

    [Then("the session timeout dialog is shown")]
    public async Task ThenTheSessionTimeoutDialogIsShown()
    {
        await Expect(page.Locator(DialogSelector)).ToBeVisibleAsync();
    }

    [Then("the session timeout dialog is not shown")]
    public async Task ThenTheSessionTimeoutDialogIsNotShown()
    {
        await Expect(page.Locator(DialogSelector)).ToBeHiddenAsync();
    }

    [Then("the session timeout dialog says the answers will be deleted soon")]
    public async Task ThenTheSessionTimeoutDialogSaysTheAnswersWillBeDeletedSoon()
    {
        var dialog = page.Locator(DialogSelector);
        await Expect(dialog.GetByRole(AriaRole.Heading, new() { Name = "Your answers will be deleted soon" })).ToBeVisibleAsync();
        await Expect(dialog).ToContainTextAsync("We will delete your answers if you do not continue in the next");
    }

    [Then("I am on the session expired page")]
    public async Task ThenIAmOnTheSessionExpiredPage()
    {
        await Expect(page).ToHaveURLAsync(new System.Text.RegularExpressions.Regex("/session-expired$"));
        await Expect(page.GetByRole(AriaRole.Heading, new() { Level = 1 })).ToHaveTextAsync("Your answers have been deleted");
    }

    private async Task<(int TimeoutSeconds, int WarningSeconds)> GetTimingsAsync()
    {
        var dialog = page.Locator(DialogSelector);
        var timeoutMinutes = int.Parse(await dialog.GetAttributeAsync("data-timeout-minutes") ?? string.Empty, System.Globalization.CultureInfo.InvariantCulture);
        var warningMinutes = int.Parse(await dialog.GetAttributeAsync("data-warning-minutes") ?? string.Empty, System.Globalization.CultureInfo.InvariantCulture);
        return (timeoutMinutes * 60, warningMinutes * 60);
    }

    private async Task FastForwardAsync(int seconds)
    {
        await page.Clock.FastForwardAsync(seconds * 1000);
    }
}
