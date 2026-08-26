using System;
using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Validators;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Summary;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Microsoft.FeatureManagement;
using AccessingChildcareEntitlementChecker.Web;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Models.User;


namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class SummaryControllerTests : IDisposable
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly IFeatureManager _featureManager;
    private readonly SummaryController _controller;
    private const string childId = "child-a";
    private readonly FakeLogger<SummaryController> _logger = new();

    public SummaryControllerTests()
    {
        _journeyState = new JourneyState();
        _journeyState.Nationality = NationalityOption.BritishOrIrishCitizen;
        _journeyState.Children[childId] = new Child(childId, "Child A")
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = new DateOnly(2020, 1, 1),
            ChildSupportOptions = [ChildSupport.NoneOfTheseApply]
        };
        _journeySession = Substitute.For<IJourneySession>();
        _featureManager = Substitute.For<IFeatureManager>();
        _featureManager.IsEnabledAsync(FeatureFlags.HmrcIntegration).Returns(false);
        var stringLocalizerFactory = AcecSubstitute.ForLocalizerFactory();


        var services = new ServiceCollection();
        services
            .AddMvcCore()
            .AddDataAnnotations();

        var metadataProvider = services
            .BuildServiceProvider()
            .GetRequiredService<IModelMetadataProvider>();

        _controller = new SummaryController(
            _journeyState,
            _journeySession,
            stringLocalizerFactory,
            new JourneyStateValidator(),
            _logger,
            _featureManager);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        _controller.TempData = new TempDataDictionary(_controller.HttpContext, Substitute.For<ITempDataProvider>());
        _controller.MetadataProvider = metadataProvider;
        _controller.Url = Substitute.For<IUrlHelper>();
        _controller.Url.Action(Arg.Any<UrlActionContext>()).Returns("backlink");
    }

    [Fact]
    public void CheckChildDetailsReturnsView()
    {
        var result = Assert.IsType<ViewResult>(_controller.CheckChildDetails());
        var checkChildDetailsViewModel = Assert.IsType<CheckChildDetailsViewModel>(result.Model);
        Assert.True(checkChildDetailsViewModel.HasChildren);
        Assert.Equal(_journeyState.CorrelationId, checkChildDetailsViewModel.CorrelationId);

        var childSummaryViewModel = Assert.Single(checkChildDetailsViewModel.Children);
        Assert.Equal(childId, childSummaryViewModel.ChildId);
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
    public void RemoveGetReturnsViewWhenChildExists()
    {
        var result = Assert.IsType<ViewResult>(_controller.Remove(childId));
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
        var model = new RemoveChildViewModel { ChildId = childId, Name = "Child A", RemoveConfirmed = null, };

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
        var model = new RemoveChildViewModel { ChildId = childId, Name = "Child A", RemoveConfirmed = false, };
        var result = Assert.IsType<RedirectToActionResult>(_controller.Remove(model));
        Assert.Equal(nameof(SummaryController.CheckChildDetails), result.ActionName);
        _journeySession.Received(0).SetState(_journeyState);
    }

    [Fact]
    public void RemovePostWhenConfirmedAndFoundRedirects()
    {
        var model = new RemoveChildViewModel { ChildId = childId, Name = "Child A", RemoveConfirmed = true, };
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

    [Fact]
    public async Task CheckAnswersReturnsView()
    {
        _journeyState.HasPartner = false;
        var result = Assert.IsType<ViewResult>(await _controller.CheckAnswers());
        var checkAnswersViewModel = Assert.IsType<CheckAnswersViewModel>(result.Model);
        Assert.True(checkAnswersViewModel.HasChildren);
        Assert.Equal(_journeyState.CorrelationId, checkAnswersViewModel.CorrelationId);
        var child = Assert.Single(checkAnswersViewModel.Children);
        Assert.Equal("child-a", child.ChildId);
        Assert.Equal("Child A", child.Name);
        Assert.Equal(2, checkAnswersViewModel.UserDetails.Count);

        var nationalityDetail = checkAnswersViewModel.UserDetails[0];
        Assert.Equal("What is your nationality?", nationalityDetail.Key);
        Assert.Equal("British or Irish citizen", nationalityDetail.Value);
        Assert.Equal("Nationality", nationalityDetail.ChangeAction);
        Assert.Equal("User", nationalityDetail.ChangeController);

        var hasPartnerDetail = checkAnswersViewModel.UserDetails[1];
        Assert.Equal("Title", hasPartnerDetail.Key);
        Assert.Equal("No", hasPartnerDetail.Value);
        Assert.Equal("HasPartner", hasPartnerDetail.ChangeAction);
        Assert.Equal("User", hasPartnerDetail.ChangeController);
    }

    [Fact]
    public async Task CheckAnswersReturnsViewWithFromChild()
    {
        _journeyState.HasPartner = false;

        var result = Assert.IsType<ViewResult>(
            await _controller.CheckAnswers(fromChildId: "child-a"));

        var model = Assert.IsType<CheckAnswersViewModel>(result.Model);
        Assert.Equal("child-a", model.LastEditedChild!.ChildId);
        Assert.Equal(_journeyState.CorrelationId, model.CorrelationId);
    }

    [Fact]
    public async Task CheckAnswersReturnsViewWithPartner()
    {
        _journeyState.HasPartner = true;
        _journeyState.PartnerAge = AgeRange.TwentyOneOrOver;

        var result = Assert.IsType<ViewResult>(
            await _controller.CheckAnswers());

        var checkAnswersViewModel =
            Assert.IsType<CheckAnswersViewModel>(result.Model);

        Assert.Equal(
            _journeyState.CorrelationId,
            checkAnswersViewModel.CorrelationId);

        var partnerDetail = checkAnswersViewModel.PartnerDetails[0];
        Assert.Equal("Title", partnerDetail.Key);
        Assert.Equal("Option_21OrOver", partnerDetail.Value);
        Assert.Equal("PartnerAge", partnerDetail.ChangeAction);
        Assert.Equal("Partner", partnerDetail.ChangeController);
    }

    [Fact]

    public async Task CheckAnswersSuppressesLocationRowWhenFeatureFlagEnabled()
    {
        _featureManager.IsEnabledAsync(FeatureFlags.HmrcIntegration).Returns(true);
        _journeyState.HasPartner = false;
        _journeyState.CountryOfResidence = CountryOfResidence.England;

        var result = Assert.IsType<ViewResult>(await _controller.CheckAnswers());
        var checkAnswersViewModel = Assert.IsType<CheckAnswersViewModel>(result.Model);

        // Should not have any location detail in UserDetails
        Assert.DoesNotContain(checkAnswersViewModel.UserDetails, x => x.Key == "Where do you live?");
    }

    [Fact]
    public void CheckAnswersPostRedirectsWhenCorrelationIdMatchesAndValidationPasses()
    {
        _journeyState.CountryOfResidence = CountryOfResidence.England;
        _journeyState.Nationality = NationalityOption.BritishOrIrishCitizen;
        _journeyState.UserAge = AgeRange.TwentyOneOrOver;
        _journeyState.PaidWork = PaidWorkOption.No;
        _journeyState.UniversalCredit = UniversalCreditOption.DoesNotReceive;
        _journeyState.Benefits = [BenefitsOption.None];
        _journeyState.ChildcareSupport = [ChildcareSupportOption.None];
        _journeyState.HasPartner = false;

        var model = new CheckAnswersSubmitModel(_journeyState.CorrelationId);

        var result = Assert.IsType<RedirectToActionResult>(
            _controller.CheckAnswers(model));

        Assert.Equal(nameof(ResultsController.Results), result.ActionName);
        Assert.Equal(ResultsController.Name, result.ControllerName);
    }

    [Fact]
    public void CheckAnswersPostReturnsStateMismatchWhenCorrelationIdMismatches()
    {
        var model = new CheckAnswersSubmitModel(Guid.NewGuid());
        var result = Assert.IsType<ViewResult>(_controller.CheckAnswers(model));
        Assert.Equal("StateMismatch", result.ViewName);
        Assert.Equal(400, _controller.Response.StatusCode);

        Assert.Contains("State mismatch detected. Correlation ID mismatch. Event: StateMismatch", _logger.Messages);
        var customEventProp = Assert.Single(_logger.Properties, p => p.Key == "microsoft.custom_event.name");
        Assert.Equal("StateMismatch", customEventProp.Value);
    }

    [Fact]
    public void CheckAnswersPostDoesNotValidateWhenCorrelationIdMismatches()
    {
        var validator = Substitute.For<IValidator<JourneyState>>();

        var controller = new SummaryController(
            _journeyState,
            _journeySession,
            AcecSubstitute.ForLocalizerFactory(),
            validator,
            _logger,
            _featureManager);

        controller.ControllerContext = _controller.ControllerContext;
        controller.MetadataProvider = _controller.MetadataProvider;
        controller.Url = _controller.Url;

        var model = new CheckAnswersSubmitModel(Guid.NewGuid());

        controller.CheckAnswers(model);

        validator.DidNotReceive().Validate(Arg.Any<ValidationContext<JourneyState>>());
    }

    [Fact]
    public void CheckChildDetailsPostRedisplaysViewWhenValidationFails()
    {
        // Arrange
        var mockValidator = Substitute.For<IValidator<JourneyState>>();
        var validationResult = new ValidationResult(new[] { new ValidationFailure("Children", "Child validation error") });
        mockValidator.Validate(Arg.Any<ValidationContext<JourneyState>>()).Returns(validationResult);

        var localizerFactory = AcecSubstitute.ForLocalizerFactory();
        var controller = new SummaryController(
            _journeyState,
            _journeySession,
            AcecSubstitute.ForLocalizerFactory(),
            mockValidator,
            _logger,
            _featureManager);
        controller.ControllerContext = _controller.ControllerContext;
        controller.MetadataProvider = _controller.MetadataProvider;
        controller.Url = _controller.Url;

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

    [Fact]
    public void CheckAnswersPostReturnsStateMismatchWhenValidationFails()
    {
        var mockValidator = Substitute.For<IValidator<JourneyState>>();

        var validationResult = new ValidationResult(
            new[]
            {
                new ValidationFailure(
                    "WeeklyEarnings",
                    "Earnings validation error")
            });

        mockValidator
            .Validate(Arg.Any<ValidationContext<JourneyState>>())
            .Returns(validationResult);

        var controller = new SummaryController(
            _journeyState,
            _journeySession,
            AcecSubstitute.ForLocalizerFactory(),
            mockValidator,
            _logger,
            _featureManager);

        controller.ControllerContext = _controller.ControllerContext;
        controller.MetadataProvider = _controller.MetadataProvider;
        controller.Url = _controller.Url;

        var model = new CheckAnswersSubmitModel(
            _journeyState.CorrelationId);

        var result = Assert.IsType<ViewResult>(
            controller.CheckAnswers(model));

        Assert.Equal("StateMismatch", result.ViewName);
        Assert.Equal(400, controller.Response.StatusCode);
    }

    public void Dispose() { _controller?.Dispose(); GC.SuppressFinalize(this); }
}
