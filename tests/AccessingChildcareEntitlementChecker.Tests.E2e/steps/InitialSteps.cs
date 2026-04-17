using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;
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


        [When("I select {string} for {string}")]
        public async Task WhenISelectOptionForField(string value, string fieldName)
        {
            await _context.Page
                .GetByLabel(value)
                .CheckAsync();
        }

        [Then("I see {string}")]
        public async Task ThenISeeOption(string option)
        {
            var radio = _context.Page.GetByRole(AriaRole.Radio, new() { Name = option });

            await Assertions.Expect(radio).ToBeVisibleAsync();
        }

        [Given("I am on the partner page")]
        public async Task GivenIAmOnThePartnerPage()
        {
            await GivenIAmOnTheChildcareEntitlementCheckerWebsite();
            await WhenIClickTheStartButton();
            await WhenISelectOptionForField("England", "Country");
            await _sharedSteps.WhenIClickOnContinue();
        }

        [Then("I see the text {string}")]
        public async Task ThenISeeTheText(string expectedText)
        {
            await Expect(_context.Page.Locator("body"))
                .ToContainTextAsync(expectedText);
        }

        [Then("{string} is selected for {string}")]
        public async Task ThenOptionIsSelectedForField(string value, string fieldName)
        {
            var radio = _context.Page.GetByRole(AriaRole.Radio, new() { Name = value });

            await Expect(radio).ToBeCheckedAsync();
        }

        [Then("{string} is not selected for {string}")]
        public async Task ThenOptionIsNotSelectedForField(string value, string fieldName)
        {
            var radio = _context.Page.GetByRole(AriaRole.Radio, new() { Name = value });

            await Expect(radio).Not.ToBeCheckedAsync();
        }
    }
}