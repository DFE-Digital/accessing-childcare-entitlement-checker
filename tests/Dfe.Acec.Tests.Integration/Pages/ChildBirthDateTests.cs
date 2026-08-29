using Dfe.Acec.Tests.Integration.Fixtures;
using Dfe.Acec.Tests.Integration.Helpers;
using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.BornChildDetails;
using Dfe.Acec.Web.Services;

namespace Dfe.Acec.Tests.Integration.Pages;

public class ChildBirthDateTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string ChildId = "9fbb8965-c988-4199-8b40-189efcfe2a1e";
    private const string Url = $"/children/{ChildId}/childs-date-of-birth";

    [Theory]
    [InlineData(null, $"/children/{ChildId}/has-the-child-been-born")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task Get(string? returnTo, string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        ChildId,
                        new Child(ChildId, "Sara")
                    }
                }
        });

        using var client = host.CreateClient();

        var url = $"{Url}?returnTo={returnTo}";
        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertDateInput()
            .AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(ReturnTo.CheckAnswers, null)]
    [InlineData(ReturnTo.CheckChildDetails, null)]
    [InlineData(ReturnTo.CheckAnswers, ChildSupport.NoneOfTheseApply)]
    [InlineData(ReturnTo.CheckChildDetails, ChildSupport.NoneOfTheseApply)]
    public async Task PostValidRedirects(string? returnTo, ChildSupport? childSupport)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        ChildId,
                        new Child(ChildId, "Sara")
                        {
                            ChildSupportOptions = childSupport == null ? [] : [childSupport.Value],
                        }
                    }
                }
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

        var yesterday = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
                new KeyValuePair<string, string>("ChildBirthDate.Day", yesterday.Day.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("ChildBirthDate.Month", yesterday.Month.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("ChildBirthDate.Year", yesterday.Year.ToString(System.Globalization.CultureInfo.InvariantCulture))
            ],
            TestContext.Current.CancellationToken);
        postResponse.AssertRedirect($"/children/{ChildId}/child-benefits");
    }

    [Theory]
    [InlineData(null, $"/children/{ChildId}/has-the-child-been-born")]
    [InlineData(ReturnTo.CheckAnswers, "/check-your-answers")]
    [InlineData(ReturnTo.CheckChildDetails, "/children/check-childs-details")]
    public async Task PostWithTomorrowsDateFailsValidationAndPreservesChildsNameWithBackLink(string? returnTo, string backLinkUrl)
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        ChildId,
                        new Child(ChildId, "Sara")
                    }
                }
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

        var tomorrow = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [
                new KeyValuePair<string, string>("ChildBirthDate.Day", tomorrow.Day.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("ChildBirthDate.Month", tomorrow.Month.ToString(System.Globalization.CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("ChildBirthDate.Year", tomorrow.Year.ToString(System.Globalization.CultureInfo.InvariantCulture))
            ],
            TestContext.Current.CancellationToken);
        var postDocument = await HtmlHelpers.ParseHtmlAsync(postResponse.Content);
        postDocument.AssertHeading("What is Sara's date of birth?")
                    .AssertValidationError()
                    .AssertBackLink(backLinkUrl);
    }

    [Fact]
    public async Task ReturnsNotFoundForNonexistantChild()
    {
        await using var host = factory.CreateClientWithJourneyState(new JourneyState());

        using var client = host.CreateClient();
        var response = await client.GetAsync(Url, TestContext.Current.CancellationToken);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}
