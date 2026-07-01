using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.E2eTests.Steps;

[Binding]
internal class CheckboxSteps(IPage page)
{
    [When("I select the {string} checkbox")]
    public async Task WhenISelectTheStringCheckbox(string label)
    {
        await page
            .GetByLabel(label, new PageGetByLabelOptions { Exact = true })
            .CheckAsync();
    }

    [When("I deselect the {string} checkbox")]
    public async Task WhenIDeselectTheStringCheckbox(string label)
    {
        await page
            .GetByLabel(label, new PageGetByLabelOptions { Exact = true })
            .UncheckAsync();
    }

    [When("I do not select a checkbox")]
    [Then("no checkboxes are selected")]
    public async Task WhenIDoNotSelectACheckbox()
    {
        var checkedCheckboxes = page
            .GetByRole(AriaRole.Checkbox)
            .And(page.Locator(":checked"));

        await Expect(checkedCheckboxes).ToHaveCountAsync(0);
    }

    [Then(@"I should see (\d+) checkboxes with the following options:")]
    public async Task ThenIShouldSeeDCheckboxesWithTheFollowingOptions(int expectedCount, DataTable dataTable)
    {
        var expectedOptions = dataTable.Rows.Select(r => r[0]).ToArray();

        if (expectedOptions.Length != expectedCount)
        {
            throw new Exception($"Step says {expectedCount} options but table has {expectedOptions.Length}");
        }

        await Expect(page.GetByRole(AriaRole.Checkbox))
            .ToHaveCountAsync(expectedCount);

        foreach (var option in expectedOptions)
        {
            await Expect(page.GetByRole(AriaRole.Checkbox, new PageGetByRoleOptions { Name = option, Exact = true }))
                .ToBeVisibleAsync();
        }
    }

    [Then("the following checkboxes should be selected:")]
    public async Task ThenTheFollowingCheckboxesShouldBeSelected(DataTable dataTable)
    {
        foreach (var row in dataTable.Rows)
        {
            var label = row[0];
            await Expect(page.GetByLabel(label)).ToBeCheckedAsync();
        }
    }
}
