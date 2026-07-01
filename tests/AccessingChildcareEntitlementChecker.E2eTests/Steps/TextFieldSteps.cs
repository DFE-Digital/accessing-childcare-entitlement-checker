using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.E2eTests.Steps;

[Binding]
internal class TextFieldSteps(IPage page)
{
    [When("I enter the text {string} into {string}")]
    public async Task WhenIEnterTheTextStringIntoString(string text, string label)
    {
        await page.GetByLabel(label).FillAsync(text);
    }

    [Then("I should see a text box with the label {string}")]
    public async Task ThenIShouldSeeATextBoxWithTheLabelString(string label)
    {
        await Expect(page.GetByLabel(label)).ToBeVisibleAsync();
    }

    [Then("the text box with the label {string} is empty")]
    public async Task ThenTheTextBoxWithTheLabelIsEmpty(string label)
    {
        await Expect(page.GetByLabel(label))
                    .ToHaveValueAsync(string.Empty);
    }
}
