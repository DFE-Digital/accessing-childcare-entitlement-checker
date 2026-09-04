using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class CheckYourAnswersTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    private const string Url = "/check-your-answers";

    [Fact]
    public async Task Get_WhenFeatureFlagEnabled_SuppressesLocationRow()
    {
        using var client = factory.CreateClientWithJourneyStateAndFeatureFlags(new JourneyState
        {
            CountryOfResidence = CountryOfResidence.England,
            HasPartner = false,
        }, new()
        {
            { "FeatureManagement:HmrcIntegration", "true" }
        });

        var response = await client.GetAsync(Url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        Assert.DoesNotContain("Where do you live?", content);
    }

    [Theory]
    [InlineData(false, null, "/partner")]
    [InlineData(true, null, "/benefits/childcare-support-partner")]
    [InlineData(true, PartnerChildcareSupportOption.ChildcareVouchers, "/benefits/childcare-vouchers-partner")]
    public async Task Get(
        bool? hasPartner,
        PartnerChildcareSupportOption? partnerChildcareSupport,
        string backLinkUrl)
    {
        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            HasPartner = hasPartner,
            PartnerChildcareSupport = partnerChildcareSupport.HasValue ? [partnerChildcareSupport.Value] : [],
        });

        var response = await client.GetAsync(Url, TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertBackLink(backLinkUrl)
            .AssertNavigationBar()
            .AssertBetaBanner();
    }
    
    [Fact]
    public async Task GetParentalLeaveChildNameMaskedForClarity()
    {
        const string childId = "child-1";

        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            CountryOfResidence = CountryOfResidence.England,
            HasPartner = false,
            PaidWork = PaidWorkOption.ParentalLeave,

            Children = new Dictionary<string, Child>
            
            {
                {
                    childId,
                    new Child(childId, "Sara")
                    {
                        BirthStatus = BirthStatus.Born,
                        BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3)),
                        ChildSupportOptions = [ChildSupport.NoneOfTheseApply]
                        
                    }
                }
            },

            ParentalLeaveChildrenIds = [childId]
        });

        var response = await client.GetAsync(Url, TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        var parentalLeaveValue = doc.QuerySelector("[data-testid=\"parental-leave-child-names\"]");

        Assert.NotNull(parentalLeaveValue);
        Assert.Equal("true", parentalLeaveValue.GetAttribute("data-clarity-mask"));
        Assert.Equal("Sara", parentalLeaveValue.TextContent.Trim());
    }
    
    [Theory]
    [InlineData("check-your-answers")]
    public async Task Get_RemovePageTitleMaskedForClarity(string returnTo)
    {
        const string childId = "child-1";

        using var client = factory.CreateClientWithJourneyState(new JourneyState
        {
            Children = new Dictionary<string, Child>
            {
                {
                    childId,
                    new Child(childId, "Sara")
                }
            }
        });

        var url = $"/children/{childId}/remove?returnTo={returnTo}";

        var response = await client.GetAsync(url, TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var document = await HtmlHelpers.ParseHtmlAsync(response.Content);

        var legend = document.QuerySelector("legend.govuk-fieldset__legend");

        Assert.NotNull(legend);
        Assert.Equal("true", legend.GetAttribute("data-clarity-mask"));
        Assert.Contains("Sara", legend.TextContent);
    }
}
