using Dfe.Acec.Web.Controllers;
using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.BornChildDetails;
using Dfe.Acec.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using NSubstitute;

namespace Dfe.Acec.Web.Tests.Unit.Controllers;

public class BornChildDetailsControllerTests : IDisposable
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly BornChildDetailsController _controller;
    private const string ChildId = "child-a";

    public BornChildDetailsControllerTests()
    {
        _journeyState = new JourneyState
        {
            Children =
            {
                [ChildId] = new Child(ChildId, "Child A")
                {
                    BirthStatus = BirthStatus.Born,
                }
            }
        };

        _journeySession = Substitute.For<IJourneySession>();
        _controller = new BornChildDetailsController(_journeyState, _journeySession)
        {
            Url = Substitute.For<IUrlHelper>()
        };
        _controller.Url.Action(Arg.Any<UrlActionContext>()).Returns("backlink");
    }

    [Fact]
    public void ChildBirthDateReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.ChildBirthDate(ChildId));
        Assert.Null(result.Model<ChildBirthDateViewModel>().ChildBirthDate);
        Assert.Equal("Child A", result.Model<ChildBirthDateViewModel>().ChildName);
    }

    [Fact]
    public void ChildBirthDateIfChildDoesNotExistReturnsNotFound()
    {
        Assert.IsType<NotFoundResult>(_controller.ChildBirthDate("DOES-NOT-EXIST"));
    }

    [Fact]
    public void ChildBirthDateGetPopulatesModelFromState()
    {
        Assert.True(_journeyState.Children.TryGetValue(ChildId, out var child));
        child.BirthDate = new DateOnly(2020, 1, 15);
        var result = Assert.IsType<ViewResult>(_controller.ChildBirthDate(ChildId));
        Assert.Equal(new DateOnly(2020, 1, 15), result.Model<ChildBirthDateViewModel>().ChildBirthDate);
        Assert.Equal("Child A", result.Model<ChildBirthDateViewModel>().ChildName);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ChildBirthDatePostValidSelectionSavesStateAndRedirects(bool hasReturnTo)
    {
        var model = new ChildBirthDateViewModel
        {
            ChildId = ChildId,
            ChildBirthDate = new DateOnly(2020, 1, 15),
            ReturnTo = hasReturnTo ? ReturnTo.CheckChildDetails : null
        };

        var result = _controller.ChildBirthDate(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.True(_journeyState.Children.TryGetValue(ChildId, out var child));
        Assert.Equal(new DateOnly(2020, 1, 15), child.BirthDate);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(nameof(BornChildDetailsController.ChildSupport), redirect.ActionName);
    }

    [Fact]
    public void ChildBirthDatePostValidSelectionSavesStateAndRedirectsWithReturnTo()
    {
        _journeyState.Children[ChildId].ChildSupportOptions = [ChildSupport.ArmedForcesIndependencePayment];

        var model = new ChildBirthDateViewModel
        {
            ChildId = ChildId,
            ChildBirthDate = new DateOnly(2020, 1, 15),
            ReturnTo = ReturnTo.CheckChildDetails
        };

        var result = _controller.ChildBirthDate(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.True(_journeyState.Children.TryGetValue(model.ChildId, out var child));
        Assert.Equal(new DateOnly(2020, 1, 15), child.BirthDate);
        Assert.True(_controller.ModelState.IsValid);

        // We should navigate to child support regardless of returnTo
        Assert.Equal(nameof(BornChildDetailsController.ChildSupport), redirect.ActionName);
    }

    [Fact]
    public void ChildBirthDatePostInvalidSelectionReturnsViewWithError()
    {
        var model = new ChildBirthDateViewModel
        {
            ChildId = "child-a",
            ChildBirthDate = null
        };

        _controller.ModelState.AddModelError(nameof(model.ChildBirthDate), "Faked Model Binding Error");

        var result = _controller.ChildBirthDate(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.ChildBirthDate)));
        _journeySession.DidNotReceive().SetState(_journeyState);
    }

    [Fact]
    public void ChildBirthDatePostNotFound()
    {
        var model = new ChildBirthDateViewModel
        {
            ChildId = "child-b",
        };

        var result = _controller.ChildBirthDate(model);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void ChildSupportReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.ChildSupport(ChildId));

        Assert.Equal(Array.Empty<ChildSupport>(), result.Model<ChildSupportViewModel>().ChildSupportOptions);
        Assert.Equal("Child A", result.Model<ChildSupportViewModel>().ChildName);
    }

    [Fact]
    public void ChildSupportIfChildDoesNotExistReturnsNotFound()
    {
        Assert.IsType<NotFoundResult>(_controller.ChildSupport("DOES-NOT-EXIST"));
    }

    [Fact]
    public void ChildSupportGetPopulatesModelFromState()
    {
        Assert.True(_journeyState.Children.TryGetValue(ChildId, out var child));
        child.ChildSupportOptions = [ChildSupport.ArmedForcesIndependencePayment];
        var result = Assert.IsType<ViewResult>(_controller.ChildSupport(ChildId));

        Assert.Equal(new[] { ChildSupport.ArmedForcesIndependencePayment }, result.Model<ChildSupportViewModel>().ChildSupportOptions);
        Assert.Equal("Child A", result.Model<ChildSupportViewModel>().ChildName);
    }

    [Theory]
    [InlineData(ReturnTo.CheckChildDetails, nameof(SummaryController.CheckChildDetails))]
    [InlineData(ReturnTo.CheckAnswers, nameof(SummaryController.CheckChildDetails))]
    public void ChildSupportPostValidSelectionSavesStateAndRedirects(string returnTo, string actionName)
    {
        var model = new ChildSupportViewModel
        {
            ChildId = ChildId,
            ChildSupportOptions = [ChildSupport.ArmedForcesIndependencePayment],
            ReturnTo = returnTo,
        };

        var result = _controller.ChildSupport(model);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        _journeySession.Received(1).SetState(_journeyState);
        Assert.True(_journeyState.Children.TryGetValue(ChildId, out var child));
        Assert.Equal(new[] { ChildSupport.ArmedForcesIndependencePayment }, child.ChildSupportOptions);
        Assert.True(_controller.ModelState.IsValid);
        Assert.Equal(actionName, redirect.ActionName);
        Assert.Equal("Summary", redirect.ControllerName);
    }

    [Fact]
    public void ChildSupportPostInvalidSelectionReturnsViewWithError()
    {
        var model = new ChildSupportViewModel
        {
            ChildId = "child-a",
            ChildSupportOptions = []
        };

        _controller.ModelState.AddModelError(nameof(model.ChildSupportOptions), "Faked Model Binding Error");

        var result = _controller.ChildSupport(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.ChildSupportOptions)));
        _journeySession.DidNotReceive().SetState(_journeyState);
    }

    [Fact]
    public void ChildSupportPostNotFound()
    {
        var model = new ChildSupportViewModel
        {
            ChildId = "child-b",
        };

        var result = _controller.ChildSupport(model);
        Assert.IsType<NotFoundResult>(result);
    }

    public void Dispose() { _controller.Dispose(); GC.SuppressFinalize(this); }
}
