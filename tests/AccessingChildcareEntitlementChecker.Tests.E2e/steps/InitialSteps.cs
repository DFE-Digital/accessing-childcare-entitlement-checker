using Microsoft.Playwright;
using Reqnroll;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.steps
{
    [Binding]
    public class InitialSteps
    {
        private Context _context;

        public InitialSteps(Context context)
        {
            _context = context;
        }

        [Given("I am on the childcare entitlement checker website")]
        public async Task GivenIAmOnTheChildcareEntitlementCheckerWebsite()
        {
            var url = Environment.GetEnvironmentVariable("TEST_URL") ?? "http://localhost:5252/";
            await _context.Page.GotoAsync(url);
        }

        [Then("the page header is {string}")]
        public async Task ThenThePageHeaderIs(string expectedHeader)
        {
            await Assertions.Expect(
                _context.Page.GetByRole(AriaRole.Heading, new() { Level = 1 })
            ).ToHaveTextAsync(expectedHeader);
        }
        
        [When("I click the start button")]
        public async Task WhenIClickTheStartButton()
        {
            await _context.Page
                .GetByRole(AriaRole.Link, new() { Name = "Start now" })
                .ClickAsync();
        }

        [When("I click on Continue")]
        public async Task WhenIClickOnContinue()
        {
            await _context.Page.GetByRole(AriaRole.Button, new() { Name = "Continue" }).ClickAsync();
        }

        [Then("the country error is {string}")]
        public async Task ThenTheCountryErrorIs(string expectedError)
        {
            var error = _context.Page.Locator("#country-error");
            
            await Assertions.Expect(error).ToBeVisibleAsync();
            await Assertions.Expect(error).ToContainTextAsync(expectedError);
        }
    }
}
