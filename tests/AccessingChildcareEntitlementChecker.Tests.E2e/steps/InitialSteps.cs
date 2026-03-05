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
<<<<<<< HEAD
            _context.Page.GotoAsync("https://sjm-test-webapp-feature-initial-gith-c02a9811.azurewebsites.net/");
=======
            var url = Environment.GetEnvironmentVariable("TEST_URL") ?? "http://localhost:5252/";
            _context.Page.GotoAsync(url);
>>>>>>> main
        }

        [Then("the page header is {string}")]
        public async Task ThenThePageHeaderIs(string p0)
        {
            var headerText = await _context.Page.Locator("h1").InnerTextAsync();
            Assert.Equal(p0, headerText);
        }

<<<<<<< HEAD
        [When("I click on Start Now")]
        public async Task WhenIClickOnStartNow()
        {
            await _context.Page.GetByRole(AriaRole.Link, new() { Name = "Start Now" }).ClickAsync();
=======
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
>>>>>>> main
        }
    }
}
