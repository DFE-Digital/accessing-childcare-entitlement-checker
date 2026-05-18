using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics.CodeAnalysis;

namespace AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails
{
    [ExcludeFromCodeCoverage(Justification = "To be covered by future pages")]
    public class CheckChildDetailsViewModel
    {
        public CheckChildDetailsViewModel(JourneyState journeyState)
        {
            if (journeyState.ChildName == null)
            {
                throw new ArgumentNullException(
                        nameof(journeyState),
                        $"{nameof(journeyState.ChildName)} must not be null.");
            }

            ChildName = journeyState.ChildName;
        }

        [BindNever]
        public string ChildName { get; set; } = string.Empty;
    }
}
