using System.Diagnostics.CodeAnalysis;
using Dfe.Acec.RulesEngine.Types;

namespace Dfe.Acec.Web.Models.Results;

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class ResultsSummaryViewModel
{
    public List<ChildResultsViewModel> Children { get; set; } = [];

    public bool HasAccessToPublicFunds { get; set; }
}

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class ChildResultsViewModel
{
    public string ChildId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool ShowThirtyHourWarning { get; set; }
    public List<SchemeResultsViewModel> Schemes { get; set; } = [];
}

[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class SchemeResultsViewModel
{
    public SchemeCode SchemeCode { get; set; }
    public string Name { get; set; } = string.Empty;
    public string WhatYouGet { get; set; } = string.Empty;
    public string WhenToApply { get; set; } = string.Empty;
}