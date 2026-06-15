using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Collections;
using System.Linq.Expressions;

namespace AccessingChildcareEntitlementChecker.UnitTests.Controllers.Navigation;

public class ControllerNavigationTests
{
    public static TheoryData<ValidityTestCase> Validity;

    public static TheoryData<BackwardTestCase> Backwards { get; }

    public static TheoryData<ForwardTestCase> Forwards { get; }

    static ControllerNavigationTests()
    {
        var withoutChildren = new Dictionary<string, Child>();
        var withChildren = new Dictionary<string, Child>
            {
                { "child1", new Child("child1", "Child 1") }
            };

        Validity = [
            new ValidityTestCase(new JourneyState { Children = withoutChildren }, c => c.UserAge(), true),
            new ValidityTestCase(new JourneyState { Children = withChildren }, n => n.UserAge(), false),

            new ValidityTestCase(new JourneyState { UserAge = null }, n => n.Nationality(), true),
            new ValidityTestCase(new JourneyState { UserAge = AgeRange.UnderEighteen }, n => n.Nationality(), false),

            new ValidityTestCase(new JourneyState { Nationality = null }, n => n.SettledStatus(), true),
            new ValidityTestCase(new JourneyState { Nationality = NationalityOption.BritishOrIrishCitizen }, n => n.SettledStatus(), true),
            new ValidityTestCase(new JourneyState { Nationality = NationalityOption.CitizenOfADifferentCountry }, n => n.SettledStatus(), true),
            new ValidityTestCase(new JourneyState { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland }, n => n.SettledStatus(), false),

            new ValidityTestCase(new JourneyState { Nationality = null }, n => n.PaidWork(), true),
            new ValidityTestCase(new JourneyState { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatus = null }, n => n.PaidWork(), true),
            new ValidityTestCase(new JourneyState { Nationality = NationalityOption.BritishOrIrishCitizen }, n => n.PaidWork(), false),
            new ValidityTestCase(new JourneyState { Nationality = NationalityOption.CitizenOfADifferentCountry }, n => n.PaidWork(), false),
            new ValidityTestCase(new JourneyState { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatus = SettledStatusOption.Yes }, n => n.PaidWork(), false),
        ];

        Backwards = [
            new BackwardTestCase(new JourneyState { Children = withChildren }, c => c.UserAge(), nameof(SummaryController.CheckChildDetails)),
            new BackwardTestCase(new JourneyState { Children = withChildren }, c => c.UserAge(ReturnTo.CheckAnswers), nameof(SummaryController.CheckAnswers)),

            new BackwardTestCase(new JourneyState { UserAge = AgeRange.UnderEighteen }, c => c.Nationality(), nameof(UserController.UserAge)),
            new BackwardTestCase(new JourneyState { UserAge = AgeRange.UnderEighteen }, c => c.Nationality(ReturnTo.CheckAnswers), nameof(SummaryController.CheckAnswers)),

            new BackwardTestCase(new JourneyState { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland }, c => c.SettledStatus(), nameof(UserController.Nationality)),
            new BackwardTestCase(new JourneyState { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland }, c => c.SettledStatus(ReturnTo.CheckAnswers), nameof(SummaryController.CheckAnswers)),

            new BackwardTestCase(new JourneyState { Nationality = NationalityOption.BritishOrIrishCitizen }, c => c.PaidWork(), nameof(UserController.Nationality)),
            new BackwardTestCase(new JourneyState { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatus = SettledStatusOption.Yes }, c => c.PaidWork(), nameof(UserController.SettledStatus)),
            new BackwardTestCase(new JourneyState { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatus = SettledStatusOption.Yes }, c => c.PaidWork(ReturnTo.CheckAnswers), nameof(SummaryController.CheckAnswers)),
        ];

        Forwards = [
            new ForwardTestCase(
                new JourneyState { Children = withChildren },
                c => c.UserAge(new UserAgeViewModel { UserAge = AgeRange.UnderEighteen }),
                nameof(UserController.Nationality)),
            new ForwardTestCase(
                new JourneyState { Children = withChildren },
                c => c.UserAge(new UserAgeViewModel { UserAge = AgeRange.UnderEighteen, ReturnTo = ReturnTo.CheckAnswers }),
                nameof(SummaryController.CheckAnswers)),

            new ForwardTestCase(
                new JourneyState { UserAge = AgeRange.UnderEighteen },
                c => c.Nationality(new NationalityViewModel { Nationality = NationalityOption.BritishOrIrishCitizen}),
                nameof(UserController.PaidWork)),
            new ForwardTestCase(
                new JourneyState { UserAge = AgeRange.UnderEighteen },
                c => c.Nationality(new NationalityViewModel { Nationality = NationalityOption.BritishOrIrishCitizen, ReturnTo = ReturnTo.CheckAnswers }),
                nameof(UserController.PaidWork)),
            new ForwardTestCase(
                new JourneyState { UserAge = AgeRange.UnderEighteen, PaidWork = PaidWorkOption.Yes },
                c => c.Nationality(new NationalityViewModel { Nationality = NationalityOption.BritishOrIrishCitizen, ReturnTo = ReturnTo.CheckAnswers }),
                nameof(SummaryController.CheckAnswers)),
            new ForwardTestCase(
                new JourneyState { UserAge = AgeRange.UnderEighteen },
                c => c.Nationality(new NationalityViewModel { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland }),
                nameof(UserController.SettledStatus)),
            new ForwardTestCase(
                new JourneyState { UserAge = AgeRange.UnderEighteen },
                c => c.Nationality(new NationalityViewModel { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, ReturnTo = ReturnTo.CheckAnswers }),
                nameof(UserController.SettledStatus)),
            new ForwardTestCase(
                new JourneyState { UserAge = AgeRange.UnderEighteen, SettledStatus = SettledStatusOption.Yes },
                c => c.Nationality(new NationalityViewModel { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, ReturnTo = ReturnTo.CheckAnswers }),
                nameof(SummaryController.CheckAnswers)),

            new ForwardTestCase(
                new JourneyState { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland },
                c => c.SettledStatus(new SettledStatusViewModel { SettledStatus = SettledStatusOption.Yes }),
                nameof(UserController.PaidWork)),
            new ForwardTestCase(
                new JourneyState { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland },
                c => c.SettledStatus(new SettledStatusViewModel { SettledStatus = SettledStatusOption.Yes, ReturnTo = ReturnTo.CheckAnswers }),
                nameof(UserController.PaidWork)),
            new ForwardTestCase(
                new JourneyState { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, PaidWork = PaidWorkOption.Yes },
                c => c.SettledStatus(new SettledStatusViewModel { SettledStatus = SettledStatusOption.Yes, ReturnTo = ReturnTo.CheckAnswers }),
                nameof(SummaryController.CheckAnswers)),

            new ForwardTestCase(
                new JourneyState { Nationality = NationalityOption.BritishOrIrishCitizen },
                c => c.PaidWork(new PaidWorkViewModel { PaidWork = PaidWorkOption.Yes }),
                nameof(UserController.WorkStatus)),
            new ForwardTestCase(
                new JourneyState { Nationality = NationalityOption.BritishOrIrishCitizen },
                c => c.PaidWork(new PaidWorkViewModel { PaidWork = PaidWorkOption.Yes, ReturnTo = ReturnTo.CheckAnswers }),
                nameof(UserController.WorkStatus)),
            new ForwardTestCase(
                new JourneyState { Nationality = NationalityOption.BritishOrIrishCitizen, WorkStatus = [WorkStatusOption.SelfEmployed] },
                c => c.PaidWork(new PaidWorkViewModel { PaidWork = PaidWorkOption.Yes, ReturnTo = ReturnTo.CheckAnswers }),
                nameof(SummaryController.CheckAnswers)),
            new ForwardTestCase(
                new JourneyState { Nationality = NationalityOption.BritishOrIrishCitizen },
                c => c.PaidWork(new PaidWorkViewModel { PaidWork = PaidWorkOption.No }),
                nameof(UserController.UniversalCredit)),
            new ForwardTestCase(
                new JourneyState { Nationality = NationalityOption.BritishOrIrishCitizen },
                c => c.PaidWork(new PaidWorkViewModel { PaidWork = PaidWorkOption.No, ReturnTo = ReturnTo.CheckAnswers }),
                nameof(UserController.UniversalCredit)),
            new ForwardTestCase(
                new JourneyState { Nationality = NationalityOption.BritishOrIrishCitizen, UniversalCredit = UniversalCreditOption.Receives },
                c => c.PaidWork(new PaidWorkViewModel { PaidWork = PaidWorkOption.No, ReturnTo = ReturnTo.CheckAnswers }),
                nameof(SummaryController.CheckAnswers)),
            new ForwardTestCase(
                new JourneyState { Nationality = NationalityOption.BritishOrIrishCitizen },
                c => c.PaidWork(new PaidWorkViewModel { PaidWork = PaidWorkOption.OnLeave }),
                nameof(UserController.TypeOfLeave)),
            new ForwardTestCase(
                new JourneyState { Nationality = NationalityOption.BritishOrIrishCitizen },
                c => c.PaidWork(new PaidWorkViewModel { PaidWork = PaidWorkOption.OnLeave, ReturnTo = ReturnTo.CheckAnswers }),
                nameof(UserController.TypeOfLeave)),
            // n.b. - it's not possible to answer the type of leave question, so we can't build that scenario.
        ];
    }

    [Theory]
    [MemberData(nameof(Validity))]
    public void TestValidity(ValidityTestCase validityTestCase)
    {
        var journeyState = validityTestCase.Arrange;
        var journeySession = Substitute.For<IJourneySession>();
        var controller = new UserController(journeyState, journeySession);
        var result = validityTestCase.Act.Compile()(controller);
        
        var isRedirectToStart = result is RedirectToActionResult redirectResult
            && redirectResult.ActionName == nameof(HomeController.Start)
            && redirectResult.ControllerName == "Home";
        Assert.Equal(validityTestCase.ShouldRedirect, isRedirectToStart);
    }

    [Theory]
    [MemberData(nameof(Backwards))]
    public void TestBackwards(BackwardTestCase backwardTestCase)
    {
        var journeyState = backwardTestCase.Arrange;
        var journeySession = Substitute.For<IJourneySession>();
        var controller = new UserController(journeyState, journeySession);
        var result = backwardTestCase.Act.Compile()(controller);
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(backwardTestCase.ExpectedAction, viewResult.ViewData["BackLinkAction"]);
    }

    [Theory]
    [MemberData(nameof(Forwards))]
    public void TestForwards(ForwardTestCase forwardTestCase)
    {
        var journeyState = forwardTestCase.Arrange;
        var journeySession = Substitute.For<IJourneySession>();
        var controller = new UserController(journeyState, journeySession);
        var result = forwardTestCase.Act.Compile()(controller);
        var viewResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(forwardTestCase.ExpectedAction, viewResult.ActionName);
    }

    public record ValidityTestCase(JourneyState Arrange, Expression<Func<UserController, IActionResult>> Act, bool ShouldRedirect)
    {
        public override string ToString()
        {
            var redirectDescription = ShouldRedirect ? "redirects to start" : "does not redirect to start";
            return $"When {GetJourneyStateDescription(Arrange)} then {Act.Body.ToString()} {redirectDescription}";
        }
    }

    public record BackwardTestCase(JourneyState Arrange, Expression<Func<UserController, IActionResult>> Act, string ExpectedAction)
    {
        public BackwardTestCase(Expression<Func<UserController, IActionResult>> act, string expectedAction)
            : this(new JourneyState(), act, expectedAction)
        {
        }

        public override string ToString() => $"When {GetJourneyStateDescription(Arrange)} then {Act.Body.ToString()} returns {ExpectedAction}";
    }

    public record ForwardTestCase(JourneyState Arrange, Expression<Func<UserController, IActionResult>> Act, string ExpectedAction)
    {
        public ForwardTestCase(Expression<Func<UserController, IActionResult>> act, string expectedAction)
            : this(new JourneyState(), act, expectedAction)
        {
        }
        public override string ToString() => $"When {GetJourneyStateDescription(Arrange)} then {Act.Body.ToString()} returns {ExpectedAction}";
    }

    private static string GetJourneyStateDescription(JourneyState journeyState)
    {
        var properties = typeof(JourneyState)
                .GetProperties()
                .Select(p => (Property: p.Name, Value: p.GetValue(journeyState)))
                .Where(x =>
                {
                    if (x.Value == null)
                    {
                        return false;
                    }

                    if (x.Value is ICollection collection)
                    {
                        if (collection.Count < 1)
                        {
                            return false;
                        }
                    }

                    return true;
                })
                .Where(x => x.Value is not null)
                .Select(x => $"{x.Property}={x.Value}");

        return string.Join(", ", properties);
    }
}
