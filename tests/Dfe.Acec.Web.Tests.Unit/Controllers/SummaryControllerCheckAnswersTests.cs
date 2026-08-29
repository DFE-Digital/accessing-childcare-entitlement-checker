using Dfe.Acec.Web.Controllers;
using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.Summary;
using Dfe.Acec.Web.Models.User;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Services.Summary;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using NSubstitute;

namespace Dfe.Acec.Web.Tests.Unit.Controllers;

public class SummaryControllerCheckAnswersTests : IDisposable
{
    private readonly JourneyState _journeyState;
    private readonly IJourneySession _journeySession;
    private readonly IFeatureManager _featureManager;
    private readonly ISummaryViewModelBuilder _summaryViewModelBuilder;
    private readonly SummaryController _controller;
    private readonly FakeLogger<SummaryController> _logger = new();
    private const string ChildId = "child-a";

    public SummaryControllerCheckAnswersTests()
    {
        _journeyState = SummaryControllerTestFactory.CreateDefaultJourneyState(ChildId);
        _journeySession = Substitute.For<IJourneySession>();
        _featureManager = Substitute.For<IFeatureManager>();
        _featureManager.IsEnabledAsync(FeatureFlags.HmrcIntegration).Returns(false);
        _summaryViewModelBuilder = SummaryControllerTestFactory.CreateRealViewModelBuilder(_featureManager);

        _controller = SummaryControllerTestFactory.Create(
            _journeyState,
            _journeySession,
            new Dfe.Acec.Web.Validators.JourneyStateValidator(),
            _logger,
            _summaryViewModelBuilder);
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

        var controller = SummaryControllerTestFactory.Create(
            _journeyState,
            _journeySession,
            validator,
            _logger,
            _summaryViewModelBuilder);

        var model = new CheckAnswersSubmitModel(Guid.NewGuid());

        controller.CheckAnswers(model);

        validator.DidNotReceive().Validate(Arg.Any<ValidationContext<JourneyState>>());
    }

    [Fact]
    public void CheckAnswersPostReturnsStateMismatchWhenValidationFails()
    {
        var mockValidator = Substitute.For<IValidator<JourneyState>>();

        var validationResult = new ValidationResult(
        [
            new ValidationFailure(
                    "WeeklyEarnings",
                    "Earnings validation error")
        ]);

        mockValidator
            .Validate(Arg.Any<ValidationContext<JourneyState>>())
            .Returns(validationResult);

        var controller = SummaryControllerTestFactory.Create(
            _journeyState,
            _journeySession,
            mockValidator,
            _logger,
            _summaryViewModelBuilder);

        var model = new CheckAnswersSubmitModel(
            _journeyState.CorrelationId);

        var result = Assert.IsType<ViewResult>(
            controller.CheckAnswers(model));

        Assert.Equal("StateMismatch", result.ViewName);
        Assert.Equal(400, controller.Response.StatusCode);
    }

    public void Dispose()
    {
        _controller.Dispose();
        GC.SuppressFinalize(this);
    }
}
