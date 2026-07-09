using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class PaidWorkStatusTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string ChildId = "9fbb8965-c988-4199-8b40-189efcfe2a1e";

    [Theory]
    [InlineData(null, NationalityOption.CitizenOfADifferentCountry, "/nationality")]
    [InlineData(null, NationalityOption.BritishOrIrishCitizen, "/nationality")]
    [InlineData(null, NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, "/nationality/settled-status")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.CitizenOfADifferentCountry, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, NationalityOption.CitizenOfADifferentCountry, "/children/check-childs-details")]
    public async Task Get(string? returnTo, NationalityOption? nationality, string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Nationality = nationality,
        });

        var url = $"/work-status/work?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertRadioButtonCount(4)
            .AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, PaidWorkOption.Yes, false, null, null, "/work-status/work-status")]
    [InlineData(null, PaidWorkOption.No, false, null, null, "/benefits/universal-credit")]
    [InlineData(null, PaidWorkOption.ParentalLeave, true, null, null, "/leave/parental-leave")]
    [InlineData(null, PaidWorkOption.SickLeave, false, null, null, "/work-status/work-status")]
    [InlineData(ReturnTo.CheckAnswers, PaidWorkOption.Yes, false, null, null, "/work-status/work-status")]
    [InlineData(ReturnTo.CheckAnswers, PaidWorkOption.Yes, false, WorkStatusOption.PaidEmployment, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, PaidWorkOption.No, false, null, null, "/benefits/universal-credit")]
    [InlineData(ReturnTo.CheckAnswers, PaidWorkOption.No, false, null, UniversalCreditOption.Receives, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, PaidWorkOption.ParentalLeave, true, null, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, PaidWorkOption.ParentalLeave, false, null, null, "/leave/parental-leave")]
    [InlineData(ReturnTo.CheckAnswers, PaidWorkOption.SickLeave, false, null, null, "/work-status/work-status")]
    [InlineData(ReturnTo.CheckAnswers, PaidWorkOption.SickLeave, false, WorkStatusOption.PaidEmployment, null, "/check-your-answers")]
    public async Task Post_Valid_Redirects(
        string? returnTo,
        PaidWorkOption paidWork,
        bool hasAnsweredParentalLeaveChildren,
        WorkStatusOption? workStatus,
        UniversalCreditOption? universalCredit,
        string continueUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        ChildId,
                        new Child(ChildId, "Sara")
                    }
                },
            PaidWork = paidWork,
            ParentalLeaveChildrenIds = hasAnsweredParentalLeaveChildren ? [ChildId] : [],
            WorkStatus = workStatus is null ? new() : [workStatus.Value],
            UniversalCredit = universalCredit,
        });

        var url = $"/work-status/work?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
            new KeyValuePair<string, string>("PaidWork", paidWork.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, NationalityOption.CitizenOfADifferentCountry, "/nationality")]
    [InlineData(null, NationalityOption.BritishOrIrishCitizen, "/nationality")]
    [InlineData(null, NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, "/nationality/settled-status")]
    [InlineData(ReturnTo.CheckAnswers, NationalityOption.CitizenOfADifferentCountry, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, NationalityOption.CitizenOfADifferentCountry, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(string? returnTo, NationalityOption? nationality, string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Nationality = nationality,
        });

        var url = $"/work-status/work?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [], TestContext.Current.CancellationToken);
        var postDocument = await HtmlHelpers.ParseHtmlAsync(postResponse.Content);
        postDocument.AssertValidationError()
            .AssertBackLink(backLinkUrl);
    }
}
