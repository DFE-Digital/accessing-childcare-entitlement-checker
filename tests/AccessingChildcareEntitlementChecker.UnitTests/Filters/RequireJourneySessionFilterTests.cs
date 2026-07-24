using AccessingChildcareEntitlementChecker.Web.Filters;
using AccessingChildcareEntitlementChecker.Web.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Filters;

public class RequireJourneySessionFilterTests
{
    private readonly ILogger<RequireJourneySessionFilter> _mockLogger;
    private readonly IJourneySession _mockJourneySession;
    private readonly ResourceExecutingContext _context;
    private readonly ResourceExecutionDelegate _next;
    private readonly RequireJourneySessionFilter _sut;
    private readonly List<string> _loggedMessages = [];

    public RequireJourneySessionFilterTests()
    {
        _mockLogger = Substitute.For<ILogger<RequireJourneySessionFilter>>();
        _mockLogger.IsEnabled(LogLevel.Information).Returns(true);

        // Capture log messages synchronously during the Log invocation before State is disposed/cleared.
        _mockLogger.When(x => x.Log(
            Arg.Any<LogLevel>(),
            Arg.Any<EventId>(),
            Arg.Any<Arg.AnyType>(),
            Arg.Any<Exception>(),
            Arg.Any<Func<Arg.AnyType, Exception?, string>>()
        )).Do(call =>
        {
            var state = call.Args()[2];
            var exception = (Exception?)call.Args()[3];
            var formatter = (Delegate)call.Args()[4]!;
            var message = (string)formatter.DynamicInvoke(state, exception)!;
            _loggedMessages.Add(message);
        });

        _mockJourneySession = Substitute.For<IJourneySession>();

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Path = "/foo";
        var actionContext = new ActionContext(
               httpContext,
               new RouteData(),
               new ActionDescriptor());

        _context = new ResourceExecutingContext(actionContext, [], []);
        _next = new ResourceExecutionDelegate(() => Task.FromResult(new ResourceExecutedContext(actionContext, [])));

        _sut = new RequireJourneySessionFilter(_mockLogger, _mockJourneySession);
    }

    [Fact]
    public async Task OnActionExecuting_JourneySessionIsNull()
    {
        _mockJourneySession.HasSession.Returns(false);
        await _sut.OnResourceExecutionAsync(_context, _next);
        Assert.IsType<RedirectToActionResult>(_context.Result);

        AssertLogged("Redirecting session-less request for /foo to SessionExpired.");
    }

    [Fact]
    public async Task OnActionExecuting_JourneySessionIsNotNull()
    {
        _mockJourneySession.HasSession.Returns(true);
        await _sut.OnResourceExecutionAsync(_context, _next);
        Assert.Null(_context.Result);
        Assert.False(_mockLogger.ReceivedCalls().Any());
    }

    private void AssertLogged(string expectedMessage)
    {
        Assert.Contains(expectedMessage, _loggedMessages);
    }
}
