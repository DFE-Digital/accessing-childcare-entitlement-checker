using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models;

//TODO: Move to core project
public enum CountryOfResidence
{
    England,

    Scotland,

    Wales,

    NorthernIreland
}

public class WhereDoYouLiveViewModel
{
    [Required(ErrorMessage = "Country_Required")]
    [Display(Name = "Country_Label")]
    public CountryOfResidence? Country { get; set; }
}
