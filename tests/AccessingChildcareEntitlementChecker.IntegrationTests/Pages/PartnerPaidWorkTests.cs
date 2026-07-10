using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class PartnerPaidWorkTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/work-status/work-partner";
    private const string ChildId = "9fbb8965-c988-4199-8b40-189efcfe2a1e";

    [Theory]
    [InlineData(null, null, null, null, "/nationality/nationality-partner")]
    [InlineData(null, null, null, NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, "/nationality/settled-status-partner")]
    [InlineData(null, NationalityOption.BritishOrIrishCitizen, null, null, "/age/partner-age")]
    [InlineData(null, NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatusOption.Yes, null, "/age/partner-age")]
    [InlineData(ReturnTo.CheckAnswers, null, null, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, null, null, null, "/children/check-childs-details")]
    public async Task Get(
        string? returnTo,
        NationalityOption? nationality,
        SettledStatusOption? settledStatus,
        NationalityOption? partnerNationality,
        string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Nationality = nationality,
            SettledStatus = settledStatus,
            PartnerNationality = partnerNationality,
        });

        var url = $"{Url}?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertRadioButtonCount(4)
            .AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, PartnerPaidWorkOption.Yes, false, null, null, "/work-status/work-status-partner")]
    [InlineData(null, PartnerPaidWorkOption.No, false, null, null, "/Partner/PartnerBenefits")]
    [InlineData(null, PartnerPaidWorkOption.ParentalLeave, true, null, null, "/leave/parental-leave-partner")]
    [InlineData(null, PartnerPaidWorkOption.SickLeave, false, null, null, "/work-status/work-status-partner")]
    [InlineData(ReturnTo.CheckAnswers, PartnerPaidWorkOption.Yes, false, null, null, "/work-status/work-status-partner")]
    [InlineData(ReturnTo.CheckAnswers, PartnerPaidWorkOption.Yes, false, WorkStatusOption.PaidEmployment, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, PartnerPaidWorkOption.No, false, null, null, "/Partner/PartnerBenefits")]
    [InlineData(ReturnTo.CheckAnswers, PartnerPaidWorkOption.No, false, null, PartnerBenefitsOption.CarersAllowance, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, PartnerPaidWorkOption.ParentalLeave, false, null, null, "/leave/parental-leave-partner")]
    [InlineData(ReturnTo.CheckAnswers, PartnerPaidWorkOption.ParentalLeave, true, null, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, PartnerPaidWorkOption.SickLeave, false, null, null, "/work-status/work-status-partner")]
    [InlineData(ReturnTo.CheckAnswers, PartnerPaidWorkOption.SickLeave, false, WorkStatusOption.PaidEmployment, null, "/check-your-answers")]
    public async Task Post_Valid_Redirects(
        string? returnTo,
        PartnerPaidWorkOption partnerPaidWork,
        bool hasAnsweredParentalLeaveChildren,
        WorkStatusOption? partnerWorkStatus,
        PartnerBenefitsOption? partnerBenefits,
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
            PartnerPaidWork = partnerPaidWork,
            PartnerParentalLeaveChildrenIds = hasAnsweredParentalLeaveChildren ? [ChildId] : [],
            PartnerWorkStatus = partnerWorkStatus is null ? new() : [partnerWorkStatus.Value],
            PartnerBenefits = partnerBenefits is null ? new() : [partnerBenefits.Value],
        });
        var url = $"{Url}?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
            new KeyValuePair<string, string>("PartnerPaidWork", partnerPaidWork.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, null, null, null, "/nationality/nationality-partner")]
    [InlineData(null, null, null, NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, "/nationality/settled-status-partner")]
    [InlineData(null, NationalityOption.BritishOrIrishCitizen, null, null, "/age/partner-age")]
    [InlineData(null, NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatusOption.Yes, null, "/age/partner-age")]
    [InlineData(ReturnTo.CheckAnswers, null, null, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, null, null, null, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(
        string? returnTo,
        NationalityOption? nationality,
        SettledStatusOption? settledStatus,
        NationalityOption? partnerNationality,
        string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Nationality = nationality,
            SettledStatus = settledStatus,
            PartnerNationality = partnerNationality,
        });

        var url = $"{Url}?returnTo={returnTo}";
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
