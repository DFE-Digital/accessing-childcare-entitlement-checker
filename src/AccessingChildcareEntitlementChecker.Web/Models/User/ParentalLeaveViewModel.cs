using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public class ParentalLeaveViewModel : IValidatableObject
{
    public const string NoneSelectedValue = "None";

    public ParentalLeaveViewModel()
    {
        BackLink = string.Empty;
    }

    public ParentalLeaveViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        ParentalLeaveChildrenIds = journeyState.ParentalLeaveChildrenIds;
        Children = journeyState.Children.Values.ToList();
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [BindNever]
    public List<Child> Children { get; set; } = [];

    [Display(Name = "Which child are you on leave for?", Description = "Select all that apply.")]
    public List<string> ParentalLeaveChildrenIds { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(ParentalLeaveViewModel));

        var isEmpty = ParentalLeaveChildrenIds.Count == 0;
        var isNoneSelectedWithOption = ParentalLeaveChildrenIds.Count > 1 && ParentalLeaveChildrenIds.Contains(NoneSelectedValue);
        if (isEmpty || isNoneSelectedWithOption)
        {
            yield return new ValidationResult(localizer["Select which child you are on leave for, or 'None of these children'"], [nameof(ParentalLeaveChildrenIds)]);
        }
    }
}
