using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class ResultsSummaryTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{

    [Fact]
    public async Task Get_Results_Has_BackLink()
    {
        var state = new JourneyState();
        using var client = factory.CreateClientWithJourneyState(state);
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        var backLink = doc.QuerySelector(".govuk-back-link");

        Assert.NotNull(backLink);
    }

    [Fact]
    public async Task Get_Results_Has_Two_Print_Buttons()
    {
        var state = new JourneyState();
        state.CountryOfResidence = CountryOfResidence.England;
        state.Children["child-1"] = new Child("child-1", "Jack")
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3)),
        };
        using var client = factory.CreateClientWithJourneyState(state);
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        var printButtons = doc.QuerySelectorAll(".app-print-link");

        Assert.Equal(2, printButtons.Length);
    }


    [Fact]
    public async Task Get_Results_ReturnsView()
    {
        var state = new JourneyState();
        state.CountryOfResidence = CountryOfResidence.England;
        state.Children["child-1"] = new Child("child-1", "Jack")
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3)),
        };
        using var client = factory.CreateClientWithJourneyState(state);
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        var heading = doc.QuerySelector("h1");

        Assert.NotNull(heading);
        Assert.Contains("Childcare support you could get", heading.TextContent.Trim());
    }

    [Fact]
    public async Task Get_Results_Displays_FifteenHoursUniversal_For_EligibleChild()
    {
        var state = new JourneyState();
        state.CountryOfResidence = CountryOfResidence.England;
        state.Children["child-1"] = new Child("child-1", "Jack")
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3)),
        };
        using var client = factory.CreateClientWithJourneyState(state);
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        Assert.Contains("Jack", doc.Body?.TextContent ?? string.Empty);
        Assert.Contains("15 hours free childcare for 3 and 4-year-olds", doc.Body?.TextContent);
    }

    [Fact]
    public async Task Get_Results_DisplaysFifteenHoursUniversalAndFCFWP()
    {
        var state = new JourneyState();
        state.CountryOfResidence = CountryOfResidence.England;
        state.WeeklyEarnings = WeeklyEarningsOption.AboveThreshold;
        state.Nationality = NationalityOption.BritishOrIrishCitizen;
        state.PaidWork = PaidWorkOption.Yes;
        state.YearlyEarnings = YearlyEarningsOption.BelowThreshold;
        state.HasPartner = false;
        state.Children["child-1"] = new Child("child-1", "Jack")
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3)),
        };

        using var client = factory.CreateClientWithJourneyState(state);
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        Assert.Contains("Jack", doc.Body?.TextContent ?? string.Empty);
        Assert.Contains("15 hours free childcare for 3 and 4-year-olds", doc.Body?.TextContent);
        Assert.Contains("Free Childcare for Working Parents", doc.Body?.TextContent);
    }

    [Fact]
    public async Task Get_Results_DisplaysThirtyHourWarning()
    {
        var state = new JourneyState();

        state.CountryOfResidence = CountryOfResidence.England;
        state.WeeklyEarnings = WeeklyEarningsOption.AboveThreshold;
        state.Nationality = NationalityOption.BritishOrIrishCitizen;
        state.PaidWork = PaidWorkOption.Yes;
        state.YearlyEarnings = YearlyEarningsOption.BelowThreshold;
        state.HasPartner = false;
        state.Children["child-1"] = new Child("child-1", "Jack")
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3)),
        };

        using var client = factory.CreateClientWithJourneyState(state);
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        Assert.Contains("Jack", doc.Body?.TextContent ?? string.Empty);
        Assert.Contains("You can only get 30 hours of childcare a week in total, even if you use more than one scheme.",
            doc.Body?.TextContent);
    }

    [Theory]
    [InlineData(NationalityOption.BritishOrIrishCitizen, null, false)]
    [InlineData(NationalityOption.CitizenOfADifferentCountry, null, true)]
    [InlineData(NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatusOption.Yes, false)]
    [InlineData(NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatusOption.No, true)]
    [InlineData(NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatusOption.StillWaiting, false)]
    public async Task Get_Results_DisplaysPublicFundsWarning(NationalityOption nationality, SettledStatusOption? settledStatus, bool hasWarning)
    {
        var state = new JourneyState();
        state.CountryOfResidence = CountryOfResidence.England;
        state.Children["child-1"] = new Child("child-1", "Jack")
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3)),
        };
        state.Nationality = nationality;
        state.SettledStatus = settledStatus;
        state.PaidWork = PaidWorkOption.No;
        state.HasPartner = false;

        using var client = factory.CreateClientWithJourneyState(state);
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        const string WarningText = "You need to check if you can access public funds";
        if (hasWarning)
        {
            Assert.Contains(WarningText, doc.Body?.TextContent);
        }
        else
        {
            Assert.DoesNotContain(WarningText, doc.Body?.TextContent);
        }
    }

    [Fact]
    public async Task Get_Results_Displays_With_Mixed_Eligibility()
    {
        var state = new JourneyState();
        state.CountryOfResidence = CountryOfResidence.England;
        state.Children["child-1"] = new Child("child-1", "CHILD-1")
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-3)),
        };

        state.Children["child-2"] = new Child("child-2", "CHILD-2")
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-25)),
        };

        using var client = factory.CreateClientWithJourneyState(state);
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        doc.AssertResultsSection("CHILD-1")
            .AssertContainsText("This is a summary of CHILD-1's childcare support.");

        doc.AssertResultsSection("CHILD-2")
            .AssertContainsText("You cannot currently get any of the childcare support this service checks for CHILD-2.");
    }

    [Fact]
    public async Task Get_Results_Displays_With_No_Eligibility()
    {
        var state = new JourneyState();
        state.CountryOfResidence = CountryOfResidence.England;
        state.Children["child-1"] = new Child("child-1", "CHILD-1")
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-25)),
        };

        state.Children["child-2"] = new Child("child-2", "CHILD-2")
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-25)),
        };

        using var client = factory.CreateClientWithJourneyState(state);
        var response = await client.GetAsync("/results", TestContext.Current.CancellationToken);
        response.EnsureSuccessStatusCode();
        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        doc.AssertHeader("You are not currently eligible for childcare support");
    }
}
