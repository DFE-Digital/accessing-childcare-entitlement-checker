using Dfe.Acec.Web.Controllers;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace Dfe.Acec.Web.Tests.Unit.Controllers;

public class SessionControllerTests : IDisposable
{
    private readonly IJourneySession _journeySession = Substitute.For<IJourneySession>();
    private readonly SessionController _controller;

    public SessionControllerTests()
    {
        _controller = new SessionController(_journeySession);
    }

    [Fact]
    public void KeepAliveReturnsNoContentWhenSessionExists()
    {
        _journeySession.HasSession.Returns(true);

        Assert.IsType<NoContentResult>(_controller.KeepAlive());
    }

    [Fact]
    public void KeepAliveReturnsUnauthorizedWhenSessionHasExpired()
    {
        _journeySession.HasSession.Returns(false);

        Assert.IsType<UnauthorizedResult>(_controller.KeepAlive());
    }

    public void Dispose() { _controller.Dispose(); GC.SuppressFinalize(this); }
}
