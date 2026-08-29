using Dfe.Acec.Web.Filters;
using Dfe.Acec.Web.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NSubstitute;

namespace Dfe.Acec.Tests.Unit.Filters;

public class RequireJourneySessionFilterTests
{
    private readonly FakeLogger<RequireJourneySessionFilter> _mockLogger = new();
    private readonly IJourneySession _mockJourneySession;
    private readonly ResourceExecutingContext _context;
    private readonly ResourceExecutionDelegate _next;
    private readonly RequireJourneySessionFilter _sut;

    public RequireJourneySessionFilterTests()
    {
        _mockJourneySession = Substitute.For<IJourneySession>();

        var httpContext = new DefaultHttpContext
        {
            Request = { Path = "/foo" }
        };
        var actionContext = new ActionContext(
               httpContext,
               new RouteData(),
               new ActionDescriptor());

        _context = new ResourceExecutingContext(actionContext, [], []);
        _next = () => Task.FromResult(new ResourceExecutedContext(actionContext, []));

        _sut = new RequireJourneySessionFilter(_mockLogger, _mockJourneySession);
    }

    [Fact]
    public async Task OnActionExecutingJourneySessionIsNull()
    {
        _mockJourneySession.HasSession.Returns(false);
        await _sut.OnResourceExecutionAsync(_context, _next);
        Assert.IsType<RedirectToActionResult>(_context.Result);

        AssertLogged("Redirecting session-less request for /foo to SessionExpired.");
    }

    [Fact]
    public async Task OnActionExecutingJourneySessionIsNotNull()
    {
        _mockJourneySession.HasSession.Returns(true);
        await _sut.OnResourceExecutionAsync(_context, _next);
        Assert.Null(_context.Result);
        Assert.Empty(_mockLogger.Messages);
    }

    private void AssertLogged(string expectedMessage)
    {
        Assert.Contains(expectedMessage, _mockLogger.Messages);
    }
}
