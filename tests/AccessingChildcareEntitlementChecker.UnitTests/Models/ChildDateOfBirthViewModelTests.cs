using AccessingChildcareEntitlementChecker.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.UnitTests.Models;

public class ChildDateOfBirthViewModelTests
{
    [Fact]
    public void Validate_WithPastDate_HasNoValidationErrors()
    {
        var model = new ChildDateOfBirthViewModel
        {
            DateOfBirth = DateTime.Today.AddDays(-1)
        };

        var validationResults = Validate(model);

        Assert.Empty(validationResults);
    }

    [Fact]
    public void Validate_WithTodayDate_HasPastDateValidationError()
    {
        var model = new ChildDateOfBirthViewModel
        {
            DateOfBirth = DateTime.Today
        };

        var validationResults = Validate(model);

        var validationError = Assert.Single(validationResults);
        Assert.Equal("Error_ChildDateOfBirthMustBeInPast", validationError.ErrorMessage);
        Assert.Equal(nameof(ChildDateOfBirthViewModel.DateOfBirth), Assert.Single(validationError.MemberNames));
    }

    [Fact]
    public void Validate_WithFutureDate_HasPastDateValidationError()
    {
        var model = new ChildDateOfBirthViewModel
        {
            DateOfBirth = DateTime.Today.AddDays(1)
        };

        var validationResults = Validate(model);

        var validationError = Assert.Single(validationResults);
        Assert.Equal("Error_ChildDateOfBirthMustBeInPast", validationError.ErrorMessage);
        Assert.Equal(nameof(ChildDateOfBirthViewModel.DateOfBirth), Assert.Single(validationError.MemberNames));
    }

    private static List<ValidationResult> Validate(ChildDateOfBirthViewModel model)
    {
        var validationResults = new List<ValidationResult>();
        Validator.TryValidateObject(model, new ValidationContext(model), validationResults, validateAllProperties: true);
        return validationResults;
    }
}
