using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.Steps;

[Binding]
public class ErrorSteps
{
    private Context _context;
    private IResponse? _lastResponse;

    public ErrorSteps(Context context)
    {
        _context = context;
    }

    [Given("I visit a non-existent page")]
    public async Task GivenIVisitANon_ExistentPage()
    {
        var url = new Uri(Context.Uri, "/ThisPageDoesNotExist");
        _lastResponse = await _context.Page.GotoAsync(url.AbsoluteUri);
    }

    [Given("I visit the development-only error test page")]
    public async Task GivenIVisitTheDevelopment_OnlyErrorTestPage()
    {
        var url = new Uri(Context.Uri, "/error");
        _lastResponse = await _context.Page.GotoAsync(url.AbsoluteUri);
    }

    [Then("I am not redirected to another page")]
    public void ThenIAmNotRedirectedToAnotherPage()
    {
        // Note: this step may fail when running the tests against HTTP; because
        // you may be redirected to HTTPs. We could consider improving this check,
        // maybe by just validating the url.
        Assert.NotNull(_lastResponse);
        Assert.Null(_lastResponse.Request.RedirectedFrom);
    }

    [Then("the HTTP status code is 404 Not Found")]
    public void ThenTheHTTPStatusCodeIs404NotFound()
    {
        Assert.Equal(404, _lastResponse?.Status);
    }

    [Then("the HTTP status code is 500 Internal Server Error")]
    public void ThenTheHTTPStatusCodeIs500InternalServerError()
    {
        Assert.Equal(500, _lastResponse?.Status);
    }

    [Then("the {string} error is {string}")]
    public async Task ThenTheFieldErrorIs(string fieldName, string expectedError)
    {
        var error = _context.Page.Locator($"#{fieldName}-error");

        await Assertions.Expect(error).ToBeVisibleAsync();
        await Assertions.Expect(error).ToContainTextAsync(expectedError);
    }

    [Then("an error summary box should appear at the top of the page")]
    public async Task ThenAnErrorSummaryBoxShouldAppear()
    {
        await Expect(_context.Page.Locator(".govuk-error-summary"))
            .ToBeVisibleAsync();
        var summary = _context.Page.Locator(".govuk-error-summary");
        await Expect(summary.Locator(".govuk-error-summary__title"))
            .ToHaveTextAsync("There is a problem");
    }

    [Then("the error summary and inline validation should be {string}")]
    public async Task ThenTheErrorSummaryTitleShouldBeWithAnErrorMessage(string message)
    {
        var summary = _context.Page.Locator(".govuk-error-summary");
        await Expect(summary.GetByRole(AriaRole.Link, new() { Name = message }))
            .ToBeVisibleAsync();
        var inlineError = _context.Page.Locator(".govuk-error-message");
        await Expect(inlineError).ToHaveTextAsync($"Error: {message}");
    }
}
