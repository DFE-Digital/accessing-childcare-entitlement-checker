using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests;

public partial class CookieBannerShownTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{
    [Fact]
    public async Task GetWithoutCookieShowsBanner()
    {
        var sessionRequiredEndpoints = GetCookieBannerRequiredEndpoints();
        var children = new Dictionary<string, Child>
               {
                   {
                       "1",
                       new Child("1", "Child 1")
                   }
               };

        foreach (var url in sessionRequiredEndpoints)
        {
            using var getClient = factory.CreateClientWithJourneyState(new JourneyState
            {
                Children = children,
                UserAge = AgeRange.UnderEighteen,
                WorkStatus = [WorkStatusOption.PaidEmployment],
                PartnerAge = AgeRange.UnderEighteen,
                PartnerWorkStatus = [WorkStatusOption.PaidEmployment],
            });

            var getResponse = await getClient.GetAsync(url, TestContext.Current.CancellationToken);
            var document = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
            document.AssertCookieBanner();
        }
    }

    [Fact]
    public async Task GetWithCookieDoesNotShowBanner()
    {
        var sessionRequiredEndpoints = GetCookieBannerRequiredEndpoints();
        var children = new Dictionary<string, Child>
               {
                   {
                       "1",
                       new Child("1", "Child 1")
                   }
               };

        foreach (var url in sessionRequiredEndpoints)
        {
            using var getClient = factory.CreateClientWithJourneyState(new JourneyState
            {
                Children = children,
                UserAge = AgeRange.UnderEighteen,
                WorkStatus = [WorkStatusOption.PaidEmployment],
                PartnerAge = AgeRange.UnderEighteen,
                PartnerWorkStatus = [WorkStatusOption.PaidEmployment],
            });

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Cookie", "cookie_policy=enabled");
            var getResponse = await getClient.SendAsync(request, TestContext.Current.CancellationToken);
            var document = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);
            document.AssertNoCookieBanner();
        }
    }

    private IEnumerable<string> GetCookieBannerRequiredEndpoints()
    {
        var cookielessRoutes = new[]
        {
            "/throw",
            "/robots.txt",
        };

        return RouteHelper.GetEndpointsExcept(factory, "GET", cookielessRoutes);
    }
}
