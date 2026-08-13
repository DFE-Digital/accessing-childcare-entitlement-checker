using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Validators;
using FluentValidation;

namespace AccessingChildcareEntitlementChecker.UnitTests.Validators;

public class JourneyStateValidatorCheckChildDetailsTests
{
    private readonly JourneyStateValidator validator = new();

    private FluentValidation.Results.ValidationResult Validate(JourneyState journeyState)
    {
        return validator.Validate(
            journeyState,
            options => options.IncludeRuleSets(
                JourneyStateValidator.CheckChildDetailsRuleSet));
    }

    [Fact]
    public void CheckChildDetails_WhenBornChildIsComplete_IsValid()
    {
        var child = new Child("child-1", "Jack")
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = new DateOnly(2020, 1, 1),
            ChildSupportOptions = [ChildSupport.NoneOfTheseApply]
        };

        var journeyState = new JourneyState
        {
            Children =
            {
                [child.ChildId] = child
            }
        };

        var result = Validate(journeyState);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void CheckChildDetails_WhenDueChildIsComplete_IsValid()
    {
        var child = new Child("child-1", "Jack")
        {
            BirthStatus = BirthStatus.Due,
            DueDate = new DateOnly(2027, 1, 1)
        };

        var journeyState = new JourneyState
        {
            Children =
            {
                [child.ChildId] = child
            }
        };

        var result = Validate(journeyState);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void CheckChildDetails_WhenChildNameIsMissing_IsInvalid()
    {
        var child = new Child("child-1", "")
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = new DateOnly(2020, 1, 1),
            ChildSupportOptions = [ChildSupport.NoneOfTheseApply]
        };

        var journeyState = new JourneyState
        {
            Children =
            {
                [child.ChildId] = child
            }
        };

        var result = Validate(journeyState);

        Assert.False(result.IsValid);

        var error = Assert.Single(result.Errors);

        Assert.Equal("child-1", error.CustomState);
    }

    [Fact]
    public void CheckChildDetails_WhenChildBornStatusIsMissing_IsInvalid()
    {
        var child = new Child("child-1", "Jack")
        {
            BirthDate = new DateOnly(2020, 1, 1),
            ChildSupportOptions = [ChildSupport.NoneOfTheseApply]
        };

        var journeyState = new JourneyState
        {
            Children =
            {
                [child.ChildId] = child
            }
        };

        var result = Validate(journeyState);

        Assert.False(result.IsValid);

        var error = Assert.Single(result.Errors);

        Assert.Equal("child-1", error.CustomState);
    }

    [Fact]
    public void CheckChildDetails_WhenChildIsBornAndMissingBirthdate_IsInvalid()
    {
        var child = new Child("child-1", "Jack")
        {
            BirthStatus = BirthStatus.Born,
            ChildSupportOptions = [ChildSupport.NoneOfTheseApply]
        };

        var journeyState = new JourneyState
        {
            Children =
            {
                [child.ChildId] = child
            }
        };

        var result = Validate(journeyState);

        Assert.False(result.IsValid);

        var error = Assert.Single(result.Errors);

        Assert.Equal("child-1", error.CustomState);
    }

    [Fact]
    public void CheckChildDetails_WhenChildIsBornAndMissingSupportOptions_IsInvalid()
    {
        var child = new Child("child-1", "Jack")
        {
            BirthStatus = BirthStatus.Born,
            BirthDate = new DateOnly(2020, 1, 1),

        };

        var journeyState = new JourneyState
        {
            Children =
            {
                [child.ChildId] = child
            }
        };

        var result = Validate(journeyState);

        Assert.False(result.IsValid);

        var error = Assert.Single(result.Errors);

        Assert.Equal("child-1", error.CustomState);
    }

    [Fact]
    public void CheckChildDetails_WhenChildIsNotBornAndMissingDueDate_IsInvalid()
    {
        var child = new Child("child-1", "Jack")
        {
            BirthStatus = BirthStatus.Due,
        };

        var journeyState = new JourneyState
        {
            Children =
            {
                [child.ChildId] = child
            }
        };

        var result = Validate(journeyState);

        Assert.False(result.IsValid);

        var error = Assert.Single(result.Errors);

        Assert.Equal("child-1", error.CustomState);
    }
}


