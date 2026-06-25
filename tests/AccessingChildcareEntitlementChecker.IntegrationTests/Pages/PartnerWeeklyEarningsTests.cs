using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class PartnerWeeklyEarningsTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Theory]
    [InlineData(null, null, "/work-status/work-status-partner")]
    [InlineData(null, WorkStatusOption.SelfEmployed, "/work-status/self-employed-partner")]
    [InlineData(ReturnTo.CheckAnswers, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, null, "/children/check-childs-details")]
    public async Task Get_Has_Input_And_BackLink(
        string? returnTo,
        WorkStatusOption? partnerWorkStatus,
        string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            PartnerWorkStatus = partnerWorkStatus.HasValue ? [partnerWorkStatus.Value] : [],
        });

        var url = $"/earnings/wage-partner?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertBackLink(backLinkUrl);
    }

    [Theory]
    [InlineData(null, null, "/work-status/work-status-partner")]
    [InlineData(null, WorkStatusOption.SelfEmployed, "/work-status/self-employed-partner")]
    [InlineData(ReturnTo.CheckAnswers, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, null, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(
        string? returnTo,
        WorkStatusOption? partnerWorkStatus,
        string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            PartnerWorkStatus = partnerWorkStatus.HasValue ? [partnerWorkStatus.Value] : [],
        });

        var url = $"/earnings/wage-partner?returnTo={returnTo}";
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
