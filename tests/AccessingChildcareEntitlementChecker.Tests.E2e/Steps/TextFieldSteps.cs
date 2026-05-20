using Reqnroll;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.Steps;

[Binding]
public class TextFieldSteps
{
    private Context _context;

    public TextFieldSteps(Context context)
    {
        _context = context;
    }

    [When("I enter the text {string} into {string}")]
    public async Task WhenIEnterTheTextInto(string text, string label)
    {
        await _context.Page.GetByLabel(label).FillAsync(text);
    }

    [Then("I should see a text box with the label {string}")]
    public async Task ThenIShouldSeeATextBoxWithTheLabel(string label)
    {
        await _context.Page.GetByLabel(label).IsVisibleAsync();
    }
}
