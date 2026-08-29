using Microsoft.Playwright;

namespace Dfe.Acec.Web.Tests.A11y;

public abstract class JourneyPageBase(ITestOutputHelper output) : PageBase(output)
{
    private const string DefaultChildName = "Jack";

    protected async Task StartJourney()
    {
        await Page.GotoAsync("/");
        await ExpectPathAndQuery("/");
        await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Continue" }).ClickAsync();

        if (Settings.HmrcIntegrationEnabled)
        {
            await ExpectPathAndQuery("/children/add-child-details");
        }
        else
        {
            await ExpectPathAndQuery("/where-do-you-live");
        }
    }

    protected async Task AnswerLocation(string location = "England")
    {
        if (Settings.HmrcIntegrationEnabled)
        {
            return;
        }

        await ExpectPathAndQuery("/where-do-you-live");
        await Page.GetByLabel(location).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/children/add-child-details");
    }

    private async Task<Guid> AddChild(string childName = DefaultChildName)
    {
        await ExpectPathAndQuery("/children/add-child-details");
        await Page.GetByLabel("What name should we use for this child?").FillAsync(childName);
        await Continue();

        await Page.WaitForURLAsync(url =>
            url.Contains("/children/", StringComparison.Ordinal) &&
            url.EndsWith(
                "/has-the-child-been-born",
                StringComparison.Ordinal));

        var childId = ExtractChildIdFromCurrentUrl();
        await ExpectPathAndQuery($"/children/{childId}/has-the-child-been-born");

        return childId;
    }

    private async Task AnswerChildHasBeenBorn(Guid childId, bool hasBeenBorn = true)
    {
        await ExpectPathAndQuery($"/children/{childId}/has-the-child-been-born");

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
        await ExpectPathAndQuery($"/children/{childId}/childs-date-of-birth");
        await Page.GetByLabel("Day").FillAsync(day);
        await Page.GetByLabel("Month").FillAsync(month);
        await Page.GetByLabel("Year").FillAsync(year);
        await Continue();
        await ExpectPathAndQuery($"/children/{childId}/child-benefits");
    }

    protected async Task EnterExpectedChildDueDate(Guid childId, string day = "1", string month = "1", string year = "2027")
    {
        await ExpectPathAndQuery($"/children/{childId}/expectant-childs-due-date");
        await Page.GetByLabel("Day").FillAsync(day);
        await Page.GetByLabel("Month").FillAsync(month);
        await Page.GetByLabel("Year").FillAsync(year);
        await Continue();
        await ExpectPathAndQuery($"/children/{childId}/child-benefits");
    }

    private async Task SelectChildSupportOptions(Guid childId)
    {
        await ExpectPathAndQuery($"/children/{childId}/child-benefits");
        await Page.GetByLabel("No, none of these apply").CheckAsync();
        await Continue();
        await ExpectPathAndQuery($"/children/check-childs-details?childId={childId}");
    }

    private async Task AnswerUserAge(string ageOption = "21 or over")
    {
        await ExpectPathAndQuery("/age/parent-age");
        await Page.GetByLabel(ageOption).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/nationality");
    }

    private async Task AnswerPartnerAge(string ageOption = "21 or over")
    {
        await ExpectPathAndQuery("/age/partner-age");
        await Page.GetByLabel(ageOption).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/work-status/work-partner");
    }

    private async Task AnswerUserNationality(string nationalityOption = "British or Irish citizen")
    {
        await ExpectPathAndQuery("/nationality");
        await Page.GetByLabel(nationalityOption).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/work-status/work");
    }

    private async Task AnswerUserNationalityCitizenOfEu(string nationalityOption = "Citizen of an EU country, EEA country or Switzerland")
    {
        await ExpectPathAndQuery("/nationality");
        await Page.GetByLabel(nationalityOption).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/nationality/settled-status");
    }

    private async Task AnswerUserPaidWorkStatus(string paidWorkStatus = "Yes, I am currently in work")
    {
        await ExpectPathAndQuery("/work-status/work");
        await Page.GetByLabel(paidWorkStatus, new PageGetByLabelOptions { Exact = true }).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/work-status/work-status");
    }

    private async Task AnswerPartnerPaidWorkStatus(string paidWorkStatus = "Yes, they are currently in work")
    {
        await ExpectPathAndQuery("/work-status/work-partner");
        await Page.GetByLabel(paidWorkStatus, new PageGetByLabelOptions { Exact = true }).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/work-status/work-status-partner");
    }

    private async Task AnswerUserIsOnParentalLeave(string paidWorkStatus = "Yes, but I am on parental leave")
    {
        await ExpectPathAndQuery("/work-status/work");
        await Page.GetByLabel(paidWorkStatus).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/leave/parental-leave");
    }

    private async Task AnswerUserWorkStatus(string workStatus = "Paid employment")
    {
        await ExpectPathAndQuery("/work-status/work-status");
        await Page.GetByLabel(workStatus).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/earnings/wage");
    }

    private async Task AnswerPartnerWorkStatus(string workStatus = "Paid employment")
    {
        await ExpectPathAndQuery("/work-status/work-status-partner");
        await Page.GetByLabel(workStatus).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/earnings/wage-partner");
    }

    private async Task AnswerUserWorkStatusSelfEmployed(string workStatus = "Self-employed")
    {
        await ExpectPathAndQuery("/work-status/work-status");
        await Page.GetByLabel(workStatus).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/work-status/self-employed");
    }

    private async Task AnswerUserWeeklyEarnings(string weeklyEarnings = "Yes")
    {
        await ExpectPathAndQuery("/earnings/wage");
        await Page.GetByLabel(weeklyEarnings).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/earnings/adjusted-net-income");
    }

    private async Task AnswerPartnerWeeklyEarnings(string weeklyEarnings = "Yes")
    {
        await ExpectPathAndQuery("/earnings/wage-partner");
        await Page.GetByLabel(weeklyEarnings).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/earnings/adjusted-net-income-partner");
    }

    private async Task AnswerUserYearlyEarnings(string yearlyEarnings = "No")
    {
        await ExpectPathAndQuery("/earnings/adjusted-net-income");
        await Page.GetByLabel(yearlyEarnings).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/benefits/universal-credit");
    }

    private async Task AnswerPartnerYearlyEarnings(string yearlyEarnings = "No")
    {
        await ExpectPathAndQuery("/earnings/adjusted-net-income-partner");
        await Page.GetByLabel(yearlyEarnings).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/Partner/PartnerBenefits");
    }

    private async Task AnswerUserUniversalCredit(string universalCredit = "No")
    {
        await ExpectPathAndQuery("/benefits/universal-credit");
        await Page.GetByLabel(universalCredit).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/benefits/benefits");
    }

    private async Task AnswerUserBenefits(string benefits = "No")
    {
        await ExpectPathAndQuery("/benefits/benefits");
        await Page.GetByLabel(benefits).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/benefits/childcare-support");
    }

    private async Task AnswerPartnerBenefits(string benefits = "No")
    {
        await ExpectPathAndQuery("/Partner/PartnerBenefits");
        await Page.GetByLabel(benefits).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/benefits/childcare-support-partner");
    }

    private async Task AnswerUserChildcareSupport(string childcareSupport = "No")
    {
        await ExpectPathAndQuery("/benefits/childcare-support");
        await Page.GetByLabel(childcareSupport).CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/partner");
    }

    private async Task AnswerUserHasPartner(bool hasPartner)
    {
        await ExpectPathAndQuery("/partner");
        await Page.GetByLabel(hasPartner ? "Yes" : "No").CheckAsync();
        await Continue();
        await ExpectPathAndQuery(hasPartner ? "/age/partner-age" : "/check-your-answers");
    }

    private Guid ExtractChildIdFromCurrentUrl()
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

    protected async Task<Guid> GoToHasChildBeenBornPage(string childName = DefaultChildName)
    {
        await StartJourney();
        await AnswerLocation();
        var childId = await AddChild(childName);
        await ExpectPathAndQuery($"/children/{childId}/has-the-child-been-born");
        return childId;
    }

    protected async Task<Guid> GoToChildDateOfBirthPage(string childName = DefaultChildName)
    {
        var childId = await GoToHasChildBeenBornPage(childName);
        await AnswerChildHasBeenBorn(childId);
        await ExpectPathAndQuery($"/children/{childId}/childs-date-of-birth");
        return childId;
    }

    protected async Task<Guid> GoToExpectedChildDueDatePage(string childName = DefaultChildName)
    {
        var childId = await GoToHasChildBeenBornPage(childName);
        await AnswerChildHasBeenBorn(childId, false);
        await ExpectPathAndQuery($"/children/{childId}/expectant-childs-due-date");
        return childId;
    }

    protected async Task<Guid> CompleteBornChildToSummary(string childName = DefaultChildName)
    {
        var childId = await GoToChildDateOfBirthPage(childName);
        await EnterChildDateOfBirth(childId);
        await SelectChildSupportOptions(childId);
        return childId;
    }

    protected async Task<Guid> GoToUserAgePage()
    {
        var childId = await CompleteBornChildToSummary();
        await ExpectPathAndQuery($"/children/check-childs-details?childId={childId}");
        await Continue();
        await ExpectPathAndQuery("/age/parent-age");
        return childId;
    }

    protected async Task<Guid> GoToUserNationalityPage()
    {
        var childId = await GoToUserAgePage();
        await AnswerUserAge();
        await ExpectPathAndQuery("/nationality");
        return childId;
    }

    protected async Task GoToUserSettledStatusPage()
    {
        await GoToUserNationalityPage();
        await AnswerUserNationalityCitizenOfEu();
        await ExpectPathAndQuery("/nationality/settled-status");
    }

    protected async Task<Guid> GoToUserPaidWorkPage()
    {
        var childId = await GoToUserNationalityPage();
        await AnswerUserNationality();
        await ExpectPathAndQuery("/work-status/work");
        return childId;
    }

    protected async Task GoToUserParentalLeavePage()
    {
        await GoToUserPaidWorkPage();
        await AnswerUserIsOnParentalLeave();
        await ExpectPathAndQuery("/leave/parental-leave");
    }

    protected async Task<Guid> GoToUserWorkStatusPage()
    {
        var childId = await GoToUserPaidWorkPage();
        await AnswerUserPaidWorkStatus();
        await ExpectPathAndQuery("/work-status/work-status");
        return childId;
    }

    protected async Task GoToUserSelfEmployedDurationPage()
    {
        await GoToUserWorkStatusPage();
        await AnswerUserWorkStatusSelfEmployed();
        await ExpectPathAndQuery("/work-status/self-employed");
    }

    protected async Task<Guid> GoToUserWeeklyEarningsPage()
    {
        var childId = await GoToUserWorkStatusPage();
        await AnswerUserWorkStatus();
        await ExpectPathAndQuery("/earnings/wage");
        return childId;
    }

    protected async Task<Guid> GoToUserYearlyEarningsPage()
    {
        var childId = await GoToUserWeeklyEarningsPage();
        await AnswerUserWeeklyEarnings();
        await ExpectPathAndQuery("/earnings/adjusted-net-income");
        return childId;
    }

    protected async Task<Guid> GoToUserUniversalCreditPage()
    {
        var childId = await GoToUserYearlyEarningsPage();
        await AnswerUserYearlyEarnings();
        await ExpectPathAndQuery("/benefits/universal-credit");
        return childId;
    }

    protected async Task<Guid> GoToUserBenefitsPage()
    {
        var childId = await GoToUserUniversalCreditPage();
        await AnswerUserUniversalCredit();
        await ExpectPathAndQuery("/benefits/benefits");
        return childId;
    }

    protected async Task<Guid> GoToUserChildcareSupportPage()
    {
        var childId = await GoToUserBenefitsPage();
        await AnswerUserBenefits();
        await ExpectPathAndQuery("/benefits/childcare-support");
        return childId;
    }

    protected async Task GoToUserChildcareVouchersPage()
    {
        await GoToUserChildcareSupportPage();
        await Page.GetByLabel("Childcare vouchers").CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/benefits/childcare-vouchers");
    }

    protected async Task<Guid> GoToHasPartnerPage()
    {
        var childId = await GoToUserChildcareSupportPage();
        await AnswerUserChildcareSupport();
        await ExpectPathAndQuery("/partner");
        return childId;
    }

    protected async Task GoToPartnerAgePage()
    {
        await GoToHasPartnerPage();
        await AnswerUserHasPartner(true);
        await ExpectPathAndQuery("/age/partner-age");
    }
    protected async Task GoToPartnerPaidWorkStatusPage()
    {
        await GoToPartnerAgePage();
        await AnswerPartnerAge();
        await ExpectPathAndQuery("/work-status/work-partner");
    }

    protected async Task GoToPartnerWorkStatusPage()
    {
        await GoToPartnerPaidWorkStatusPage();
        await AnswerPartnerPaidWorkStatus();
        await ExpectPathAndQuery("/work-status/work-status-partner");
    }

    protected async Task GoToPartnerWeeklyEarningsPage()
    {
        await GoToPartnerPaidWorkStatusPage();
        await AnswerPartnerPaidWorkStatus();
        await AnswerPartnerWorkStatus();
        await ExpectPathAndQuery("/earnings/wage-partner");
    }

    protected async Task GoToPartnerYearlyEarningsPage()
    {
        await GoToPartnerWeeklyEarningsPage();
        await AnswerPartnerWeeklyEarnings();
        await ExpectPathAndQuery("/earnings/adjusted-net-income-partner");
    }

    protected async Task GoToPartnerBenefitsPage()
    {
        await GoToPartnerYearlyEarningsPage();
        await AnswerPartnerYearlyEarnings();
        await ExpectPathAndQuery("/Partner/PartnerBenefits");
    }

    protected async Task GoToPartnerChildcareSupportPage()
    {
        await GoToPartnerBenefitsPage();
        await AnswerPartnerBenefits();
        await ExpectPathAndQuery("/benefits/childcare-support-partner");
    }

    protected async Task GoToPartnerChildcareVouchersPage()
    {
        await GoToPartnerChildcareSupportPage();
        await Page.GetByLabel("Childcare vouchers").CheckAsync();
        await Continue();
        await ExpectPathAndQuery("/benefits/childcare-vouchers-partner");
    }


    protected async Task<Guid> CompleteJourneyToCheckYourAnswers()
    {
        var childId = await GoToHasPartnerPage();
        await AnswerUserHasPartner(false);
        await ExpectPathAndQuery("/check-your-answers");
        return childId;
    }

    protected async Task<Guid> CompleteJourneyToResults()
    {
        var childId = await CompleteJourneyToCheckYourAnswers();
        await Continue();
        await ExpectPathAndQuery("/results");
        return childId;
    }

    protected async Task CompleteJourneyToResultsDetailed()
    {
        var childId = await CompleteJourneyToResults();

        await Page.GetByRole(
                AriaRole.Link,
                new PageGetByRoleOptions { Name = $"View {DefaultChildName}'s full childcare support information" })
            .ClickAsync();

        await ExpectPathAndQuery($"/Results/ResultsDetailed?childId={childId}");
    }
}
