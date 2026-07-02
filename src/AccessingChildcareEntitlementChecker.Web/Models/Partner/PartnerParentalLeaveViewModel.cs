using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.Partner;

public class PartnerParentalLeaveViewModel : IValidatableObject
{
    public const string NoneSelectedValue = "None";

    public PartnerParentalLeaveViewModel()
    {
        BackLink = string.Empty;
    }

    public PartnerParentalLeaveViewModel(JourneyState journeyState, string backLink, string? returnTo = null)
    {
        PartnerParentalLeaveChildrenIds = journeyState.PartnerParentalLeaveChildrenIds;
        Children = journeyState.Children.Values.ToList();
        BackLink = backLink;
        ReturnTo = returnTo;
    }

    [BindNever]
    public string BackLink { get; set; }

    public string? ReturnTo { get; set; }

    [BindNever]
    public List<Child> Children { get; set; } = [];

    [Display(Name = "Which child is your partner on leave for?", Description = "Select all that apply.")]
    public List<string> PartnerParentalLeaveChildrenIds { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(PartnerParentalLeaveViewModel));

        var isEmpty = PartnerParentalLeaveChildrenIds.Count == 0;
        var isNoneSelectedWithOtherOption = PartnerParentalLeaveChildrenIds.Count > 1 && PartnerParentalLeaveChildrenIds.Contains(NoneSelectedValue);
        if (isEmpty || isNoneSelectedWithOtherOption)
        {
            yield return new ValidationResult(localizer["Select which child your partner is on leave for, or 'None of these children'"], [nameof(PartnerParentalLeaveChildrenIds)]);
        }
    }
}
