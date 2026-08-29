using Dfe.Acec.Web.Controllers;
using Dfe.Acec.Web.Models.Summary;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Services.Summary;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using NSubstitute;

namespace Dfe.Acec.Web.Tests.Unit.Controllers;

public class SummaryControllerCheckChildDetailsTests : IDisposable
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly ISummaryViewModelBuilder _summaryViewModelBuilder;
    private readonly SummaryController _controller;
    private readonly FakeLogger<SummaryController> _logger = new();
    private const string ChildId = "child-a";

    public SummaryControllerCheckChildDetailsTests()
    {
        _journeyState = SummaryControllerTestFactory.CreateDefaultJourneyState(ChildId);
        _journeySession = Substitute.For<IJourneySession>();
        var featureManager = Substitute.For<IFeatureManager>();
        featureManager.IsEnabledAsync(FeatureFlags.HmrcIntegration).Returns(false);
        _summaryViewModelBuilder = SummaryControllerTestFactory.CreateRealViewModelBuilder(featureManager);

        _controller = SummaryControllerTestFactory.Create(
            _journeyState,
            _journeySession,
            new Dfe.Acec.Web.Validators.JourneyStateValidator(),
            _logger,
            _summaryViewModelBuilder);
    }

    [Fact]
    public void CheckChildDetailsReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.CheckChildDetails());
        var checkChildDetailsViewModel = Assert.IsType<CheckChildDetailsViewModel>(result.Model);
        Assert.True(checkChildDetailsViewModel.HasChildren);
        Assert.Equal(_journeyState.CorrelationId, checkChildDetailsViewModel.CorrelationId);

        var childSummaryViewModel = Assert.Single(checkChildDetailsViewModel.Children);
        Assert.Equal(ChildId, childSummaryViewModel.ChildId);
        Assert.Equal("Child A", childSummaryViewModel.Name);
    }

    [Fact]
    public void CheckChildDetailsReturnsViewWithFromChild()
    {
        var result = Assert.IsType<ViewResult>(_controller.CheckChildDetails(childId: "child-a"));
        var model = Assert.IsType<CheckChildDetailsViewModel>(result.Model);
        Assert.Equal("child-a", model.LastEditedChild!.ChildId);
        Assert.Equal(_journeyState.CorrelationId, model.CorrelationId);
    }

    [Fact]
    public void CheckChildDetailsPostRedirectsWhenCorrelationIdMatches()
    {
        var model = new CheckChildDetailsSubmitModel(_journeyState.CorrelationId);
        var result = Assert.IsType<RedirectToActionResult>(_controller.CheckChildDetails(model));
        Assert.Equal(nameof(UserController.UserAge), result.ActionName);
        Assert.Equal(UserController.Name, result.ControllerName);
    }

    [Fact]
    public void CheckChildDetailsPostReturnsStateMismatchWhenCorrelationIdMismatches()
    {
        var model = new CheckChildDetailsSubmitModel(Guid.NewGuid());
        var result = Assert.IsType<ViewResult>(_controller.CheckChildDetails(model));
        Assert.Equal("StateMismatch", result.ViewName);
        Assert.Equal(400, _controller.Response.StatusCode);

        Assert.Contains("State mismatch detected. Correlation ID mismatch. Event: StateMismatch", _logger.Messages);
        var customEventProp = Assert.Single(_logger.Properties, p => p.Key == "microsoft.custom_event.name");
        Assert.Equal("StateMismatch", customEventProp.Value);
    }

    [Fact]
    public void CheckChildDetailsPostRedisplaysViewWhenValidationFails()
    {
        // Arrange
        var mockValidator = Substitute.For<IValidator<JourneyState>>();
        var validationResult = new ValidationResult([new ValidationFailure("Children", "Child validation error")]);
        mockValidator.Validate(Arg.Any<ValidationContext<JourneyState>>()).Returns(validationResult);

        var controller = SummaryControllerTestFactory.Create(
            _journeyState,
            _journeySession,
            mockValidator,
            _logger,
            _summaryViewModelBuilder);

        var model = new CheckChildDetailsSubmitModel(_journeyState.CorrelationId);

        // Act
        var result = Assert.IsType<ViewResult>(controller.CheckChildDetails(model));

        // Assert
        Assert.False(controller.ModelState.IsValid);
        Assert.True(controller.ModelState.ContainsKey("Children"));
        var error = Assert.Single(controller.ModelState["Children"]!.Errors);
        Assert.Equal("Child validation error", error.ErrorMessage);

        var viewModel = Assert.IsType<CheckChildDetailsViewModel>(result.Model);
        Assert.Equal(_journeyState.CorrelationId, viewModel.CorrelationId);
    }

    public void Dispose()
    {
        _controller.Dispose();
        GC.SuppressFinalize(this);
    }
}
