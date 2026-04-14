using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;
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
            await WhenIClickOnContinue();
        }

        [When("I click the back link")]
        public async Task WhenIClickTheBackLink()
        {
            await _context.Page
                .Locator(".govuk-back-link")
                .ClickAsync();
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