using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Services;
using System.Net;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class ChildBirthDateTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task Post_With_Tomorrows_Date_Fails_Validation_And_Preserves_Childs_Name()
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

        var page = await client.GetPageAsync("/BornChildDetails/ChildBirthDate?childId=9fbb8965-c988-4199-8b40-189efcfe2a1e", TestContext.Current.CancellationToken);
        var tomorrow = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var submitted = await client.SubmitPageAsync(
            page,
            [
                new KeyValuePair<string, string>("ChildBirthDate.Day", tomorrow.Day.ToString()),
                new KeyValuePair<string, string>("ChildBirthDate.Month", tomorrow.Month.ToString()),
                new KeyValuePair<string, string>("ChildBirthDate.Year", tomorrow.Year.ToString())
            ]);

        submitted.EnsureSuccessStatusCode();
        var validationFailedPage = await submitted.ReadContentAsPageAsync();
        Assert.Equal("What is Sara's date of birth?", validationFailedPage.H1);
    }
}
