using AccessingChildcareEntitlementChecker.E2eTests.Data;
using AccessingChildcareEntitlementChecker.E2eTests.Data.Builders;
using AccessingChildcareEntitlementChecker.E2eTests.Pages;
using Microsoft.Playwright;
using Reqnroll;

namespace AccessingChildcareEntitlementChecker.E2eTests.Steps;

[Binding]
internal class UseCaseSteps(IPage page)
{
    [Given("I complete the journey for the use case {string}")]
    public async Task GivenICompleteTheJourneyForTheUseCaseString(string useCaseName)
    {
        var journey = UseCaseRepository.GetJourney(useCaseName);
        var factory = new PageFactory(page);

        foreach (var step in journey)
        {
            switch (step)
            {
                case ActionStep action:
                    await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = action.ActionName }).ClickAsync();
                    break;
                case AnswerStep answer:
                    var pageObj = factory.GetPage(answer.PageName);
                    await pageObj.AssertHeaderAsync();
                    await pageObj.AnswerAsync(answer.Answer);
                    await pageObj.ContinueAsync();
                    break;
            }
        }
    }
}
