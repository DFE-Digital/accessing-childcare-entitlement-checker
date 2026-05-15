using AccessingChildcareEntitlementChecker.Web.Services;
using GovUk.Frontend.AspNetCore;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.ExpectedChildDetails
{
    public class ChildDueDateViewModel : IValidatableObject
    {
        public ChildDueDateViewModel()
        {

        }

        public ChildDueDateViewModel(JourneyState journeyState)
        {
            ChildDueDate = journeyState.ChildDueDate;
        }

        [Display(Name = "What is this child's due date?", Description = "For example, 30 9 2026")]
        [Required(ErrorMessage = "Enter this child's due date")]
        [DateInput(ErrorMessagePrefix = "The due date")]
        public DateOnly? ChildDueDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var todayFactory = validationContext.GetService(typeof(ITodayFactory)) as ITodayFactory;
            var localizerFactory = validationContext.GetService(typeof(IStringLocalizerFactory)) as IStringLocalizerFactory;

            var today = todayFactory!.Today;
            if (ChildDueDate.HasValue && ChildDueDate.Value <= today)
            {
                var localizer = localizerFactory!.Create(typeof(ChildDueDateViewModel));
                var localised = localizer["Enter a due date in the future"];
                yield return new ValidationResult(localised, [nameof(ChildDueDate)]);
            }
        }
    }
}
