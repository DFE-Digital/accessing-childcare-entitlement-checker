using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;

public class ChildRelationshipViewModel : IValidatableObject
{
    public ChildRelationshipViewModel()
    {
        ChildId = string.Empty;
        BackLink = string.Empty;
    }

    public ChildRelationshipViewModel(Child child, string backLink, string? returnTo = null)
    {
        ChildId = child.ChildId;
        Relationship = child.BornRelationship;
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

    [Display(Name = "What is your relationship to {0}?")]
    public Relationship? Relationship { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var journeyState = validationContext.GetService(typeof(JourneyState)) as JourneyState;
        if (!journeyState!.Children.TryGetValue(ChildId, out var child))
        {
            throw new InvalidOperationException($"No child found with ID {ChildId}");
        }

        var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
        var localizer = localizerFactory!.Create(typeof(ChildRelationshipViewModel));
        if (!Relationship.HasValue)
        {
            yield return new ValidationResult(localizer["Select your relationship to {0}", child.Name], [nameof(Relationship)]);
        }
    }
}
