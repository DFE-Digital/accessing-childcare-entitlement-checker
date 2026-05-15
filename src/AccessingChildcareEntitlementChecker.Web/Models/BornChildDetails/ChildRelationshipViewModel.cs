using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;

public class ChildRelationshipViewModel : IValidatableObject
{
    public ChildRelationshipViewModel()
    {

    }

    public ChildRelationshipViewModel(JourneyState journeyState)
    {
        if (journeyState.ChildName == null)
        {
            throw new ArgumentNullException(
                    nameof(journeyState),
                    $"{nameof(journeyState.ChildName)} must not be null.");
        }

        ChildName = journeyState.ChildName;
        Relationship = journeyState.Relationship;
    }

    [BindNever]
    public string ChildName { get; set; } = string.Empty;

    [Display(Name = "What is your relationship to the child?")]
    public Relationship? Relationship { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!Relationship.HasValue)
        {
            var journeyState = validationContext.GetService(typeof(JourneyState)) as JourneyState;
            var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
            var localizer = localizerFactory!.Create(typeof(ChildRelationshipViewModel));
            yield return new ValidationResult(
                localizer["Select your relationship to {0}", journeyState!.ChildName!],
                [nameof(Relationship)]);
        }
    }
}
