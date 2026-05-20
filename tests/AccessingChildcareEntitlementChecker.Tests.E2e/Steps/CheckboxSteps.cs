using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.Steps;

[Binding]
public class CheckboxSteps
{
    private Context _context;

    public CheckboxSteps(Context context)
    {
        _context = context;
    }

    [When("I select the {string} checkbox")]
    public async Task WhenISelectTheCheckbox(string label)
    {
        await _context.Page
            .GetByLabel(label, new() { Exact = true })
            .CheckAsync();
    }

    [When("I do not select a checkbox")]
    public async Task WhenIDoNotSelectACheckbox()
    {
        var checkedCheckboxes = _context.Page
            .GetByRole(AriaRole.Checkbox)
            .And(_context.Page.Locator(":checked"));

        await Expect(checkedCheckboxes).ToHaveCountAsync(0);
    }

    [Then(@"I should see (\d+) checkboxes with the following options:")]
    public async Task ThenIShouldSeeCheckboxesWithTheFollowingOptions(int expectedCount, DataTable dataTable)
    {
        var expectedOptions = dataTable.Rows.Select(r => r[0]).ToArray();

        if (expectedOptions.Length != expectedCount)
        {
            throw new Exception($"Step says {expectedCount} options but table has {expectedOptions.Length}");
        }

        await Expect(_context.Page.GetByRole(AriaRole.Checkbox))
            .ToHaveCountAsync(expectedCount);

        foreach (var option in expectedOptions)
        {
            await Expect(_context.Page.GetByRole(AriaRole.Checkbox, new() { Name = option, Exact = true }))
                .ToBeVisibleAsync();
        }
    }

    [Then("the following checkboxes should be selected:")]
    public void ThenTheFollowingCheckboxesShouldBeSelected(DataTable dataTable)
    {
        foreach (var row in dataTable.Rows)
        {
            var label = row[0];
            Expect(_context.Page.GetByLabel(label)).ToBeCheckedAsync();
        }
    }
}
