using AccessingChildcareEntitlementChecker.IntegrationTests.Fixtures;
using AccessingChildcareEntitlementChecker.IntegrationTests.Helpers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;

namespace AccessingChildcareEntitlementChecker.IntegrationTests.Pages;

public class ResultsDetailsTests(IntegrationTestFixture factory) : IClassFixture<IntegrationTestFixture>
{

    private static JourneyState CreateJourneyState()
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

        return state;
    }

    [Fact]
    public async Task Get_ResultsDetails_HasNavBarAndBetaBanner()
    {
        using var client = factory.CreateClientWithJourneyState(CreateJourneyState());

        var response = await client.GetAsync($"/Results/ResultsDetailed?childId=child-1",
            TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        var backLink = doc
            .AssertNavigationBar()
            .AssertBetaBanner();
    }

    [Fact]
    public async Task Get_ResultsDetailed_Has_BackLink()
    {
        using var client = factory.CreateClientWithJourneyState(CreateJourneyState());

        var response = await client.GetAsync($"/Results/ResultsDetailed?childId=child-1",
            TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        var backLink = doc.QuerySelector(".govuk-back-link");

        Assert.NotNull(backLink);
    }

    [Fact]
    public async Task Get_ResultsDetailed_Has_PrintButton()
    {
        using var client = factory.CreateClientWithJourneyState(CreateJourneyState());

        var response = await client.GetAsync($"/Results/ResultsDetailed?childId=child-1",
            TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        var printButtons = doc.QuerySelectorAll(".app-print-link");

        Assert.Equal(2, printButtons.Length);
    }

    [Fact]
    public async Task Get_ResultsDetailed_ReturnsView()
    {
        using var client = factory.CreateClientWithJourneyState(CreateJourneyState());

        var response = await client.GetAsync("/Results/ResultsDetailed?childId=child-1",
            TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);
        var heading = doc.QuerySelector("h1");

        Assert.NotNull(heading);
        Assert.Contains("Jack's childcare support", heading.TextContent.Trim());
    }


    [Fact]
    public async Task Get_ResultsDetailed_DisplaysTaxFreeChildcareDescription()
    {
        using var client = factory.CreateClientWithJourneyState(CreateJourneyState());

        var response = await client.GetAsync("/Results/ResultsDetailed?childId=child-1",
            TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        Assert.Contains(
            "Working families can get help with childcare costs for children aged 0 to 11 (or 16 if they have a disability).",
            doc.Body?.TextContent);
        Assert.Contains(
            "You can get up to £2,000 a year towards these costs. If your child has a disability, this increases to £4,000 a year.",
            doc.Body?.TextContent);
    }

    [Fact]
    public async Task Get_ResultsDetailed_DisplaysThirtyHoursDescription()
    {
        using var client = factory.CreateClientWithJourneyState(CreateJourneyState());

        var response = await client.GetAsync("/Results/ResultsDetailed?childId=child-1",
            TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        Assert.Contains(
            "Working parents can get up to 30 hours of funded childcare a week for children aged 9 months to 4 years old.",
            doc.Body?.TextContent);
        Assert.Contains(
            "You can only get up to 30 hours of funded childcare per child each week, even if your child is eligible for more than one scheme.",
            doc.Body?.TextContent);
    }

    [Fact]
    public async Task Get_ResultsDetailed_DisplaysCanBeUsedWithRow()
    {
        using var client = factory.CreateClientWithJourneyState(CreateJourneyState());

        var response = await client.GetAsync("/Results/ResultsDetailed?childId=child-1",
            TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        Assert.Contains("Can be used with", doc.Body?.TextContent);
    }

    [Fact]
    public async Task Get_ResultsDetailed_DisplaysCompatibleSchemes()
    {
        using var client = factory.CreateClientWithJourneyState(CreateJourneyState());

        var response = await client.GetAsync("/Results/ResultsDetailed?childId=child-1",
            TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        Assert.Contains("Free Childcare for Working Parents", doc.Body?.TextContent);
        Assert.Contains("15 hours free childcare for 3 and 4-year-olds", doc.Body?.TextContent);
    }

    [Fact]
    public async Task Get_ResultsDetailed_DoesNotDisplayIncompatibleSchemes()
    {
        using var client = factory.CreateClientWithJourneyState(CreateJourneyState());

        var response = await client.GetAsync("/Results/ResultsDetailed?childId=child-1",
            TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        Assert.DoesNotContain("Universal Credit and childcare", doc.Body?.TextContent);
    }

    [Fact]
    public async Task Get_ResultsDetailed_DisplaysThirtyHourWarning()
    {
        using var client = factory.CreateClientWithJourneyState(CreateJourneyState());

        var response = await client.GetAsync("/Results/ResultsDetailed?childId=child-1",
            TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var doc = await HtmlHelpers.ParseHtmlAsync(response.Content);

        Assert.Contains(
            "You can only get up to 30 hours of funded childcare per child each week, even if your child is eligible for more than one scheme.",
            doc.Body?.TextContent);
    }

    [Theory]
    [InlineData(NationalityOption.BritishOrIrishCitizen, null, false)]
    [InlineData(NationalityOption.CitizenOfADifferentCountry, null, true)]
    [InlineData(NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatusOption.Yes, false)]
    [InlineData(NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatusOption.No, true)]
    [InlineData(NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatusOption.StillWaiting, false)]
    public async Task Get_ResultsDetailed_DisplaysPublicFundsWarning(NationalityOption nationality,
        SettledStatusOption? settledStatus, bool hasWarning)
    {
        var state = CreateJourneyState();
        state.Nationality = nationality;
        state.SettledStatus = settledStatus;
        using var client = factory.CreateClientWithJourneyState(state);

        var response = await client.GetAsync("/Results/ResultsDetailed?childId=child-1",
            TestContext.Current.CancellationToken);

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
    public async Task Get_SensitiveElementsMaskedForClarity()
    {
        using var client = factory.CreateClientWithJourneyState(CreateJourneyState());

        var response = await client.GetAsync("/Results/ResultsDetailed?childId=child-1", TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();

        var document = await HtmlHelpers.ParseHtmlAsync(response.Content);

        var heading = document.QuerySelector("h1.govuk-heading-xl");
        var intro = document.QuerySelector("p.govuk-body[data-clarity-mask=\"true\"]");
        var whenToApply = document.QuerySelector("#when-to-apply");
        var starts = document.QuerySelector("#starts");
        var ends = document.QuerySelector("#ends");

        Assert.NotNull(heading);
        Assert.Equal("true", heading.GetAttribute("data-clarity-mask"));

        Assert.NotNull(intro);
        Assert.Equal("true", intro.GetAttribute("data-clarity-mask"));

        Assert.NotNull(whenToApply);
        Assert.Equal("true", whenToApply.GetAttribute("data-clarity-mask"));

        Assert.NotNull(starts);
        Assert.Equal("true", starts.GetAttribute("data-clarity-mask"));

        Assert.NotNull(ends);
        Assert.Equal("true", ends.GetAttribute("data-clarity-mask"));
    }
}
