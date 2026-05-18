using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;

public class ChildRelationshipViewModel : IValidatableObject
{
    public string? ReturnTo { get; set; }

    public ChildRelationshipViewModel()
    {
        ChildId = string.Empty;
    }

    public ChildRelationshipViewModel(Child child)
    {
        ChildId = child.ChildId;
        ChildName = child.Name;
        Relationship = child.BornRelationship;
    }

    public string ChildId { get; set; }

    [BindNever]
    public string ChildName { get; set; } = string.Empty;

    [Display(Name = "What is your relationship to {0}?")]
    public Relationship? Relationship { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var journeyState = validationContext.GetService(typeof(JourneyState)) as JourneyState;
        var child = (journeyState?.GetChild(ChildId)) ?? throw new InvalidOperationException($"No child found with ID {ChildId}");
        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(ChildRelationshipViewModel));
        if (!Relationship.HasValue)
        {
            yield return new ValidationResult(localizer["Select your relationship to {0}", child.Name], [nameof(Relationship)]);
        }
    }
}
