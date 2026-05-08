using Reqnroll;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.steps.Introduction
{
    [Binding]
    public class ChildNameSteps
    {
        private Context _context;

        public ChildNameSteps(Context context)
        {
            _context = context;
        }

        [Then("I should see a text box with the label {string}")]
        public async Task ThenIShouldSeeATextBoxWithTheLabel(string label)
        {
            await _context.Page.GetByLabel(label).IsVisibleAsync();
        }

        [Given("I have entered the text {string} into {string}")]
        public async Task GivenIHaveEnteredTheTextInto(string text, string label)
        {
            await _context.Page.GetByLabel(label).FillAsync(text);
        }
    }
}
