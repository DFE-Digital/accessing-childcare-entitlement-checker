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
            _context.Page.GotoAsync("https://sjm-test-webapp-feature-initial-gith-c02a9811.azurewebsites.net/");
        }

        [Then("the page header is {string}")]
        public async Task ThenThePageHeaderIs(string p0)
        {
            var headerText = await _context.Page.Locator("h1").InnerTextAsync();
            Assert.Equal(p0, headerText);
        }

        [When("I click on Start Now")]
        public async Task WhenIClickOnStartNow()
        {
            await _context.Page.GetByRole(AriaRole.Link, new() { Name = "Start Now" }).ClickAsync();
        }
    }
}
