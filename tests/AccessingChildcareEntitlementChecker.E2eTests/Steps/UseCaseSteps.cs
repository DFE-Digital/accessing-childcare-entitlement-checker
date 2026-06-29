using AccessingChildcareEntitlementChecker.E2eTests.Data;
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

        foreach (var (pageName, answer) in journey)
        {
            if (pageName == PageNames.Action)
            {
                await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = answer }).ClickAsync();
                continue;
            }

            var pageObj = factory.GetPage(pageName);
            await pageObj.AnswerAsync(answer);
            await pageObj.ContinueAsync();
        }
    }
}
