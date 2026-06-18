using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class ChildRelationshipTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task Post_With_None_Fails_Validation_And_Preserves_Child_Name()
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
                {
                    {
                        "9fbb8965-c988-4199-8b40-189efcfe2a1e",
                        new Child("9fbb8965-c988-4199-8b40-189efcfe2a1e", "Sara")
                        {
                            BirthDate = new DateOnly(2020, 1, 1)
                        }
                    }
                }
        });

        var url = "/BornChildDetails/ChildRelationship?childId=9fbb8965-c988-4199-8b40-189efcfe2a1e";
        var getResponse = await client.GetAsync(url, TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var getDocument = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
        var token = HtmlHelpers.ExtractAntiforgeryToken(getDocument);
        var cookie = HtmlHelpers.ExtractAntiforgeryCookie(getResponse);
        Assert.NotNull(token);
        Assert.NotNull(cookie);

        var postResponse = await HttpClientHelpers.PostFormAsync(client, url, cookie, token, [], TestContext.Current.CancellationToken);
        var postDocument = await HtmlHelpers.ParseHtmlAsync(postResponse.Content);
        postDocument.AssertHeader("What is your relationship to Sara?");
    }
}
