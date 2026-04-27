using AccessingChildcareEntitlementChecker.Web.Services;
using GovUk.Frontend.AspNetCore;
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
            yield return new ValidationResult(
                "Error_ChildDateOfBirthMustBeInPast",
                new[] { nameof(DateOfBirth) });
        }
    }
}
