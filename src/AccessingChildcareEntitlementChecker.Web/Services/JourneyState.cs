using AccessingChildcareEntitlementChecker.Web.Models;

namespace AccessingChildcareEntitlementChecker.Web.Services;

public class JourneyState
{
    public CountryOfResidence? CountryOfResidence { get; set; }

    public bool? HasPartner { get; set; }

    public Age? Age { get; set; }
}