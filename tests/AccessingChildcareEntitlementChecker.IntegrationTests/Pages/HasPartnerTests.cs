using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class HasPartnerTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/partner";

    [Fact]
    public async Task Get_HasPartner_Has_Radios_And_BackLink_Defaults_To_ChildcareSupport_Back()
    {
        using var host = factory.CreateClientWithJourneyState(new JourneyState());

        using var client = host.CreateClient();

        var response = await client.GetAsync(Url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertRadioButtonCount(2)
            .AssertBackLink("/benefits/childcare-support")
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Fact]
    public async Task Get_HasPartner_BackLink_When_Vouchers_Points_To_ChildcareVoucherReceipt()
    {
        var state = new JourneyState();
        state.ChildcareSupport.Add(ChildcareSupportOption.ChildcareVouchers);
        using var host = factory.CreateClientWithJourneyState(state);

        using var client = host.CreateClient();

        var response = await client.GetAsync(Url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertBackLink("/benefits/childcare-vouchers")
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, true, null, "/age/partner-age")]
    [InlineData(ReturnTo.CheckAnswers, true, null, "/age/partner-age")]
    [InlineData(ReturnTo.CheckAnswers, true, AgeRange.UnderEighteen, "/age/partner-age")]
    [InlineData(null, false, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckAnswers, false, null, "/check-your-answers")]
    public async Task Post_Valid_Redirects(string? returnTo, bool hasPartner, AgeRange? partnerAge, string continueUrl)
    {
        using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            HasPartner = hasPartner,
            PartnerAge = partnerAge,
        });

        using var client = host.CreateClient();
        var url = $"{Url}?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
            new KeyValuePair<string, string>("HasPartner", hasPartner.ToString())
        ], TestContext.Current.CancellationToken);

        postResponse.AssertRedirect(continueUrl);
    }
}
