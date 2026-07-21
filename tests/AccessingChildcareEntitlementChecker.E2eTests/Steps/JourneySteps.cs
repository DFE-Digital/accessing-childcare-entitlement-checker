using AccessingChildcareEntitlementChecker.E2eTests.Pages;
using AccessingChildcareEntitlementChecker.E2eTests.Extensions;
using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.E2eTests.Steps;

[Binding]
internal class JourneySteps(IPage page)
{
    [Given("I am on the childcare entitlement checker website")]
    public async Task GivenIAmOnTheChildcareEntitlementCheckerWebsite()
    {
        await page.GotoAsync("/");
    }

    [Given("I click the link to start the journey")]
    public async Task GivenIClickTheLinkToStartTheJourney()
    {
        var startPage = new StartPage(page);
        await startPage.ContinueAsync();
    }

    [Given("I answer {string} as {string}")]
    [When("I answer {string} as {string}")]
    public async Task GivenIAnswerStringAsString(string question, string answer)
    {
        var factory = new PageFactory(page);
        var pageObj = factory.GetPage(question);
        await pageObj.AssertHeaderAsync();
        await pageObj.AnswerAsync(answer);
        await pageObj.ContinueAsync();
    }

    [Given("I answer questions as follows:")]
    [When("I answer questions as follows:")]
    public async Task GivenIAnswerQuestionsAsFollows(DataTable answers)
    {
        var factory = new PageFactory(page);
        foreach (var (pageName, answer) in answers.ToPageAnswerPairs())
        {
            var pageObj = factory.GetPage(pageName);
            await pageObj.AssertHeaderAsync();
            await pageObj.AnswerAsync(answer);
            await pageObj.ContinueAsync();
        }
    }

    [Given(@"I answer questions for ""(.*)"" as follows:")]
    [When(@"I answer questions for ""(.*)"" as follows:")]
    [Given(@"I answers the following questions about my child ""(.*)""")]
    public async Task GivenIAnswersTheFollowingQuestionsAboutMyChild(string childName, DataTable answers)
    {
        var factory = new PageFactory(page);
        foreach (var (pageName, answer) in answers.ToPageAnswerPairs())
        {
            var pageObj = factory.GetPage(pageName);
            await pageObj.AssertHeaderAsync();
            await pageObj.AnswerAsync(answer);
            await pageObj.ContinueAsync();
        }
    }

    [Given("I start the journey, filling in Aydin's and Sara's details")]
    public async Task GivenIStartTheJourneyFillingInAydinsDetails()
    {
        await GivenIClickTheLinkToStartTheJourney();

        var locationPage = new LocationPage(page);
        await locationPage.AssertHeaderAsync();
        await locationPage.AnswerAsync("England");
        await locationPage.ContinueAsync();

        // Aydin details
        var childName = new ChildNamePage(page);
        await childName.AssertHeaderAsync();
        await childName.AnswerAsync("Aydin");
        await childName.ContinueAsync();

        var childIsBorn = new ChildIsBornPage(page);
        await childIsBorn.AssertHeaderAsync();
        await childIsBorn.AnswerAsync("No");
        await childIsBorn.ContinueAsync();

        var childDueDate = new ChildDueDatePage(page);
        await childDueDate.AssertHeaderAsync();
        await childDueDate.AnswerAsync("Tomorrow");
        await childDueDate.ContinueAsync();

        // Add another child
        await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Add another child" }).ClickAsync();

        // Sara details
        await childName.AssertHeaderAsync();
        await childName.AnswerAsync("Sara");
        await childName.ContinueAsync();

        await childIsBorn.AssertHeaderAsync();
        await childIsBorn.AnswerAsync("Yes");
        await childIsBorn.ContinueAsync();

        var childBirthDate = new ChildBirthDatePage(page);
        await childBirthDate.AssertHeaderAsync();
        await childBirthDate.AnswerAsync("Yesterday");
        await childBirthDate.ContinueAsync();

        var childSupport = new ChildSupportPage(page);
        await childSupport.AssertHeaderAsync();
        await childSupport.AnswerAsync("Education, health and care (EHC) plan");
        await childSupport.ContinueAsync();
    }

    [Given("I check my children's details and click on Continue")]
    public async Task GivenICheckMyChildrensDetailsAndClickOnContinue()
    {
        await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Continue" }).ClickAsync();
    }

    [Given("I fill in my own details")]
    public async Task GivenIFillInMyOwnDetails()
    {
        var factory = new PageFactory(page);
        var answers = new[]
        {
            ("What is your age?", "Under 18"),
            ("What is your nationality?", "British or Irish citizen"),
            ("Are you in paid work?", "Yes"),
            ("How would you describe your work status?", "Self-employed"),
            ("Have you been self-employed for less than 12 months?", "No"),
            ("On average, do you expect to earn £203 a week or more before tax?", "Yes"),
            ("Do you expect your adjusted net income to be more than £100,000 for the current tax year?", "No"),
            ("Does your household receive universal credit?", "Yes"),
            ("Do you get any of these benefits?", "Carer's Allowance"),
            ("Do you already get any of these to help pay for childcare?", "Childcare vouchers"),
            ("How do you receive your childcare vouchers?", "A workplace nursery scheme"),
        };

        foreach (var (pageName, answer) in answers)
        {
            var pageObj = factory.GetPage(pageName);
            await pageObj.AssertHeaderAsync();
            await pageObj.AnswerAsync(answer);
            await pageObj.ContinueAsync();
        }
    }

    [Given("I fill in my partner's details")]
    public async Task GivenIFillInMyPartnersDetails()
    {
        var factory = new PageFactory(page);
        var answers = new[]
        {
            ("Do you live with a partner?", "Yes"),
            ("What is your partner's age?", "21 or over"),
            ("Is your partner in paid work?", "No, they are not in work"),
            ("Does your partner get any of these benefits?", "Carer's Allowance"),
            ("Does your partner already get any of these to help pay for childcare?", "Childcare vouchers"),
            ("How does your partner receive childcare vouchers?", "A workplace nursery scheme"),
        };

        foreach (var (pageName, answer) in answers)
        {
            var pageObj = factory.GetPage(pageName);
            await pageObj.AssertHeaderAsync();
            await pageObj.AnswerAsync(answer);
            await pageObj.ContinueAsync();
        }
    }

    [Given("I click the Add another child button")]
    [When("I click the Add another child button")]
    public async Task GivenIClickTheAddAnotherChildButton()
    {
        await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Add another child" }).ClickAsync();
    }

    [When("I answer the questions as follows:")]
    public async Task WhenIAnswerTheQuestionsAsFollows(DataTable answers)
    {
        var factory = new PageFactory(page);
        foreach (var (pageName, answer) in answers.ToPageAnswerPairs())
        {
            var pageObj = factory.GetPage(pageName);
            await pageObj.AssertHeaderAsync();
            await pageObj.AnswerAsync(answer);
            await pageObj.ContinueAsync();
        }
    }

    [When(@"the page header is ""(.*)""")]
    [Then(@"the page header is ""(.*)""")]
    public async Task WhenThePageHeaderIs(string expectedHeader)
    {
        await Expect(page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions { Level = 1 })).ToHaveTextAsync(expectedHeader);
    }

    [When("I click on Continue")]
    public async Task WhenIClickOnContinue()
    {
        await page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Continue" }).ClickAsync();
    }

    [When("I click the back link")]
    public async Task WhenIClickTheBackLink()
    {
        await page.Locator(".govuk-back-link").ClickAsync();
    }

    [Then(@"I will be directed to the next page in the user journey ""(.*)""")]
    public async Task ThenIWillBeDirectedToTheNextPageInTheUserJourney(string expectedHeader)
    {
        await Expect(page.Locator("body")).ToContainTextAsync(expectedHeader);
    }

    [Then(@"I should be returned to the previous page in the user journey ""(.*)""")]
    public async Task ThenIShouldBeReturnedToThePreviousPageInTheUserJourney(string expectedHeader)
    {
        await Expect(page.GetByRole(AriaRole.Heading, new PageGetByRoleOptions { Level = 1 })).ToHaveTextAsync(expectedHeader);
    }
}
