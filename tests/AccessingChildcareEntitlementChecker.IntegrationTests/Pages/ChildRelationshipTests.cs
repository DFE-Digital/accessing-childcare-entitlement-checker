using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class ChildRelationshipTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string ChildId = "9fbb8965-c988-4199-8b40-189efcfe2a1e";

    [Theory]
    [InlineData(null, $"/children/{ChildId}/childs-date-of-birth")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Get_Has_Input_And_BackLink(string? returnTo, string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        ChildId,
                        new Child(ChildId, "Sara")
                    }
                }
        });

        var url = $"/children/{ChildId}/relationship-to-child?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertRadioButtonCount(3)
            .AssertBackLink(backLinkUrl);
    }

    [Theory]
    [InlineData(null, null, $"/children/{ChildId}/child-benefits")]
    [InlineData(ReturnTo.CheckAnswers, null, $"/children/{ChildId}/child-benefits")]
    [InlineData(ReturnTo.CheckChildDetails, null, $"/children/{ChildId}/child-benefits")]
    [InlineData(ReturnTo.CheckAnswers, ChildSupport.EducationHealthAndCarePlan, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, ChildSupport.EducationHealthAndCarePlan, "/children/check-childs-details")]
    public async Task Post_Valid_Redirects(string? returnTo, ChildSupport? childSupport, string continueUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        ChildId,
                        new Child(ChildId, "Sara")
                        {
                            ChildSupportOptions = childSupport == null ? [] : [childSupport.Value]
                        }
                    }
                }
        });

        var url = $"/children/{ChildId}/relationship-to-child?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
                new KeyValuePair<string, string>("Relationship", "Parent")],
            TestContext.Current.CancellationToken);
        postResponse.AssertRedirect(continueUrl);
    }

    [Theory]
    [InlineData(null, $"/children/{ChildId}/childs-date-of-birth")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Post_Invalid_Fails_Validation_And_Preserves_Childs_Name_With_BackLink(string? returnTo, string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        ChildId,
                        new Child(ChildId, "Sara")
                    }
                }
        });

        var url = $"/children/{ChildId}/relationship-to-child?returnTo={returnTo}";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var tomorrow = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [], TestContext.Current.CancellationToken);
        var postDocument = await HtmlHelpers.ParseHtmlAsync(postResponse.Content);
        postDocument.AssertHeader("What is your relationship to Sara?")
                    .AssertValidationError()
                    .AssertBackLink(backLinkUrl);
    }

    [Fact]
    public async Task Returns_Not_Found_For_Nonexistant_Child()
    {
        using var client = factory.CreateClient();
        var url = $"/children/{ChildId}/relationship-to-child";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}
