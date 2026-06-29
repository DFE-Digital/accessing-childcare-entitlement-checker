using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.E2eTests.Steps;

[Binding]
internal class RadioButtonSteps(IPage page)
{
    [When("I select the {string} radio button")]
    public async Task WhenISelectTheStringRadioButton(string label)
    {
        await page
            .GetByLabel(label, new PageGetByLabelOptions { Exact = true })
            .CheckAsync();
    }

    [When("I do not select a radio button")]
    public async Task WhenIDoNotSelectARadioButton()
    {
        var checkedRadios = page
            .GetByRole(AriaRole.Radio)
            .And(page.Locator(":checked"));

        await Expect(checkedRadios).ToHaveCountAsync(0);
    }

    [When(@"I should see (\d+) radio buttons with the following options:")]
    [Then(@"I should see (\d+) radio buttons with the following options:")]
    public async Task ThenIShouldSeeDRadioButtonsWithTheFollowingOptions(int expectedCount, DataTable dataTable)
    {
        var expectedOptions = dataTable.Rows.Select(r => r[0]).ToArray();

        if (expectedOptions.Length != expectedCount)
        {
            throw new Exception($"Step says {expectedCount} options but table has {expectedOptions.Length}");
        }

        await Expect(page.GetByRole(AriaRole.Radio))
            .ToHaveCountAsync(expectedCount);

        foreach (var option in expectedOptions)
        {
            await Expect(page.GetByRole(AriaRole.Radio, new PageGetByRoleOptions { Name = option, Exact = true }))
                .ToBeVisibleAsync();
        }
    }

    [Then("the {string} radio button should be selected")]
    public async Task ThenTheStringRadioButtonShouldBeSelected(string label)
    {
        await Expect(page.GetByLabel(label)).ToBeCheckedAsync();
    }

    [Then("all other options should be deselected")]
    public async Task ThenAllOtherOptionsShouldBeDeselected()
    {
        var checkedRadios = page
            .GetByRole(AriaRole.Radio)
            .And(page.Locator(":checked"));

        await Expect(checkedRadios).ToHaveCountAsync(1);
    }
}
