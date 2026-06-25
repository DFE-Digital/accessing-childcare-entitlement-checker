using AccessingChildcareEntitlementChecker.RulesEngine.Types;

namespace AccessingChildcareEntitlementChecker.Web.Models.Results;

public class ResultsViewModel
{
    public List<ChildResultsViewModel> Children { get; set; } = [];
}

public class ChildResultsViewModel
{
    public string Name { get; set; } = string.Empty;
    public bool ShowThirtyHourWarning { get; set; }

    public bool IsBorn { get; set; }
    public List<SchemeResultsViewModel> Schemes { get; set; } = [];
}

public class SchemeResultsViewModel
{
    public SchemeCode SchemeCode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string WhatYouGet { get; set; } = string.Empty;
    public bool EligibleNow { get; set; }
    public bool EligibleInFuture { get; set; }
    public DateOnly? ApplyFromDate { get; set; }
    public DateOnly? UseFromDate { get; set; }
    public string WhenToApply { get; set; } = string.Empty;
}