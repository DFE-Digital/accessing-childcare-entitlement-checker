using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers;

public class NavigationTests
{
    private UserController _controller;

    public static TheoryData<BackTestCase> Backwards { get; }

    public static TheoryData<IForwardTestCase> Forwards { get; }

    private readonly JourneyState _journeyState;

    public NavigationTests()
    {
        _journeyState = new JourneyState();
        var journeySession = Substitute.For<IJourneySession>();
        _controller = new UserController(_journeyState, journeySession);
    }

    static NavigationTests()
    {
        const string CheckYourAnswers = "check-your-answers";


        Backwards = [
            new BackTestCase("Nationality goes back to user age", _ => { }, c => c.Nationality(), "User", nameof(UserController.UserAge)),
            new BackTestCase("Nationality goes back to summary", _ => { }, c => c.Nationality(CheckYourAnswers), "Summary", nameof(SummaryController.CheckAnswers)),

            new BackTestCase("Settled Status goes back to nationality", _ => { }, c => c.SettledStatus(), "User", nameof(UserController.Nationality)),
            new BackTestCase("Settled Status goes back to summary", _ => { }, c => c.SettledStatus(CheckYourAnswers), "Summary", nameof(SummaryController.CheckAnswers)),

            new BackTestCase("Paid Work goes back to nationality when settled status is null", s => s.SettledStatus = null, c => c.PaidWork(), "User", nameof(UserController.Nationality)),
            new BackTestCase("Paid Work goes back to settled status when settled status is yes", s => s.SettledStatus = SettledStatusOption.Yes, c => c.PaidWork(), "User", nameof(UserController.SettledStatus)),
            new BackTestCase("Paid Work goes back to summary", s => { }, c => c.PaidWork(CheckYourAnswers), "Summary", nameof(SummaryController.CheckAnswers)),
            ];

        Forwards = [
            new ForwardTestCase<NationalityViewModel>("Nationality goes forward to paid work", _ => { }, new NationalityViewModel() { Nationality = NationalityOption.BritishOrIrishCitizen }, (c, m) => c.Nationality(m), "User", nameof(UserController.PaidWork)),
            new ForwardTestCase<NationalityViewModel>("Nationality goes forward to paid work with return to check answers", _ => { }, new NationalityViewModel() { Nationality = NationalityOption.BritishOrIrishCitizen, ReturnTo = CheckYourAnswers }, (c, m) => c.Nationality(m), "User", nameof(UserController.PaidWork)),
            new ForwardTestCase<NationalityViewModel>("Nationality goes forward to summary when paid work is yes", s => s.PaidWork = PaidWorkOption.Yes, new NationalityViewModel() { Nationality = NationalityOption.BritishOrIrishCitizen, ReturnTo = CheckYourAnswers }, (c, m) => c.Nationality(m), "Summary", nameof(SummaryController.CheckAnswers)),

            new ForwardTestCase<NationalityViewModel>("Nationality goes forward to settled status", _ => { }, new NationalityViewModel() { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland }, (c, m) => c.Nationality(m), "User", nameof(UserController.SettledStatus)),
            new ForwardTestCase<NationalityViewModel>("Nationality goes forward to settled status when settled status is yes", s => s.SettledStatus = SettledStatusOption.Yes, new NationalityViewModel() { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland }, (c, m) => c.Nationality(m), "User", nameof(UserController.SettledStatus)),
            new ForwardTestCase<NationalityViewModel>("Nationality goes forward to settled status with return to check answers", _ => { }, new NationalityViewModel() { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, ReturnTo = CheckYourAnswers }, (c, m) => c.Nationality(m), "User", nameof(UserController.SettledStatus)),
            new ForwardTestCase<NationalityViewModel>("Nationality goes forward to summary when settled status is yes and return to check answers", s => s.SettledStatus = SettledStatusOption.Yes, new NationalityViewModel() { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, ReturnTo = CheckYourAnswers }, (c, m) => c.Nationality(m), "Summary", nameof(SummaryController.CheckAnswers)),

            ];
    }

    [Theory]
    [MemberData(memberName: nameof(Backwards))]
    public void TestBackwards(BackTestCase backTestCase)
    {
        backTestCase.Arrange(_journeyState);
        var result = Assert.IsType<ViewResult>(backTestCase.Act(_controller));

        var backLinkController = result.ViewData["BackLinkController"] as string;
        var backLinkAction = result.ViewData["BackLinkAction"] as string;
        Assert.Equal(backTestCase.AssertController, backLinkController);
        Assert.Equal(backTestCase.AssertAction, backLinkAction);
    }

    [Theory]
    [MemberData(nameof(Forwards))]
    public void TestForwards(IForwardTestCase actionTestCase)
    {
        actionTestCase.Arrange(_journeyState);
        var result = actionTestCase.Act(_controller);

        var redirect = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal(actionTestCase.AssertController, redirect.ControllerName);
        Assert.Equal(actionTestCase.AssertAction, redirect.ActionName);
    }

    public interface IForwardTestCase
    {
        string Name { get; }

        Action<JourneyState> Arrange { get; }

        IActionResult Act(UserController userController);

        string AssertController { get; }

        string AssertAction { get; }
    }

    [Serializable]
    public record ForwardTestCase<TViewModel>(
        string Name,
        Action<JourneyState> Arrange,
        TViewModel ArrangeModel,
        Func<UserController, TViewModel, IActionResult> Action,
        string AssertController,
        string AssertAction) : IForwardTestCase
    {
        public IActionResult Act(UserController userController)
        {
            return Action(userController, ArrangeModel);
        }
    }

    [Serializable]
    public record BackTestCase(
        string Name,
        Action<JourneyState> Arrange,
        Func<UserController, IActionResult> Act,
        string AssertController,
        string AssertAction);
}
