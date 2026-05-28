using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class IntroductionControllerTests
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly IntroductionController _controller;
    private const string childId = "child-a";

    public IntroductionControllerTests()
    {
        _journeyState = new JourneyState();
        _journeySession = Substitute.For<IJourneySession>();
        _controller = new IntroductionController(_journeyState, _journeySession);
    }

    [Fact]
    public void SessionExpired_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.SessionExpired());
        Assert.Null(result.ViewName);
    }

    [Fact]
    public void Start_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.Start());
        Assert.Null(result.ViewName);
    }
}
