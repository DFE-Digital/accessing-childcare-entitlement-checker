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
        public void GivenIAmOnTheChildcareEntitlementCheckerWebsite()
        {
            var url = Environment.GetEnvironmentVariable("TEST_URL") ?? "http://localhost:5252/";
            _context.Page.GotoAsync(url);
        }

        [Then("the page header is {string}")]
        public async Task ThenThePageHeaderIs(string p0)
        {
            var headerText = await _context.Page.Locator("h1").InnerTextAsync();
            Assert.Equal(p0, headerText);
        }

        [When("I click on Continue")]
        public async Task WhenIClickOnContinue()
        {
            await _context.Page.GetByRole(AriaRole.Button, new() { Name = "Continue" }).ClickAsync();
        }

        [Then("the country error is {string}")]
        public async Task ThenTheCountryErrorIs(string p0)
        {
            await Assertions.Expect(_context.Page.Locator("#country-error")).ToBeVisibleAsync();
            await Assertions.Expect(_context.Page.Locator("#country-error")).ToContainTextAsync("Please select where you live");
        }
    }
}