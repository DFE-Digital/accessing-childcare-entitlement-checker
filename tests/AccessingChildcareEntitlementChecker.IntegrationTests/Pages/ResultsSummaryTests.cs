using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class ResultsSummaryTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{

    [Fact]
    public async Task GetResultsHasBackLink()
    {
        var state = new JourneyState();
        using var host = factory.CreateClientWithJourneyState(state);

        using var client = host.CreateClient();
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        var backLink = doc.QuerySelector(".govuk-back-link");

        Assert.NotNull(backLink);
    }

    [Fact]
    public async Task GetResultsHasNavBarAndBetaBanner()
    {
        var state = new JourneyState
        {
            CountryOfResidence = CountryOfResidence.England,
            Children =
            {
                ["child-1"] = new Child("child-1", "Jack")
                {
                    BirthStatus = BirthStatus.Born,
                    BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3)),
                }
            }
        };
        using var host = factory.CreateClientWithJourneyState(state);

        using var client = host.CreateClient();
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Fact]
    public async Task GetResultsHasTwoPrintButtons()
    {
        var state = new JourneyState
        {
            CountryOfResidence = CountryOfResidence.England,
            Children =
            {
                ["child-1"] = new Child("child-1", "Jack")
                {
                    BirthStatus = BirthStatus.Born,
                    BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3)),
                }
            }
        };
        using var host = factory.CreateClientWithJourneyState(state);

        using var client = host.CreateClient();
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        var printButtons = doc.QuerySelectorAll(".app-print-link");

        Assert.Equal(2, printButtons.Length);
    }


    [Fact]
    public async Task GetResultsReturnsView()
    {
        var state = new JourneyState
        {
            CountryOfResidence = CountryOfResidence.England,
            Children =
            {
                ["child-1"] = new Child("child-1", "Jack")
                {
                    BirthStatus = BirthStatus.Born,
                    BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3)),
                }
            }
        };
        using var host = factory.CreateClientWithJourneyState(state);

        using var client = host.CreateClient();
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        var heading = doc.QuerySelector("h1");

        Assert.NotNull(heading);
        Assert.Contains("Childcare support you could get", heading.TextContent.Trim());
    }

    [Fact]
    public async Task GetResultsDisplaysFifteenHoursUniversalForEligibleChild()
    {
        var state = new JourneyState
        {
            CountryOfResidence = CountryOfResidence.England,
            Children =
            {
                ["child-1"] = new Child("child-1", "Jack")
                {
                    BirthStatus = BirthStatus.Born,
                    BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3)),
                }
            }
        };
        using var host = factory.CreateClientWithJourneyState(state);

        using var client = host.CreateClient();
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        Assert.Contains("Jack", doc.Body?.TextContent ?? string.Empty);
        Assert.Contains("15 hours free childcare for 3 and 4-year-olds", doc.Body?.TextContent);
    }

    [Fact]
    public async Task GetResultsDisplaysFifteenHoursUniversalAndFcfwp()
    {
        var state = new JourneyState
        {
            CountryOfResidence = CountryOfResidence.England,
            WeeklyEarnings = WeeklyEarningsOption.AboveThreshold,
            Nationality = NationalityOption.BritishOrIrishCitizen,
            PaidWork = PaidWorkOption.Yes,
            YearlyEarnings = YearlyEarningsOption.BelowThreshold,
            HasPartner = false,
            Children =
            {
                ["child-1"] = new Child("child-1", "Jack")
                {
                    BirthStatus = BirthStatus.Born,
                    BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3)),
                }
            }
        };

        using var host = factory.CreateClientWithJourneyState(state);

        using var client = host.CreateClient();
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        Assert.Contains("Jack", doc.Body?.TextContent ?? string.Empty);
        Assert.Contains("15 hours free childcare for 3 and 4-year-olds", doc.Body?.TextContent);
        Assert.Contains("Free Childcare for Working Parents", doc.Body?.TextContent);
    }

    [Fact]
    public async Task GetResultsDisplaysThirtyHourWarning()
    {
        var state = new JourneyState
        {
            CountryOfResidence = CountryOfResidence.England,
            WeeklyEarnings = WeeklyEarningsOption.AboveThreshold,
            Nationality = NationalityOption.BritishOrIrishCitizen,
            PaidWork = PaidWorkOption.Yes,
            YearlyEarnings = YearlyEarningsOption.BelowThreshold,
            HasPartner = false,
            Children =
            {
                ["child-1"] = new Child("child-1", "Jack")
                {
                    BirthStatus = BirthStatus.Born,
                    BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3)),
                }
            }
        };

        using var host = factory.CreateClientWithJourneyState(state);

        using var client = host.CreateClient();
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        Assert.Contains("Jack", doc.Body?.TextContent ?? string.Empty);
        Assert.Contains("You can only get up to 30 hours of funded childcare per child each week, even if your child is eligible for more than one scheme.",
            doc.Body?.TextContent);
    }

    [Theory]
    [InlineData(NationalityOption.BritishOrIrishCitizen, null, false)]
    [InlineData(NationalityOption.CitizenOfADifferentCountry, null, true)]
    [InlineData(NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland, SettledStatusOption.Yes, false)]
    [InlineData(NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland, SettledStatusOption.No, true)]
    [InlineData(NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland, SettledStatusOption.StillWaiting, false)]
    public async Task GetResultsDisplaysPublicFundsWarning(NationalityOption nationality, SettledStatusOption? settledStatus, bool hasWarning)
    {
        var state = new JourneyState
        {
            CountryOfResidence = CountryOfResidence.England,
            Nationality = nationality,
            SettledStatus = settledStatus,
            PaidWork = PaidWorkOption.No,
            HasPartner = false,
            Children =
            {
                ["child-1"] = new Child("child-1", "Jack")
                {
                    BirthStatus = BirthStatus.Born,
                    BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3)),
                }
            }
        };

        using var host = factory.CreateClientWithJourneyState(state);

        using var client = host.CreateClient();
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        const string warningText = "You need to check if you can access public funds";
        if (hasWarning)
        {
            Assert.Contains(warningText, doc.Body?.TextContent);
        }
        else
        {
            Assert.DoesNotContain(warningText, doc.Body?.TextContent);
        }
    }

    [Fact]
    public async Task GetResultsDisplaysWithMixedEligibility()
    {
        var state = new JourneyState
        {
            CountryOfResidence = CountryOfResidence.England,
            Children =
            {
                ["child-1"] = new Child("child-1", "CHILD-1")
                {
                    BirthStatus = BirthStatus.Born,
                    BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3)),
                },
                ["child-2"] = new Child("child-2", "CHILD-2")
                {
                    BirthStatus = BirthStatus.Born,
                    BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-25)),
                }
            }
        };

        using var host = factory.CreateClientWithJourneyState(state);

        using var client = host.CreateClient();
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        doc.AssertResultsSection("CHILD-1")
            .AssertContainsText("This is a summary of CHILD-1's childcare support.");

        doc.AssertResultsSection("CHILD-2")
            .AssertContainsText("You cannot currently get any of the childcare support this service checks for CHILD-2.");
    }

    [Fact]
    public async Task GetResultsDisplaysWithNoEligibility()
    {
        var state = new JourneyState
        {
            CountryOfResidence = CountryOfResidence.England,
            Children =
            {
                ["child-1"] = new Child("child-1", "CHILD-1")
                {
                    BirthStatus = BirthStatus.Born,
                    BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-25)),
                },
                ["child-2"] = new Child("child-2", "CHILD-2")
                {
                    BirthStatus = BirthStatus.Born,
                    BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-25)),
                }
            }
        };

        using var host = factory.CreateClientWithJourneyState(state);

        using var client = host.CreateClient();
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertHeading("You are not currently eligible for childcare support");
    }
}
