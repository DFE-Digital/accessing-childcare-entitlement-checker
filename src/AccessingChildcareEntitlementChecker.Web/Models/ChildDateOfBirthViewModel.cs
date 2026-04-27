using AccessingChildcareEntitlementChecker.Web.Services;
using GovUk.Frontend.AspNetCore;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

public class ChildDateOfBirthViewModel : IValidatableObject
{
    public ChildDateOfBirthViewModel()
    {
    }

    public ChildDateOfBirthViewModel(JourneyState journeyState)
    {
        DateOfBirth = journeyState.ChildDateOfBirth;
        HasPartner = journeyState.HasPartner;
    }

    [Required(ErrorMessage = "Error_EnterChildDateOfBirth")]
    [DateInput(ErrorMessagePrefix = "Child's date of birth")]
    public DateTime? DateOfBirth { get; set; }

    public bool? HasPartner { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DateOfBirth.HasValue && DateOfBirth.Value.Date >= DateTime.Today)
        {
            var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
            var errorMessage = localizerFactory?
                .Create(typeof(ChildDateOfBirthViewModel))
                ["Error_ChildDateOfBirthMustBeInPast"].Value
                ?? "Error_ChildDateOfBirthMustBeInPast";

            yield return new ValidationResult(
                errorMessage,
                new[] { nameof(DateOfBirth) });
        }
    }
}
