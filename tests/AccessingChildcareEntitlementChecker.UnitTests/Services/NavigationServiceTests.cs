using System;
using System.Collections.Generic;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation.Steps;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using Xunit;

namespace AccessingChildcareEntitlementChecker.UnitTests.Services;

public class NavigationServiceTests
{
    private readonly JourneyState _journeyState;
    private readonly List<IWorkflowStep> _steps;
    private readonly TestNavigationService _navigationService;

    public NavigationServiceTests()
    {
        _journeyState = new JourneyState();

        _steps = [
            new HomeWorkflowSteps(),
            new IntroductionWorkflowSteps(),
            new BornChildDetailsWorkflowSteps(),
            new ExpectedChildDetailsWorkflowSteps(),
            new SummaryWorkflowSteps(),
            new UserWorkflowSteps(),
            new PartnerWorkflowSteps()
        ];

        _navigationService = new TestNavigationService(_steps, _journeyState);
    }

    private class TestNavigationService : NavigationService
    {
        public TestNavigationService(IEnumerable<IWorkflowStep> steps, JourneyState journeyState)
            : base(steps, journeyState, null!, null!)
        {
        }

        protected override string GetUrl(Page page, string? returnTo = null, string? childId = null)
        {
            var url = $"/{page}";
            if (!string.IsNullOrEmpty(childId) && page is not Page.CheckAnswers && page is not Page.CheckChildDetails)
            {
                url += $"?childId={childId}";
            }
            else if (!string.IsNullOrEmpty(childId) && (page is Page.CheckAnswers || page is Page.CheckChildDetails))
            {
                url += $"?fromChildId={childId}";
            }

            if (!string.IsNullOrEmpty(returnTo) && page is not Page.CheckAnswers && page is not Page.CheckChildDetails)
            {
                url += (url.Contains('?') ? "&" : "?") + $"returnTo={returnTo}";
            }
            return url;
        }
    }

    private class RealNavigationServiceWrapper : NavigationService
    {
        public RealNavigationServiceWrapper(
            IEnumerable<IWorkflowStep> steps,
            JourneyState journeyState,
            LinkGenerator linkGenerator,
            IHttpContextAccessor httpContextAccessor)
            : base(steps, journeyState, linkGenerator, httpContextAccessor)
        {
        }

        public string CallGetUrl(Page page, string? returnTo = null, string? childId = null)
        {
            return GetUrl(page, returnTo, childId);
        }
    }

    [Fact]
    public void PageRoutes_AllPageEnumValues_AreMapped()
    {
        // Arrange
        var steps = Array.Empty<IWorkflowStep>();
        var state = new JourneyState();
        var linkGenerator = Substitute.For<LinkGenerator>();
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        var httpContext = new DefaultHttpContext();
        httpContextAccessor.HttpContext.Returns(httpContext);

        linkGenerator.GetPathByAddress(
            Arg.Any<HttpContext>(),
            Arg.Any<RouteValuesAddress>(),
            Arg.Any<RouteValueDictionary>(),
            Arg.Any<RouteValueDictionary>(),
            Arg.Any<PathString?>(),
            Arg.Any<FragmentString>(),
            Arg.Any<LinkOptions>()
        ).Returns("/test-path");

        var realNavigationService = new RealNavigationServiceWrapper(steps, state, linkGenerator, httpContextAccessor);

        // Act & Assert
        foreach (Page page in Enum.GetValues<Page>())
        {
            var exception = Record.Exception(() => realNavigationService.CallGetUrl(page));
            Assert.Null(exception);
        }
    }

    [Theory]
    [InlineData(Page.Location, null, null, "/Location")]
    [InlineData(Page.CheckAnswers, null, "123", "/CheckAnswers?fromChildId=123")]
    [InlineData(Page.CheckChildDetails, null, "123", "/CheckChildDetails?fromChildId=123")]
    [InlineData(Page.ChildBirthDate, null, "123", "/ChildBirthDate?childId=123")]
    [InlineData(Page.UserAge, ReturnTo.CheckAnswers, null, "/UserAge?returnTo=check-your-answers")]
    [InlineData(Page.UserAge, ReturnTo.CheckAnswers, "123", "/UserAge?childId=123&returnTo=check-your-answers")]
    public void GetUrl_ConstructsExpectedRouteValues(Page page, string? returnTo, string? childId, string expectedUrlStub)
    {
        // Arrange
        var steps = Array.Empty<IWorkflowStep>();
        var state = new JourneyState();
        var linkGenerator = Substitute.For<LinkGenerator>();
        var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
        var httpContext = new DefaultHttpContext();
        httpContextAccessor.HttpContext.Returns(httpContext);

        linkGenerator.GetPathByAddress(
            Arg.Any<HttpContext>(),
            Arg.Any<RouteValuesAddress>(),
            Arg.Any<RouteValueDictionary>(),
            Arg.Any<RouteValueDictionary>(),
            Arg.Any<PathString?>(),
            Arg.Any<FragmentString>(),
            Arg.Any<LinkOptions>()
        ).Returns(callInfo =>
        {
            var address = callInfo.ArgAt<RouteValuesAddress>(1);
            var action = address.ExplicitValues["action"];
            var values = callInfo.ArgAt<RouteValueDictionary>(2);

            var query = "";
            if (values.TryGetValue("fromChildId", out var fcId))
            {
                query += $"?fromChildId={fcId}";
            }
            else if (values.TryGetValue("childId", out var cId))
            {
                query += $"?childId={cId}";
            }

            if (values.TryGetValue("returnTo", out var rt))
            {
                query += (string.IsNullOrEmpty(query) ? "?" : "&") + $"returnTo={rt}";
            }

            return $"/{action}{query}";
        });

        var realNavigationService = new RealNavigationServiceWrapper(steps, state, linkGenerator, httpContextAccessor);

        // Act
        var result = realNavigationService.CallGetUrl(page, returnTo, childId);

        // Assert
        Assert.Equal(expectedUrlStub, result);
    }

    [Fact]
    public void GetNextUrl_FromLocation_ReturnsChildNameUrl()
    {
        var result = _navigationService.GetNextUrl(Page.Location);

        Assert.Equal("/ChildName", result);
    }

    [Fact]
    public void GetBackUrl_FromChildName_ReturnsLocationUrl()
    {
        var result = _navigationService.GetBackUrl(Page.ChildName);

        Assert.Equal("/Location", result);
    }

    [Fact]
    public void GetNextUrl_FromHasPartner_WhenTrue_ReturnsPartnerAgeUrl()
    {
        _journeyState.HasPartner = true;

        var result = _navigationService.GetNextUrl(Page.HasPartner);

        Assert.Equal("/PartnerAge", result);
    }

    [Fact]
    public void GetNextUrl_FromHasPartner_WhenFalse_ReturnsCheckAnswersUrl()
    {
        _journeyState.HasPartner = false;

        var result = _navigationService.GetNextUrl(Page.HasPartner);

        Assert.Equal("/CheckAnswers", result);
    }

    [Fact]
    public void GetNextUrl_WithReturnToCheckAnswers_ReturnsCheckAnswersUrl()
    {
        var result = _navigationService.GetNextUrl(Page.UserAge, ReturnTo.CheckAnswers);

        Assert.Equal("/CheckAnswers", result);
    }

    [Fact]
    public void GetBackUrl_WithReturnToCheckAnswers_ReturnsCheckAnswersUrl()
    {
        var result = _navigationService.GetBackUrl(Page.UserAge, ReturnTo.CheckAnswers);

        Assert.Equal("/CheckAnswers", result);
    }

    [Fact]
    public void GetBackUrl_WhenCannotFireBack_ReturnsRootPath()
    {
        var result = _navigationService.GetBackUrl(Page.Location);

        Assert.Equal("/", result);
    }
}
