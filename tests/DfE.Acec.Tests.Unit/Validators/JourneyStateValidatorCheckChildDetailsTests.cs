using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.BornChildDetails;
using Dfe.Acec.Web.Services;
using Dfe.Acec.Web.Validators;
using FluentValidation;

namespace Dfe.Acec.Tests.Unit.Validators;

public class JourneyStateValidatorCheckChildDetailsTests
{
    private readonly JourneyStateValidator _validator = new();

    private FluentValidation.Results.ValidationResult Validate(JourneyState journeyState)
    {
        return _validator.Validate(
            journeyState,
            options => options.IncludeRuleSets(
                JourneyStateValidator.CheckChildDetailsRuleSet));
    }

    [Fact]
    public void CheckChildDetailsWhenBornChildIsCompleteIsValid()
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
    public void CheckChildDetailsWhenDueChildIsCompleteIsValid()
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
    public void CheckChildDetailsWhenChildNameIsMissingIsInvalid()
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
    public void CheckChildDetailsWhenChildBornStatusIsMissingIsInvalid()
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
    public void CheckChildDetailsWhenChildIsBornAndMissingBirthdateIsInvalid()
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
    public void CheckChildDetailsWhenChildIsBornAndMissingSupportOptionsIsInvalid()
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
    public void CheckChildDetailsWhenChildIsNotBornAndMissingDueDateIsInvalid()
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


