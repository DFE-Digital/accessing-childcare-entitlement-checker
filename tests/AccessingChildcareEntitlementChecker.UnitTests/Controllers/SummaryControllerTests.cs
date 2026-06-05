using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Summary;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class SummaryControllerTests
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly SummaryController _controller;
    private const string childId = "child-a";

    public SummaryControllerTests()
    {
        _journeyState = new JourneyState();
        _journeyState.Nationality = NationalityOption.BritishOrIrishCitizen;
        _journeyState.Children[childId] = new Child(childId, "Child A");
        _journeySession = Substitute.For<IJourneySession>();
        var stringLocalizerFactory = Substitute.For<IStringLocalizerFactory>();

        var services = new ServiceCollection();
        services
            .AddMvcCore()
            .AddDataAnnotations();

        var metadataProvider = services
            .BuildServiceProvider()
            .GetRequiredService<IModelMetadataProvider>();

        _controller = new SummaryController(_journeyState, _journeySession, stringLocalizerFactory);
        _controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Substitute.For<ITempDataProvider>());
        _controller.MetadataProvider = metadataProvider;
    }

    [Fact]
    public void CheckChildDetails_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.CheckChildDetails());
        var checkChildDetailsViewModel = Assert.IsType<CheckChildDetailsViewModel>(result.Model);
        Assert.True(checkChildDetailsViewModel.HasChildren);

        var childSummaryViewModel = Assert.Single(checkChildDetailsViewModel.Children);
        Assert.Equal(childId, childSummaryViewModel.ChildId);
        Assert.Equal("Child A", childSummaryViewModel.Name);
    }

    [Fact]
    public void CheckChildDetails_ReturnsView_WithFromChild()
    {
        var result = Assert.IsType<ViewResult>(_controller.CheckChildDetails(fromChildId: "child-a"));
        var model = Assert.IsType<CheckChildDetailsViewModel>(result.Model);
        Assert.Equal("child-a", model.LastEditedChild!.ChildId);
    }

    [Fact]
    public void Remove_Get_ReturnsView_WhenChildExists()
    {
        var result = Assert.IsType<ViewResult>(_controller.Remove(childId));
        Assert.IsType<RemoveChildViewModel>(result.Model);
        Assert.Equal("Child A", result.Model<RemoveChildViewModel>().Name);
    }

    [Fact]
    public void Remove_Get_Redirects_WhenChildDoesNotExist()
    {
        var result = Assert.IsType<RedirectToActionResult>(_controller.Remove("DOES-NOT-EXIST"));
        Assert.Equal(nameof(SummaryController.CheckChildDetails), result.ActionName);
    }

    [Fact]
    public void Remove_Get_Redirects_WhenChildIdNotPassed()
    {
        var result = Assert.IsType<RedirectToActionResult>(_controller.Remove((string?)null));
        Assert.Equal(nameof(SummaryController.CheckChildDetails), result.ActionName);
    }

    [Fact]
    public void Remove_Post_WhenNotValidReturns()
    {
        var model = new RemoveChildViewModel { ChildId = childId, Name = "Child A", RemoveConfirmed = null, };

        _controller.ModelState.AddModelError(nameof(model.RemoveConfirmed), "Faked Model Binding Error");

        var result = _controller.Remove(model);

        Assert.IsType<ViewResult>(result);
        Assert.False(_controller.ModelState.IsValid);
        Assert.True(_controller.ModelState.ContainsKey(nameof(model.RemoveConfirmed)));
        _journeySession.DidNotReceive().Set(_journeyState);
    }

    [Fact]
    public void Remove_Post_WhenNotConfirmed_Redirects()
    {
        var model = new RemoveChildViewModel { ChildId = childId, Name = "Child A", RemoveConfirmed = false, };
        var result = Assert.IsType<RedirectToActionResult>(_controller.Remove(model));
        Assert.Equal(nameof(SummaryController.CheckChildDetails), result.ActionName);
        _journeySession.Received(0).Set(_journeyState);
    }

    [Fact]
    public void Remove_Post_WhenConfirmed_AndFound_Redirects()
    {
        var model = new RemoveChildViewModel { ChildId = childId, Name = "Child A", RemoveConfirmed = true, };
        var result = Assert.IsType<RedirectToActionResult>(_controller.Remove(model));
        Assert.Equal(nameof(SummaryController.CheckChildDetails), result.ActionName);
        _journeySession.Received(1).Set(_journeyState);
    }

    [Fact]
    public void Remove_Post_WhenConfirmed_AndNotFound_Redirects()
    {
        var model = new RemoveChildViewModel { ChildId = "child-b", Name = "Child B", RemoveConfirmed = true, };
        var result = Assert.IsType<RedirectToActionResult>(_controller.Remove(model));
        Assert.Equal(nameof(SummaryController.CheckChildDetails), result.ActionName);
        _journeySession.Received(0).Set(_journeyState);
    }

    [Fact]
    public void CheckAnswers_ReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.CheckAnswers());
        var checkAnswersViewModel = Assert.IsType<CheckAnswersViewModel>(result.Model);
        Assert.True(checkAnswersViewModel.HasChildren);
        var child = Assert.Single(checkAnswersViewModel.Children);
        Assert.Equal("child-a", child.ChildId);
        Assert.Equal("Child A", child.Name);
        var userDetail = Assert.Single(checkAnswersViewModel.UserDetails);
        Assert.Equal("What is your nationality?", userDetail.Key);
        Assert.Equal("British or Irish citizen", userDetail.Value);
        Assert.Equal("Nationality", userDetail.ChangeAction);
        Assert.Equal("User", userDetail.ChangeController);
    }

    [Fact]
    public void CheckAnswers_ReturnsView_WithFromChild()
    {
        var result = Assert.IsType<ViewResult>(_controller.CheckAnswers(fromChildId: "child-a"));
        var model = Assert.IsType<CheckAnswersViewModel>(result.Model);
        Assert.Equal("child-a", model.LastEditedChild!.ChildId);
    }
}
