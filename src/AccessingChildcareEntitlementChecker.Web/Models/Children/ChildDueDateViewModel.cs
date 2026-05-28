using AccessingChildcareEntitlementChecker.Web.Services;
using GovUk.Frontend.AspNetCore;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Children;

public class ChildDueDateViewModel : IValidatableObject
{
    public string? ReturnTo { get; set; }

    public ChildDueDateViewModel()
    {
        ChildId = Guid.Empty;
    }

    public ChildDueDateViewModel(ChildState child)
    {
        ChildId = child.ChildId;
        ChildDueDate = child.DueDate;
    }

    public Guid ChildId { get; set; }

    [Display(Name = "What is this child's due date?", Description = "For example, 30 9 2026")]
    [Required(ErrorMessage = "Enter this child's due date")]
    [DateInput(ErrorMessagePrefix = "The due date")]
    public DateOnly? ChildDueDate { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(ChildDueDateViewModel));
        var todayFactory = validationContext.GetService(typeof(ITodayFactory)) as ITodayFactory;
        var today = todayFactory!.Today;
        if (ChildDueDate.HasValue && ChildDueDate.Value <= today)
        {
            yield return new ValidationResult(localizer["Enter a due date in the future"], [nameof(ChildDueDate)]);
        }
    }
}
