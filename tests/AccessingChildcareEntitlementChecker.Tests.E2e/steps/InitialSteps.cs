using Microsoft.Playwright;
using Reqnroll;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.steps
{
    [Binding]
    public class InitialSteps
    {
        private Context _context;
        private SharedSteps _sharedSteps;

        public InitialSteps(Context context)
        {
            _context = context;
            _sharedSteps = new SharedSteps(context);
        }

        [Given("I am on the childcare entitlement checker website")]
        public async Task GivenIAmOnTheChildcareEntitlementCheckerWebsite()
        {
            await _context.Page.GotoAsync(_context.Uri.ToString());
        }

        [When("I click the start button")]
        public async Task WhenIClickTheStartButton()
        {
            await _context.Page
                .GetByRole(AriaRole.Link, new() { Name = "Start now" })
                .ClickAsync();
        }

        [Then("the {string} error is {string}")]
        public async Task ThenTheFieldErrorIs(string fieldName, string expectedError)
        {
            var error = _context.Page.Locator($"#{fieldName}-error");

            await Assertions.Expect(error).ToBeVisibleAsync();
            await Assertions.Expect(error).ToContainTextAsync(expectedError);
        }
    }
}
