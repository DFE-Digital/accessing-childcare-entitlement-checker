using Microsoft.Playwright;
using Reqnroll;

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
        await page.GetByLabel(label).IsVisibleAsync();
    }
}
