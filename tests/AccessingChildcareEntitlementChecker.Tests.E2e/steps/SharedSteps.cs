using Microsoft.Playwright;
using Reqnroll;

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
            await Assertions.Expect(
                _context.Page.GetByRole(AriaRole.Heading, new() { Level = 1 })
            ).ToHaveTextAsync(expectedHeader);
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
    }
}
