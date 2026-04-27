using AccessingChildcareEntitlementChecker.Web.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

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
        Assert.Equal("Jack's date of birth must be in the past", validationError.ErrorMessage);
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
        Assert.Equal("Jack's date of birth must be in the past", validationError.ErrorMessage);
        Assert.Equal(nameof(ChildDateOfBirthViewModel.DateOfBirth), Assert.Single(validationError.MemberNames));
    }

    private static List<ValidationResult> Validate(ChildDateOfBirthViewModel model)
    {
        var originalCulture = CultureInfo.CurrentCulture;
        var originalUiCulture = CultureInfo.CurrentUICulture;

        CultureInfo.CurrentCulture = new CultureInfo("en-GB");
        CultureInfo.CurrentUICulture = new CultureInfo("en-GB");

        var services = new ServiceCollection()
            .AddLogging()
            .AddLocalization(options => options.ResourcesPath = "Resources")
            .BuildServiceProvider();

        try
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, services, items: null);
            Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true);
            return validationResults;
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUiCulture;
        }
    }
}
