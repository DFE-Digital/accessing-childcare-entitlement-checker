using AccessingChildcareEntitlementChecker.Web.Services;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Children;

public class ChildBirthDateViewModel : IValidatableObject
{
    public string? ReturnTo { get; set; }

    public ChildBirthDateViewModel()
    {
        ChildId = string.Empty;
    }

    public ChildBirthDateViewModel(ChildState child)
    {
        ChildId = child.ChildId;
        ChildName = child.Name;
        ChildBirthDate = child.BirthDate;
    }

    public string ChildId { get; set; }

    [BindNever]
    public string ChildName { get; set; } = string.Empty;

    [Display(Name = "What is {0}'s date of birth?", Description = "For example, 31 3 2026")]
    [Required(ErrorMessage = "Enter this child's date of birth")]
    [DateInput(ErrorMessagePrefix = "The date of birth")]
    public DateOnly? ChildBirthDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(ChildBirthDateViewModel));
        var todayFactory = validationContext.GetService(typeof(ITodayFactory)) as ITodayFactory;
        var today = todayFactory!.Today;
        if (ChildBirthDate.HasValue && ChildBirthDate.Value > today)
        {
            yield return new ValidationResult(localizer["Enter a date of birth in the past"], [nameof(ChildBirthDate)]);
        }
    }
}
