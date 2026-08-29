using Dfe.Acec.Web.Controllers;
using Dfe.Acec.Web.Models.Summary;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using NSubstitute;

namespace Dfe.Acec.Web.Tests.Unit.Controllers;

public class SummaryControllerRemoveTests : IDisposable
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly SummaryController _controller;
    private readonly FakeLogger<SummaryController> _logger = new();
    private const string ChildId = "child-a";

    public SummaryControllerRemoveTests()
    {
        _journeyState = SummaryControllerTestFactory.CreateDefaultJourneyState(ChildId);
        _journeySession = Substitute.For<IJourneySession>();
        var featureManager = Substitute.For<IFeatureManager>();
        featureManager.IsEnabledAsync(FeatureFlags.HmrcIntegration).Returns(false);
        var summaryViewModelBuilder = SummaryControllerTestFactory.CreateRealViewModelBuilder(featureManager);

        _controller = SummaryControllerTestFactory.Create(
            _journeyState,
            _journeySession,
            new Dfe.Acec.Web.Validators.JourneyStateValidator(),
            _logger,
            summaryViewModelBuilder);
    }

    [Fact]
    public void RemoveGetReturnsViewWhenChildExists()
    {
        var result = Assert.IsType<ViewResult>(_controller.Remove(ChildId));
        Assert.IsType<RemoveChildViewModel>(result.Model);
        Assert.Equal("Child A", result.Model<RemoveChildViewModel>().Name);
    }

    [Fact]
    public void RemoveGetRedirectsWhenChildDoesNotExist()
    {
        var result = Assert.IsType<RedirectToActionResult>(_controller.Remove("DOES-NOT-EXIST"));
        Assert.Equal(nameof(SummaryController.CheckChildDetails), result.ActionName);
    }

    [Fact]
    public void RemoveGetRedirectsWhenChildIdNotPassed()
    {
        var result = Assert.IsType<RedirectToActionResult>(_controller.Remove((string?)null));
        Assert.Equal(nameof(SummaryController.CheckChildDetails), result.ActionName);
    }

    [Fact]
    public void RemovePostWhenNotValidReturns()
    {
        var model = new RemoveChildViewModel { ChildId = ChildId, Name = "Child A", RemoveConfirmed = null, };

        _controller.ModelState.AddModelError(nameof(model.RemoveConfirmed), "Faked Model Binding Error");

        var result = _controller.Remove(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.RemoveConfirmed)));
        _journeySession.DidNotReceive().SetState(_journeyState);
    }

    [Fact]
    public void RemovePostWhenNotConfirmedRedirects()
    {
        var model = new RemoveChildViewModel { ChildId = ChildId, Name = "Child A", RemoveConfirmed = false, };
        var result = Assert.IsType<RedirectToActionResult>(_controller.Remove(model));
        Assert.Equal(nameof(SummaryController.CheckChildDetails), result.ActionName);
        _journeySession.Received(0).SetState(_journeyState);
    }

    [Fact]
    public void RemovePostWhenConfirmedAndFoundRedirects()
    {
        var model = new RemoveChildViewModel { ChildId = ChildId, Name = "Child A", RemoveConfirmed = true, };
        var result = Assert.IsType<RedirectToActionResult>(_controller.Remove(model));
        Assert.Equal(nameof(SummaryController.CheckChildDetails), result.ActionName);
        _journeySession.Received(1).SetState(_journeyState);
    }

    [Fact]
    public void RemovePostWhenConfirmedAndNotFoundRedirects()
    {
        var model = new RemoveChildViewModel { ChildId = "child-b", Name = "Child B", RemoveConfirmed = true, };
        var result = Assert.IsType<RedirectToActionResult>(_controller.Remove(model));
        Assert.Equal(nameof(SummaryController.CheckChildDetails), result.ActionName);
        _journeySession.Received(0).SetState(_journeyState);
    }

    public void Dispose()
    {
        _controller.Dispose();
        GC.SuppressFinalize(this);
    }
}
