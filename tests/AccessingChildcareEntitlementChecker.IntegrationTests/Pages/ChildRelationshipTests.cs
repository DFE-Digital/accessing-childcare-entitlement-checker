using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Services;
using System.Net;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class ChildRelationshipTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task Post_With_None_Fails_Validation_And_Preserves_Child_Name()
    {
        var client = factory.CreateClientWithJourneyState(new JourneyState
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

        var page = await client.GetPageAsync("/BornChildDetails/ChildRelationship?childId=9fbb8965-c988-4199-8b40-189efcfe2a1e", TestContext.Current.CancellationToken);
        var submitted = await client.SubmitPageAsync(page, []);

        submitted.EnsureSuccessStatusCode();
        var validationFailedPage = await submitted.ReadContentAsPageAsync();
        Assert.Equal("What is your relationship to Sara?", validationFailedPage.H1);
    }
}
