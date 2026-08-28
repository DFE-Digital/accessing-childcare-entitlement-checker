using System.Diagnostics.CodeAnalysis;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.Web.Models.Results;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class ResultsDetailsViewModel
{
    public string ChildId { get; set; } = string.Empty;
    public string ChildName { get; set; } = string.Empty;
    public List<SchemeSectionViewModel> Sections { get; set; } = [];
    public bool HouseholdHasAccessToPublicFunds { get; set; }
}

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class SchemeSectionViewModel
{
    public SchemeSectionType SectionType { get; set; }
    public bool ShowThirtyHourWarning { get; set; }
    public List<SchemeDetailsViewModel> Schemes { get; set; } = [];
}

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class SchemeDetailsViewModel
{
    public SchemeCode SchemeCode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string WhenToApply { get; set; } = string.Empty;
    public string Starts { get; set; } = string.Empty;
    public string Ends { get; set; } = string.Empty;
    public List<SchemeCode> CanBeUsedWith { get; set; } = [];
}

public enum SchemeSectionType
{
    HelpWithChildcareCosts,
    FundedChildCareHours
}