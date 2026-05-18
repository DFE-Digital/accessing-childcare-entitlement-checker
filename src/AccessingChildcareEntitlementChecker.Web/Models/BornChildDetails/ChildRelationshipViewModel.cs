using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using AccessingChildcareEntitlementChecker.Web.Models;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;

public class ChildRelationshipViewModel : IValidatableObject
{
    public string? ReturnTo { get; set; }

    public ChildRelationshipViewModel()
    {
        ChildId = default!;
    }

    public ChildRelationshipViewModel(string? childId, JourneyState journeyState)
    {
        var child = journeyState.GetChild(childId);
        ChildId = childId;
        ChildName = child.Name;
        ChildRelationship = child.BornRelationship;
    }

    public string? ChildId { get; set; }

    [BindNever]
    public string? ChildName { get; set; }

    [Display(Name = "What is your relationship to {0}?")]
    public RelationshipOption? ChildRelationship { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(ChildRelationshipViewModel));
        if (!ChildRelationship.HasValue)
        {
            yield return new ValidationResult(localizer["Select your relationship to {0}", ChildName ?? string.Empty], [nameof(ChildRelationship)]);
        }
    }

}
