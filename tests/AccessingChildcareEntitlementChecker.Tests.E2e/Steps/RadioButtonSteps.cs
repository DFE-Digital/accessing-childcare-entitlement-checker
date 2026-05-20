using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.Steps;

[Binding]
public class RadioButtonSteps
{
    private Context _context;

    public RadioButtonSteps(Context context)
    {
        _context = context;
    }

    [When("I select the {string} radio button")]
    public async Task WhenISelectTheRadioButton(string label)
    {
        await _context.Page
            .GetByLabel(label, new() { Exact = true })
            .CheckAsync();
    }

    [When("I do not select a radio button")]
    public async Task WhenIDoNotSelectARadioButton()
    {
        var checkedRadios = _context.Page
            .GetByRole(AriaRole.Radio)
            .And(_context.Page.Locator(":checked"));

        await Expect(checkedRadios).ToHaveCountAsync(0);
    }

    [When(@"I should see (\d+) radio buttons with the following options:")]
    [Then(@"I should see (\d+) radio buttons with the following options:")]
    public async Task ThenIShouldSeeRadioButtonsWithTheFollowingOptions(int expectedCount, DataTable dataTable)
    {
        var expectedOptions = dataTable.Rows.Select(r => r[0]).ToArray();

        if (expectedOptions.Length != expectedCount)
        {
            throw new Exception($"Step says {expectedCount} options but table has {expectedOptions.Length}");
        }

        await Expect(_context.Page.GetByRole(AriaRole.Radio))
            .ToHaveCountAsync(expectedCount);

        foreach (var option in expectedOptions)
        {
            await Expect(_context.Page.GetByRole(AriaRole.Radio, new() { Name = option, Exact = true }))
                .ToBeVisibleAsync();
        }
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
}
