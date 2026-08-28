using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.Web.Models.Results;

public class ResultsSummaryViewModel
{
    public List<ChildResultsViewModel> Children { get; set; } = [];

    public bool HasAccessToPublicFunds { get; set; }
}

public class ChildResultsViewModel
{
    public string ChildId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool ShowThirtyHourWarning { get; set; }
    public List<SchemeResultsViewModel> Schemes { get; set; } = [];
}

public class SchemeResultsViewModel
{
    public SchemeCode SchemeCode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string WhatYouGet { get; set; } = string.Empty;
    public string WhenToApply { get; set; } = string.Empty;
}