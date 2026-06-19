using AccessingChildcareEntitlementChecker.Web.Services;
using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.ExpectedChildDetails;

public class ChildDueDateViewModel : IValidatableObject
{
    public ChildDueDateViewModel()
    {
        ChildId = string.Empty;
        BackLink = string.Empty;
    }

    public ChildDueDateViewModel(Child child, string backLink, string? returnTo = null)
    {
        ChildId = child.ChildId;
        ChildDueDate = child.DueDate;
        ChildName = child.Name;
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    public string ChildId { get; set; }

    [BindNever]
    public string ChildName { get; set; } = string.Empty;

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
