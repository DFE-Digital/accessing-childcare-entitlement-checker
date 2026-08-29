using Dfe.Acec.Tests.E2e.Pages;
using Dfe.Acec.Tests.E2e.UseCases;
using Dfe.Acec.Tests.E2e.UseCases.Builders;
using Microsoft.Playwright;
using Reqnroll;

namespace Dfe.Acec.Tests.E2e.Steps;

[Binding]
[Scope(Feature = "End to End Use Cases")]
internal sealed class UseCaseSteps(IPage page, TestSettings settings)
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
                    if (answer.PageName == "Where do you live?" && settings.HmrcIntegrationEnabled)
                    {
                        break;
                    }
                    var pageObj = factory.GetPage(answer.PageName);
                    await pageObj.AssertHeaderAsync();
                    await pageObj.AnswerAsync(answer.Answer);
                    await pageObj.ContinueAsync();
                    break;
            }
        }
    }
}
