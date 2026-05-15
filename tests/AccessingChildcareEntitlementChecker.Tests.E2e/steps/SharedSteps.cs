using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.steps
{
    [Binding]
    public class SharedSteps
    {
        private Context _context;

        public SharedSteps(Context context)
        {
            _context = context;
        }

        [Given("the page header is {string}")]
        [Then("the page header is {string}")]
        public async Task ThenThePageHeaderIs(string expectedHeader)
        {
            await _context.Page
                .GetByRole(AriaRole.Heading, new() { Name = expectedHeader })
                .WaitForAsync();
        }

        [When("I click on Continue")]
        public async Task WhenIClickOnContinue()
        {
            await _context.Page.GetByRole(AriaRole.Button, new() { Name = "Continue" }).ClickAsync();
        }

        [When("I click the back link")]
        public async Task WhenIClickTheBackLink()
        {
            await _context.Page
                .Locator(".govuk-back-link")
                .ClickAsync();
        }

        [Then("an error summary box should appear at the top of the page")]
        public async Task ThenAnErrorSummaryBoxShouldAppear()
        {
            await Expect(_context.Page.Locator(".govuk-error-summary"))
                .ToBeVisibleAsync();
        }

        [Then("the error summary title should be {string} with an error message {string}")]
        public async Task ThenTheErrorSummaryTitleShouldBeWithAnErrorMessage(string title, string message)
        {
            var summary = _context.Page.Locator(".govuk-error-summary");

            await Expect(summary.Locator(".govuk-error-summary__title"))
                .ToHaveTextAsync(title);

            await Expect(summary.GetByRole(AriaRole.Link, new() { Name = message }))
                .ToBeVisibleAsync();
        }

        [Then("inline validation should display with the error message {string}")]
        public async Task ThenInlineValidationShouldDisplay(string message)
        {
            await Expect(_context.Page.Locator(".govuk-error-message"))
                .ToContainTextAsync(message);
        }

        [Then("I will be directed to the next page in the user journey {string}")]
        public async Task ThenIWillBeDirectedToTheNextPageInTheUserJourney(string expectedHeader)
        {
            await Expect(_context.Page.Locator("body"))
                .ToContainTextAsync(expectedHeader);
        }

        [Then("I should be returned to the previous page in the user journey {string}")]
        public async Task ThenIShouldBeReturnedToThePreviousPageInTheUserJourney(string expectedHeader)
        {
            await Expect(
                _context.Page.GetByRole(AriaRole.Heading, new() { Level = 1 })
            ).ToHaveTextAsync(expectedHeader);
        }

        [Then("I should see the hint text {string}")]
        public async Task ThenIShouldSeeTheHintText(string hintText)
        {
            await Expect(_context.Page.Locator(".govuk-hint"))
                .ToHaveTextAsync(hintText);
        }
    }
}
