using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class WeeklyEarningsTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Theory]
    [InlineData(null, WorkStatusOption.PaidEmployment, null, "/work-status/work-status")]
    [InlineData(null, WorkStatusOption.SelfEmployed, SelfEmployedDurationOption.NotLessThan12Months, "/work-status/self-employed")]
    [InlineData(null, WorkStatusOption.Apprentice, null, "/work-status/work-status")]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.PaidEmployment, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, WorkStatusOption.PaidEmployment, null, "/children/check-childs-details")]
    public async Task Get_Has_Input_And_BackLink(
        string? returnTo,
        WorkStatusOption? workStatus,
        SelfEmployedDurationOption? selfEmployedDuration,
        string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            WorkStatus = workStatus.HasValue ? [workStatus.Value] : [],
            SelfEmployedDuration = selfEmployedDuration,
        });

        var url = $"/earnings/wage?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertRadioButtonCount(2)
            .AssertBackLink(backLinkUrl);
    }

    [Theory]
    [InlineData(null, WorkStatusOption.PaidEmployment, null, "/work-status/work-status")]
    [InlineData(null, WorkStatusOption.SelfEmployed, SelfEmployedDurationOption.NotLessThan12Months, "/work-status/self-employed")]
    [InlineData(null, WorkStatusOption.Apprentice, null, "/work-status/work-status")]
    [InlineData(ReturnTo.CheckAnswers, WorkStatusOption.PaidEmployment, null, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, WorkStatusOption.PaidEmployment, null, "/children/check-childs-details")]
    public async Task Post_Invalid_Shows_Validation_Error(
        string? returnTo,
        WorkStatusOption? workStatus,
        SelfEmployedDurationOption? selfEmployedDuration,
        string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            WorkStatus = workStatus.HasValue ? [workStatus.Value] : [],
            SelfEmployedDuration = selfEmployedDuration,
        });

        var url = $"/earnings/wage?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(
            client,
            url,
            cookie,
            token,
            [],
            TestContext.Current.CancellationToken);
        var postDocument = await HtmlHelpers.ParseHtmlAsync(postResponse.Content);
        postDocument.AssertValidationError()
            .AssertBackLink(backLinkUrl);
    }
}
