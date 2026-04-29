using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models
{
    public enum CountryOfResidence
    {
        [Display(Name = "Option_England")]
        England,

        [Display(Name = "Option_Scotland")]
        Scotland,

        [Display(Name = "Option_Wales")]
        Wales,

        [Display(Name = "Option_NorthernIreland")]
        NorthernIreland
    }
}
