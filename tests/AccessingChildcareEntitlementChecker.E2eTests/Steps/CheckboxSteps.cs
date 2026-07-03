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

    [Then(@"I should see the following checkboxes:")]
    public async Task ThenIShouldSeeTheFollowingCheckboxes(DataTable dataTable)
    {
        var expectedOptions = dataTable.Rows.Select(r => new
        {
            Name = r[0],
            Description = r.Count > 1 ? r[1] : null
        }).ToArray();
        await Expect(page.GetByRole(AriaRole.Checkbox))
            .ToHaveCountAsync(expectedOptions.Length);

        foreach (var option in expectedOptions)
        {
            var checkbox = page.GetByRole(AriaRole.Checkbox, new PageGetByRoleOptions { Name = option.Name, Exact = true });
            await Expect(checkbox)
                .ToBeVisibleAsync();
            if (!string.IsNullOrEmpty(option.Description))
            {
                await Expect(checkbox)
                    .ToHaveAccessibleDescriptionAsync(option.Description);
            }
        }
    }

    [Then("the following checkboxes should be selected:")]
    public async Task ThenTheFollowingCheckboxesShouldBeSelected(DataTable dataTable)
    {
        var selected = page.GetByRole(AriaRole.Checkbox, new PageGetByRoleOptions { Checked = true });
        await Expect(selected).ToHaveCountAsync(dataTable.Rows.Count);
        foreach (var row in dataTable.Rows)
        {
            var label = row[0];
            await Expect(page.GetByLabel(label)).ToBeCheckedAsync();
        }
    }
}
