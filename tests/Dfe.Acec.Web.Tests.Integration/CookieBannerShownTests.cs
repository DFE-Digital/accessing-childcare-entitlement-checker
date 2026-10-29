using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.BornChildDetails;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Tests.Integration.Fixtures;
using Dfe.Acec.Web.Tests.Integration.Helpers;

namespace Dfe.Acec.Web.Tests.Integration;

public class CookieBannerShownTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{

    [Theory]
    [InlineData("cookie_policy=enabled", false,true)] //Consent given: No banner, Include GTM code
    [InlineData("cookie_policy=disabled", false,false)] // Consent NOT given, No banner, DON'T include GTM code
    [InlineData(null, true,false)] // no cookie set, show the banner,  DON'T include GTM code
    public async Task CookieControlsBannerAndGtmScript(string? cookieHeader, bool expectBanner, bool expectGtm)
    {
        var sessionRequiredEndpoints = GetCookieBannerRequiredEndpoints();
        var children = new Dictionary<string, Child>
               {
                   {
                       "1",
                       new Child("1", "Child 1")
                       {
                           BirthStatus = BirthStatus.Born,
                           BirthDate = new DateOnly(2020, 1, 1),
                           ChildSupportOptions = [ChildSupport.NoneOfTheseApply]
                       }
                   }
               };

        foreach (var url in sessionRequiredEndpoints)
        {
            await using var getHost = factory.CreateClientWithJourneyState(new JourneyState
            {
                Children = children,
                UserAge = AgeRange.UnderEighteen,
                WorkStatus = [WorkStatusOption.PaidEmployment],
                PartnerAge = AgeRange.UnderEighteen,
                PartnerWorkStatus = [WorkStatusOption.PaidEmployment],
            });

            using var getClient = getHost.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            if (!string.IsNullOrWhiteSpace(cookieHeader))
            {
                request.Headers.Add("Cookie", cookieHeader);
            }

            var getResponse = await getClient.SendAsync(request, TestContext.Current.CancellationToken);
            var document = await HtmlHelpers.ParseHtmlAsync(getResponse.Content);

            if (expectBanner)
            {
                document.AssertCookieBanner();
            }
            else
            {
                document.AssertNoCookieBanner();
            }

            if(expectGtm)
            {
                document.AssertGtmTagCodePresent();
            }
            else
            {
                document.AssertNoGtmTagCode();
            }
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
