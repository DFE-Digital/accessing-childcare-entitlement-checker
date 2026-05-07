using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails
{
    public class ChildBirthDateViewModel : IValidatableObject
    {
        public ChildBirthDateViewModel()
        {

        }

        public ChildBirthDateViewModel(JourneyState journeyState)
        {
            if (journeyState.ChildName == null)
            {
                throw new ArgumentNullException(
                    nameof(journeyState),
                    $"{nameof(journeyState.ChildName)} must not be null.");
            }

            ChildName = journeyState.ChildName;
            ChildBirthDate = journeyState.ChildBirthDate;
        }

        [BindNever]
        public string ChildName { get; set; } = string.Empty;

        [Display(Name = "Label_ChildBirthDate", Description = "Description_ChildBirthDate")]
        [Required(ErrorMessage = "Error_ChildBirthDate")]
        public DateTime? ChildBirthDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var dateTimeFactory = validationContext.GetService(typeof(IDateTimeFactory)) as IDateTimeFactory;
            var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;

            var utcNow = dateTimeFactory!.UtcNow;
            if (ChildBirthDate.HasValue && ChildBirthDate.Value.ToUniversalTime() > utcNow)
            {
                var localizer = localizerFactory!.Create(typeof(ChildBirthDateViewModel));
                var localised = localizer["Error_ChildBirthDateInFuture"];
                yield return new ValidationResult(localised, [nameof(ChildBirthDate)]);
            }
        }
    }
}
