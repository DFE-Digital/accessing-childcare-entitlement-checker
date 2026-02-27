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
    public CountryOfResidence? Country { get; set; }
}