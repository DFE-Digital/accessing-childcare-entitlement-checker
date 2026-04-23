using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.steps
{
    [Binding]
    [Scope(Feature = "Partner Age")]
    public class PartnerAgeSteps
    {
        private readonly Context _context;

        public PartnerAgeSteps(Context context)
        {
            _context = context;
        }

        [Given("I am on the 'How old is your partner?' page")]
        public async Task GivenIAmOnThePartnerAgePage()
        {
            var url = new Uri(_context.Uri, "/Partner/PartnerAge");
            await _context.Page.GotoAsync(url.AbsoluteUri);
        }

        [Then("the page header is {string}")]
        public async Task ThenThePageHeaderIs(string expectedHeader)
        {
            await Expect(
                _context.Page.GetByRole(AriaRole.Heading, new() { Level = 1 })
            ).ToHaveTextAsync(expectedHeader);
        }

        [Then("I should see three radio buttons with the following options:")]
        public async Task ThenIShouldSeeThreeRadioButtonsWithTheFollowingOptions(DataTable dataTable)
        {
            var expectedOptions = dataTable.Rows.Select(r => r[0]).ToArray();
            await Expect(_context.Page.GetByRole(AriaRole.Radio)).ToHaveCountAsync(expectedOptions.Length);
            foreach (var option in expectedOptions)
            {
                await Expect(_context.Page.GetByRole(AriaRole.Radio, new() { Name = option })).ToBeVisibleAsync();
            }
        }

        [Given("I have selected the {string} radio button")]
        [When("I select the {string} radio button")]
        public async Task WhenISelectTheRadioButton(string label)
        {
            await _context.Page
                .GetByLabel(label)
                .CheckAsync();
        }

        [Then("the {string} radio button should be selected")]
        public async Task ThenTheRadioButtonShouldBeSelected(string label)
        {
            await Expect(_context.Page.GetByLabel(label)).ToBeCheckedAsync();
        }

        [Then("all other options should be deselected")]
        public async Task ThenAllOtherOptionsShouldBeDeselected()
        {
            var checkedRadios = _context.Page
                .GetByRole(AriaRole.Radio)
                .And(_context.Page.Locator(":checked"));

            await Expect(checkedRadios).ToHaveCountAsync(1);
        }

        [Given("I have not selected an option")]
        public async Task GivenIHaveNotSelectedAnOption()
        {
            var checkedRadios = _context.Page
                .GetByRole(AriaRole.Radio)
                .And(_context.Page.Locator(":checked"));

            await Expect(checkedRadios).ToHaveCountAsync(0);
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
            await AssertHeader(expectedHeader);
        }

        private async Task AssertHeader(string expectedHeader)
        {
            await Expect(
                _context.Page.GetByRole(AriaRole.Heading, new() { Level = 1 })
            ).ToHaveTextAsync(expectedHeader);
        }
    }
}