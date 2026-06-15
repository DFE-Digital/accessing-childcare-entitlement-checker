using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Collections;
using System.Linq.Expressions;

namespace AccessingChildcareEntitlementChecker.UnitTests.Services;

public class NavigationServiceTests
{
    public static TheoryData<ValidityTestCase> Validity;

    public static TheoryData<BackwardTestCase> Backwards { get; }

    public static TheoryData<ForwardTestCase> Forwards { get; }

    static NavigationServiceTests()
    {
        var withoutChildren = new Dictionary<string, Child>();
        var withChildren = new Dictionary<string, Child>
            {
                { "child1", new Child("child1", "Child 1") }
            };

        Validity = [
            new ValidityTestCase(new JourneyState { Children = withoutChildren }, n => n.UserAgeValid(), false),
            new ValidityTestCase(new JourneyState { Children = withChildren }, n => n.UserAgeValid(), true),

            new ValidityTestCase(new JourneyState { UserAge = null }, n => n.NationalityValid(), false),
            new ValidityTestCase(new JourneyState { UserAge = AgeRange.UnderEighteen }, n => n.NationalityValid(), true),

            new ValidityTestCase(new JourneyState { Nationality = null }, n => n.SettledStatusValid(), false),
            new ValidityTestCase(new JourneyState { Nationality = NationalityOption.BritishOrIrishCitizen }, n => n.SettledStatusValid(), false),
            new ValidityTestCase(new JourneyState { Nationality = NationalityOption.CitizenOfADifferentCountry }, n => n.SettledStatusValid(), false),
            new ValidityTestCase(new JourneyState { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland }, n => n.SettledStatusValid(), true),

            new ValidityTestCase(new JourneyState { Nationality = null }, n => n.PaidWorkValid(), false),
            new ValidityTestCase(new JourneyState { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatus = null }, n => n.PaidWorkValid(), false),
            new ValidityTestCase(new JourneyState { Nationality = NationalityOption.BritishOrIrishCitizen }, n => n.PaidWorkValid(), true),
            new ValidityTestCase(new JourneyState { Nationality = NationalityOption.CitizenOfADifferentCountry }, n => n.PaidWorkValid(), true),
            new ValidityTestCase(new JourneyState { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatus = SettledStatusOption.Yes }, n => n.PaidWorkValid(), true),
        ];

        Backwards = [
            new BackwardTestCase(n => n.UserAgePrevious(null), nameof(SummaryController.CheckChildDetails)),
            new BackwardTestCase(n => n.UserAgePrevious(ReturnTo.CheckAnswers), nameof(SummaryController.CheckAnswers)),

            new BackwardTestCase(n => n.NationalityPrevious(null), nameof(UserController.UserAge)),
            new BackwardTestCase(n => n.NationalityPrevious(ReturnTo.CheckAnswers), nameof(SummaryController.CheckAnswers)),

            new BackwardTestCase(n => n.SettledStatusPrevious(null), nameof(UserController.Nationality)),
            new BackwardTestCase(n => n.SettledStatusPrevious(ReturnTo.CheckAnswers), nameof(SummaryController.CheckAnswers)),

            new BackwardTestCase(n => n.PaidWorkPrevious(null), nameof(UserController.Nationality)),
            new BackwardTestCase(new JourneyState { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland }, n => n.PaidWorkPrevious(null), nameof(UserController.SettledStatus)),
            new BackwardTestCase(n => n.PaidWorkPrevious(ReturnTo.CheckAnswers), nameof(SummaryController.CheckAnswers)),
        ];

        Forwards = [
            new ForwardTestCase(n => n.UserAgeNext(null), nameof(UserController.Nationality)),
            new ForwardTestCase(n => n.UserAgeNext(ReturnTo.CheckAnswers), nameof(SummaryController.CheckAnswers)),

            new ForwardTestCase(n => n.NationalityNext(null), nameof(UserController.PaidWork)),
            new ForwardTestCase(new JourneyState { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland }, n => n.NationalityNext(null), nameof(UserController.SettledStatus)),
            new ForwardTestCase(n => n.NationalityNext(ReturnTo.CheckAnswers), nameof(UserController.PaidWork)),
            new ForwardTestCase(new JourneyState { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland }, n => n.NationalityNext(ReturnTo.CheckAnswers), nameof(UserController.SettledStatus)),
            new ForwardTestCase(new JourneyState { PaidWork = PaidWorkOption.Yes }, n => n.NationalityNext(ReturnTo.CheckAnswers), nameof(SummaryController.CheckAnswers)),
            new ForwardTestCase(new JourneyState { Nationality = NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, SettledStatus = SettledStatusOption.Yes }, n => n.NationalityNext(ReturnTo.CheckAnswers), nameof(SummaryController.CheckAnswers)),

            new ForwardTestCase(n => n.SettledStatusNext(null), nameof(UserController.PaidWork)),
            new ForwardTestCase(n => n.SettledStatusNext(ReturnTo.CheckAnswers), nameof(UserController.PaidWork)),
            new ForwardTestCase(new JourneyState { PaidWork = PaidWorkOption.Yes }, n => n.SettledStatusNext(ReturnTo.CheckAnswers), nameof(SummaryController.CheckAnswers)),

            new ForwardTestCase(new JourneyState { PaidWork = PaidWorkOption.Yes }, n => n.PaidWorkNext(null), nameof(UserController.WorkStatus)),
            new ForwardTestCase(new JourneyState { PaidWork = PaidWorkOption.No }, n => n.PaidWorkNext(null), nameof(UserController.UniversalCredit)),
            new ForwardTestCase(new JourneyState { PaidWork = PaidWorkOption.OnLeave }, n => n.PaidWorkNext(null), nameof(UserController.TypeOfLeave)),
            new ForwardTestCase(new JourneyState { PaidWork = PaidWorkOption.Yes }, n => n.PaidWorkNext(ReturnTo.CheckAnswers), nameof(UserController.WorkStatus)),
            new ForwardTestCase(new JourneyState { PaidWork = PaidWorkOption.No }, n => n.PaidWorkNext(ReturnTo.CheckAnswers), nameof(UserController.UniversalCredit)),
            new ForwardTestCase(new JourneyState { PaidWork = PaidWorkOption.OnLeave }, n => n.PaidWorkNext(ReturnTo.CheckAnswers), nameof(UserController.TypeOfLeave)), // n.b. no corresponding state yet
            new ForwardTestCase(new JourneyState { PaidWork = PaidWorkOption.Yes, WorkStatus = [WorkStatusOption.SelfEmployed] }, n => n.PaidWorkNext(ReturnTo.CheckAnswers), nameof(SummaryController.CheckAnswers)),
            new ForwardTestCase(new JourneyState { PaidWork = PaidWorkOption.No, UniversalCredit = UniversalCreditOption.Receives }, n => n.PaidWorkNext(ReturnTo.CheckAnswers), nameof(SummaryController.CheckAnswers)),
        ];
    }

    [Theory]
    [MemberData(nameof(Validity))]
    public void TestValidity(ValidityTestCase validityTestCase)
    {
        var navigationService = new NavigationService(validityTestCase.Arrange);
        var result = validityTestCase.Act.Compile()(navigationService);
        Assert.Equal(validityTestCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(Backwards))]
    public void TestBackwards(BackwardTestCase backwardTestCase)
    {
        var navigationService = new NavigationService(backwardTestCase.Arrange);
        var result = backwardTestCase.Act.Compile()(navigationService);
        Assert.Equal(backwardTestCase.ExpectedAction, result.Action);
    }

    [Theory]
    [MemberData(nameof(Forwards))]
    public void TestForwards(ForwardTestCase forwardTestCase)
    {
        var navigationService = new NavigationService(forwardTestCase.Arrange);
        var result = forwardTestCase.Act.Compile()(navigationService);
        Assert.Equal(forwardTestCase.ExpectedAction, result.ActionName);
    }

    public record ValidityTestCase(JourneyState Arrange, Expression<Func<NavigationService, bool>> Act, bool Expected)
    {
        public override string ToString() => $"When {GetJourneyStateDescription(Arrange)} then {Act.Body.ToString()} returns {Expected}";
    }

    public record BackwardTestCase(JourneyState Arrange, Expression<Func<NavigationService, ActionLink>> Act, string ExpectedAction)
    {
        public BackwardTestCase(Expression<Func<NavigationService, ActionLink>> act, string expectedAction)
            : this(new JourneyState(), act, expectedAction)
        {
        }

        public override string ToString() => $"When {GetJourneyStateDescription(Arrange)} then {Act.Body.ToString()} returns {ExpectedAction}";
    }

    public record ForwardTestCase(JourneyState Arrange, Expression<Func<NavigationService, RedirectToActionResult>> Act, string ExpectedAction)
    {
        public ForwardTestCase(Expression<Func<NavigationService, RedirectToActionResult>> act, string expectedAction)
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
