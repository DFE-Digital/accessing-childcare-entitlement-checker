using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails
{
    public class ChildSupportViewModel : IValidatableObject
    {
        public ChildSupportViewModel()
        {

        }

    public class ChildSupportViewModel
    {
        public ChildSupportViewModel(JourneyState journeyState)
        {
            if (journeyState.ChildName == null)
            {
                throw new ArgumentNullException(
                        nameof(journeyState),
                        $"{nameof(journeyState.ChildName)} must not be null.");
            }

            ChildName = journeyState.ChildName;
            ChildSupportOptions = journeyState.ChildSupportOptions;
        }

        [BindNever]
        public string ChildName { get; set; } = string.Empty;

        [Display(Name = "Does the child get any of the following support?", Description = "Select all that apply")]
        public List<ChildSupport> ChildSupportOptions { get; set; } = new List<ChildSupport>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var journeyState = validationContext.GetService(typeof(JourneyState)) as JourneyState;
            var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;
            var localizer = localizerFactory!.Create(typeof(ChildSupportViewModel));
            if (ChildSupportOptions.Contains(ChildSupport.NoneOfTheseApply) && ChildSupportOptions.Count > 1)
            {
                var localised = localizer["You may not select 'no, none of these apply' with other options"];
                yield return new ValidationResult(localised, [nameof(ChildSupportOptions)]);
            }

            if (ChildSupportOptions.Count == 0)
            {
                var localised = localizer["Select any support {0} gets, or select 'No, none of these apply'", journeyState!.ChildName!];
                yield return new ValidationResult(localised, [nameof(ChildSupportOptions)]);
            }
        }
    }
}
