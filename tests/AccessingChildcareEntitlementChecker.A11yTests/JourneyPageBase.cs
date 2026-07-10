using System.Text.RegularExpressions;
using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.A11yTests;

public abstract class JourneyPageBase : PageBase
{
    protected JourneyPageBase(ITestOutputHelper output) : base(output) { }

    private const string DefaultChildName = "Jack";
    protected async Task AnswerLocation(string location = "England")
    {
        await Page.GotoAsync(BuildUrl("where-do-you-live"));
        await Page.GetByLabel(location).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/children/add-child-details");
    }

    protected async Task<Guid> AddChild(string childName = DefaultChildName)
    {
        await Page.GotoAsync(BuildUrl("children/add-child-details"));
        await Page.GetByLabel("What name should we use for this child?").FillAsync(childName);
        await Continue();

        var uri = new Uri(Page.Url);

        Assert.StartsWith("/children/", uri.AbsolutePath);
        Assert.EndsWith("/has-the-child-been-born", uri.AbsolutePath);

        return ExtractChildIdFromCurrentUrl();
    }

    protected async Task AnswerChildHasBeenBorn(Guid childId, bool hasBeenBorn = true)
    {
        if (hasBeenBorn)
        {
            await Page.GetByLabel("Yes").CheckAsync();
            await Continue();
            await ExpectPathAndQuery($"/children/{childId}/childs-date-of-birth");

            return;
        }

        await Page.GetByLabel("No").CheckAsync();
        await Continue();
        await ExpectPathAndQuery($"/children/{childId}/expectant-childs-due-date");
    }

    protected async Task EnterChildDateOfBirth(Guid childId, string day = "1", string month = "1", string year = "2024")
    {
        await Page.GetByLabel("Day").FillAsync(day);
        await Page.GetByLabel("Month").FillAsync(month);
        await Page.GetByLabel("Year").FillAsync(year);
        await Continue();
        await ExpectPathAndQuery($"/children/{childId}/child-benefits");
    }

    protected async Task SelectChildSupportOptions(Guid childId)
    {
        await Page.GetByLabel("No, none of these apply").CheckAsync();
        await Continue();
        await ExpectPathAndQuery($@"/children/check-childs-details?childId={childId}");
    }

    protected async Task<Guid> CompleteBornChildToSummary(string childName = DefaultChildName)
    {
        var childId = await AddChild(childName);
        await AnswerChildHasBeenBorn(childId);
        await EnterChildDateOfBirth(childId);
        await SelectChildSupportOptions(childId);

        return childId;
    }

    protected Guid ExtractChildIdFromCurrentUrl()
    {
        var segments = new Uri(Page.Url).AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

        if (segments.Length < 2 || !string.Equals(segments[0], "children", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Could not extract child ID from current URL: {Page.Url}");
        }

        if (!Guid.TryParse(segments[1], out var childId))
        {
            throw new InvalidOperationException($"Could not parse child ID from current URL: {Page.Url}");
        }

        return childId;
    }

    protected async Task AnswerUserAge(string ageOption = "21 or over")
    {
        await Page.GotoAsync(BuildUrl("/age/parent-age"));
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
        await ExpectPathAndQuery($"/work-status/work");
    }

    protected async Task AnswerUserNationalityCitizenOfEU(string nationalityOption = "Citizen of an EU country, EEA country or Switzerland")
    {
        await Page.GotoAsync(BuildUrl("/nationality"));
        await Page.GetByLabel(nationalityOption).CheckAsync();
        await Continue();
        await ExpectPathAndQuery($"/nationality/settled-status");
    }

    protected async Task AnswerUserPaidWorkStatus(string paidWorkStatus = "Yes")
    {
        await Page.GotoAsync(BuildUrl("/work-status/work"));
        await Page.GetByLabel(paidWorkStatus, new() { Exact = true }).CheckAsync();
        await Continue();
        await ExpectPathAndQuery($"/work-status/work-status");
    }

    protected async Task AnswerPartnerPaidWorkStatus(string paidWorkStatus = "Yes")
    {
        await Page.GotoAsync(BuildUrl("/work-status/work-partner"));
        await Page.GetByLabel(paidWorkStatus, new() { Exact = true }).CheckAsync();
        await Continue();
        await ExpectPathAndQuery($"/work-status/work-status-partner");
    }

    protected async Task AnswerUserIsOnParentalLeave(string paidWorkStatus = "Yes, but I am on parental leave")
    {
        await Page.GotoAsync(BuildUrl("/work-status/work"));
        await Page.GetByLabel(paidWorkStatus).CheckAsync();
        await Continue();
        await ExpectPathAndQuery($"/leave/parental-leave");
    }

    protected async Task AnswerUserWorkStatus(string workStatus = "Paid employment")
    {
        await Page.GotoAsync(BuildUrl("/work-status/work-status"));
        await Page.GetByLabel(workStatus).CheckAsync();
        await Continue();
        await ExpectPathAndQuery($"/earnings/wage");
    }

    protected async Task AnswerPartnerWorkStatus(string workStatus = "Paid employment")
    {
        await Page.GotoAsync(BuildUrl("/work-status/work-status-partner"));
        await Page.GetByLabel(workStatus).CheckAsync();
        await Continue();
        await ExpectPathAndQuery($"/earnings/wage-partner");
    }

    protected async Task AnswerUserWorkStatusSelfEmployed(string workStatus = "Self-employed")
    {
        await Page.GotoAsync(BuildUrl("/work-status/work-status"));
        await Page.GetByLabel(workStatus).CheckAsync();
        await Continue();
        await ExpectPathAndQuery($"/work-status/self-employed");
    }

    protected async Task AnswerUserWeeklyEarnings(string weeklyEarnings = "Yes")
    {
        await Page.GotoAsync(BuildUrl("/earnings/wage"));
        await Page.GetByLabel(weeklyEarnings).CheckAsync();
        await Continue();
        await ExpectPathAndQuery($"/earnings/adjusted-net-income");
    }

    protected async Task AnswerUserYearlyEarnings(string yearlyEarnings = "No")
    {
        await Page.GotoAsync(BuildUrl("/earnings/adjusted-net-income"));
        await Page.GetByLabel(yearlyEarnings).CheckAsync();
        await Continue();
        await ExpectPathAndQuery($"/benefits/universal-credit");
    }

    protected async Task AnswerUserUniversalCredit(string universalCredit = "No")
    {
        await Page.GotoAsync(BuildUrl("/benefits/universal-credit"));
        await Page.GetByLabel(universalCredit).CheckAsync();
        await Continue();
        await ExpectPathAndQuery($"/benefits/benefits");
    }

    protected async Task AnswerUserBenefits(string benefits = "No")
    {
        await Page.GotoAsync(BuildUrl("/benefits/benefits"));
        await Page.GetByLabel(benefits).CheckAsync();
        await Continue();
        await ExpectPathAndQuery($"/benefits/childcare-support");
    }

    protected async Task AnswerUserChildcareSupport(string childcareSupport = "No")
    {
        await Page.GotoAsync(BuildUrl("/benefits/childcare-support"));
        await Page.GetByLabel(childcareSupport).CheckAsync();
        await Continue();
        await ExpectPathAndQuery($"/partner");
    }

    protected async Task AnswerUserHasPartner(bool hasPartner)
    {
        await Page.GotoAsync(BuildUrl("/partner"));
        await Page.GetByLabel(hasPartner ? "Yes" : "No").CheckAsync();
        await Continue();
    }

    protected async Task<Guid> CompleteJourneyToCheckYourAnswers()
    {
        await AnswerLocation();
        var childId = await CompleteBornChildToSummary();
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

        return childId;
    }

    protected async Task<Guid> CompleteJourneyToResults()
    {
        var childId = await CompleteJourneyToCheckYourAnswers();
        await Continue();
        await ExpectPathAndQuery($"/results");

        return childId;
    }

    protected async Task<Guid> CompleteJourneyToResultsDetailed()
    {
        var childId = await CompleteJourneyToResults();
        await Page.GetByRole(AriaRole.Link, new() { Name = $"See the full details for {DefaultChildName}" }).ClickAsync();
        await ExpectPathAndQuery($"/Results/ResultsDetailed?childId={childId}");

        return childId;
    }

}