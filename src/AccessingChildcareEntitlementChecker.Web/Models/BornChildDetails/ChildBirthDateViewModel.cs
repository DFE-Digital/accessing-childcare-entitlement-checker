using AccessingChildcareEntitlementChecker.Web.Services;
using GovUk.Frontend.AspNetCore;
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

        [Display(Name = "What is the child's date of birth?", Description = "For example, 31 3 2026")]
        [Required(ErrorMessage = "Enter this child's date of birth")]
        [DateInput(ErrorMessagePrefix = "The date of birth")]
        public DateOnly? ChildBirthDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var todayFactory = validationContext.GetService(typeof(ITodayFactory)) as ITodayFactory;
            var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;

            var today = todayFactory!.Today;
            if (ChildBirthDate.HasValue && ChildBirthDate.Value > today)
            {
                var localizer = localizerFactory!.Create(typeof(ChildBirthDateViewModel));
                var localised = localizer["Enter a date of birth in the past"];
                yield return new ValidationResult(localised, [nameof(ChildBirthDate)]);
            }
        }
    }
}
