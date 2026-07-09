using System.Text.RegularExpressions;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests;

public abstract class JourneyPageBase(ITestOutputHelper output) : PageBase(output)
{
    protected async Task AnswerLocation(string location = "England")
    {
        //await Page.GotoAsync($"{ServiceUrl}/where-do-you-live");
        await GoToPath("/where-do-you-live");
        //await Page.GetByLabel(location).CheckAsync();
        await CheckLabel(location);
        await Continue();
        await Expect(Page).ToHaveURLAsync(new Regex(@"/children/add-child-details$"));
    }

    protected async Task<Guid> AddChild(string childName = "Jack")
    {
        //await Page.GotoAsync($"{ServiceUrl}/children/add-child-details");
        await GoToPath("/children/add-child-details");
        //await Page.GetByLabel("What name should we use for this child?").FillAsync(childName);
        await FillLabel("What name should we use for this child?", childName);
        await Continue();
        await Expect(Page).ToHaveURLAsync(new Regex(@"/children/(?<childId>[0-9a-f-]+)/has-the-child-been-born$"));

        return ExtractChildIdFromCurrentUrl();
    }

    protected async Task AnswerChildHasBeenBorn(Guid childId, bool hasBeenBorn = true)
    {
        if (hasBeenBorn)
        {
            await Page.GetByLabel("Yes").CheckAsync();
            await Continue();
            await Expect(Page).ToHaveURLAsync(new Regex($@"/children/{childId}/childs-date-of-birth$"));
            return;
        }

        await Page.GetByLabel("No").CheckAsync();
        await Continue();
        await Expect(Page).ToHaveURLAsync(new Regex($@"/children/{childId}/expectant-childs-due-date$"));

    }

    protected async Task EnterChildDateOfBirth(Guid childId, string day = "1", string month = "1", string year = "2024")
    {
        await Page.GetByLabel("Day").FillAsync(day);
        await Page.GetByLabel("Month").FillAsync(month);
        await Page.GetByLabel("Year").FillAsync(year);
        await Continue();
        await Expect(Page).ToHaveURLAsync(new Regex($@"/children/{childId}/child-benefits$"));
    }

    protected async Task SelectChildSupportOptions(Guid childId)
    {
        await Page.GetByLabel("No, none of these apply").CheckAsync();
        await Continue();
        await Expect(Page).ToHaveURLAsync(new Regex($@"/children/check-childs-details\?childId={childId}$"));
    }

    protected async Task<Guid> CompleteBornChildToSummary(string childName = "Jack")
    {
        var childId = await AddChild(childName);
        await AnswerChildHasBeenBorn(childId);
        await EnterChildDateOfBirth(childId);
        await SelectChildSupportOptions(childId);

        return childId;
    }

    protected Guid ExtractChildIdFromCurrentUrl()
    {
        var match = Regex.Match(Page.Url, @"/children/(?<childId>[0-9a-f-]+)");

        if (!match.Success)
        {
            throw new InvalidOperationException($"Could not extract child ID from current URL: {Page.Url}");
        }

        return Guid.Parse(match.Groups["childId"].Value);
    }

    protected async Task AnswerUserAge(string ageOption = "21 or over")
    {
        await Page.GotoAsync(BuildUrl("/age/parent-age"));

        output.WriteLine($"After navigating to parent age: {Page.Url}");
        output.WriteLine(await Page.Locator("body").InnerTextAsync());

        await Page.GetByLabel(ageOption).CheckAsync();

        await Continue();
    }

    protected async Task AnswerPartnerAge(string ageOption = "21 or over")
    {
        await Page.GotoAsync(BuildUrl("/age/partner-age"));
        await Page.GetByLabel(ageOption).CheckAsync();
        await Continue();
    }

    protected async Task AnswerUserNationality(string nationalityOption = "British or Irish citizen")
    {
        await Page.GotoAsync(BuildUrl("/nationality"));
        await Page.GetByLabel(nationalityOption).CheckAsync();
        await Continue();
        await Expect(Page).ToHaveURLAsync(new Regex("/work-status/work"));
    }

    protected async Task AnswerUserNationalityCitizenOfEU(string nationalityOption = "Citizen of an EU country, EEA country or Switzerland")
    {
        await Page.GotoAsync(BuildUrl("/nationality"));
        await Page.GetByLabel(nationalityOption).CheckAsync();
        await Continue();
        await Expect(Page).ToHaveURLAsync(new Regex("/nationality/settled-status"));
    }

    protected async Task AnswerUserPaidWorkStatus(string paidWorkStatus = "Yes")
    {
        await Page.GotoAsync(BuildUrl("/work-status/work"));
        await Page.GetByLabel(paidWorkStatus, new() { Exact = true }).CheckAsync();
        await Continue();
        await Expect(Page).ToHaveURLAsync(new Regex("/work-status/work-status"));

    }

    protected async Task AnswerUserIsOnParentalLeave(string paidWorkStatus = "Yes, but I am on parental leave")
    {
        await Page.GotoAsync(BuildUrl("/work-status/work"));
        await Page.GetByLabel(paidWorkStatus).CheckAsync();
        await Continue();
        await Expect(Page).ToHaveURLAsync(new Regex("/leave/parental-leave"));
    }

    protected async Task AnswerUserWorkStatus(string workStatus = "Paid employment")
    {
        await Page.GotoAsync(BuildUrl("/work-status/work-status"));
        await Page.GetByLabel(workStatus).CheckAsync();
        await Continue();
        await Expect(Page).ToHaveURLAsync(new Regex("/earnings/wage"));
    }

    protected async Task AnswerUserWorkStatusSelfEmployed(string workStatus = "Self-employed")
    {
        await Page.GotoAsync(BuildUrl("/work-status/work-status"));
        await Page.GetByLabel(workStatus).CheckAsync();
        await Continue();
        await Expect(Page).ToHaveURLAsync(new Regex("/work-status/self-employed"));
    }

    protected async Task AnswerUserWeeklyEarnings(string weeklyEarnings = "Yes")
    {
        await Page.GotoAsync(BuildUrl("/earnings/wage"));
        await Page.GetByLabel(weeklyEarnings).CheckAsync();
        await Continue();
        await Expect(Page).ToHaveURLAsync(new Regex("/earnings/adjusted-net-income"));
    }

    protected async Task AnswerUserYearlyEarnings(string yearlyEarnings = "No")
    {
        await Page.GotoAsync(BuildUrl("/earnings/adjusted-net-income"));
        await Page.GetByLabel(yearlyEarnings).CheckAsync();
        await Continue();
        await Expect(Page).ToHaveURLAsync(new Regex("/benefits/universal-credit"));
    }

    protected async Task AnswerUserUniversalCredit(string universalCredit = "No")
    {
        await Page.GotoAsync(BuildUrl("/benefits/universal-credit"));
        await Page.GetByLabel(universalCredit).CheckAsync();
        await Continue();
        await Expect(Page).ToHaveURLAsync(new Regex("/benefits/benefits"));
    }

    protected async Task AnswerUserBenefits(string benefits = "No")
    {
        await Page.GotoAsync(BuildUrl("/benefits/benefits"));
        await Page.GetByLabel(benefits).CheckAsync();
        await Continue();
        await Expect(Page).ToHaveURLAsync(new Regex("/benefits/childcare-support"));
    }

    protected async Task AnswerUserChildcareSupport(string childcareSupport = "No")
    {
        await Page.GotoAsync(BuildUrl("/benefits/childcare-support"));
        await Page.GetByLabel(childcareSupport).CheckAsync();
        await Continue();
        await Expect(Page).ToHaveURLAsync(new Regex("/partner"));
    }

    protected async Task AnswerUserHasPartner(bool hasPartner)
    {
        await Page.GotoAsync(BuildUrl("/partner"));
        await Page.GetByLabel(hasPartner ? "Yes" : "No").CheckAsync();
        await Continue();
    }

    protected async Task CompleteJourneyToCheckYourAnswers()
    {
        await AnswerLocation();
        await CompleteBornChildToSummary();
        await AnswerUserAge();
        await AnswerUserNationality();
        await AnswerUserPaidWorkStatus();
        await AnswerUserWorkStatus();
        await AnswerUserWeeklyEarnings();
        await AnswerUserYearlyEarnings();
        await AnswerUserUniversalCredit();
        await AnswerUserBenefits();
        await AnswerUserChildcareSupport();
        await AnswerUserHasPartner(false);
    }

    protected async Task CompleteJourneyToResults()
    {
        await CompleteJourneyToCheckYourAnswers();
        await Continue();
        await Expect(Page).ToHaveURLAsync(new Regex(@"/results$"));
    }

    protected async Task CompleteJourneyToResultsDetailed()
    {
        await CompleteJourneyToResults();
        await Page.GetByRole(AriaRole.Link, new() { NameRegex = new Regex("See the full details for") }).ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex(@"/Results/ResultsDetailed\?childId=[0-9a-f-]+$"));
    }
    
    protected async Task CheckLabel(string label)
    {
        var locator = Page.GetByLabel(label);

        try
        {
            await locator.CheckAsync();
        }
        catch
        {
            await WritePageDiagnostics($"Could not check label: {label}");
            throw;
        }
    }

    protected async Task FillLabel(string label, string value)
    {
        var locator = Page.GetByLabel(label);

        try
        {
            await locator.FillAsync(value);
        }
        catch
        {
            await WritePageDiagnostics($"Could not fill label: {label}");
            throw;
        }
    }

}