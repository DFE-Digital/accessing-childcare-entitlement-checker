using System.ComponentModel.DataAnnotations;

namespace Dfe.Acec.Web.Models.Partner;

public enum PartnerSettledStatusOption
{
    [Display(Name = "Yes")]
    Yes,

    [Display(Name = "No")]
    No,

    [Display(Name = "They applied before 1 July 2021 and are still waiting for a decision")]
    StillWaiting,
}
