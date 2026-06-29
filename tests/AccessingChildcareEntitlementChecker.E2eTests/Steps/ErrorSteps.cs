using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.E2eTests.Steps;

[Binding]
internal class ErrorSteps(IPage page)
{
    private IResponse? _lastResponse;

    [Given("I visit a non-existent page")]
    public async Task GivenIVisitANonExistentPage()
    {
        _lastResponse = await page.GotoAsync("/ThisPageDoesNotExist");
    }

    [Given("I visit the development-only error test page")]
    public async Task GivenIVisitTheDevelopmentOnlyErrorTestPage()
    {
        _lastResponse = await page.GotoAsync("/error");
    }

    [Then("I am not redirected to another page")]
    public void ThenIAmNotRedirectedToAnotherPage()
    {
        Assert.NotNull(_lastResponse);
        Assert.Null(_lastResponse.Request.RedirectedFrom);
    }

    [Then("the HTTP status code is 404 Not Found")]
    public void ThenTheHttpStatusCodeIsNotFound()
    {
        Assert.Equal(404, _lastResponse?.Status);
    }

    [Then("the HTTP status code is 500 Internal Server Error")]
    public void ThenTheHttpStatusCodeIsInternalServerError()
    {
        Assert.Equal(500, _lastResponse?.Status);
    }

    [Then("the {string} error is {string}")]
    public async Task ThenTheStringErrorIsString(string fieldName, string expectedError)
    {
        var error = page.Locator($"#{fieldName}-error");

        await Expect(error).ToBeVisibleAsync();
        await Expect(error).ToContainTextAsync(expectedError);
    }

    [Then("an error summary box should appear at the top of the page")]
    public async Task ThenAnErrorSummaryBoxShouldAppearAtTheTopOfThePage()
    {
        await Expect(page.Locator(".govuk-error-summary"))
            .ToBeVisibleAsync();
        var summary = page.Locator(".govuk-error-summary");
        await Expect(summary.Locator(".govuk-error-summary__title"))
            .ToHaveTextAsync("There is a problem");
    }

    [Then("the error summary and inline validation should be {string}")]
    public async Task ThenTheErrorSummaryAndInlineValidationShouldBeString(string message)
    {
        var summary = page.Locator(".govuk-error-summary");
        await Expect(summary.GetByRole(AriaRole.Link, new LocatorGetByRoleOptions { Name = message }))
            .ToBeVisibleAsync();
        var inlineError = page.Locator(".govuk-error-message");
        await Expect(inlineError).ToHaveTextAsync($"Error: {message}");
    }
}
