using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.User;

public enum CountryOfResidence
{
    [Display(Name = "England")]
    England,

    [Display(Name = "Scotland")]
    Scotland,

    [Display(Name = "Wales")]
    Wales,

    [Display(Name = "Northern Ireland")]
    NorthernIreland
}
