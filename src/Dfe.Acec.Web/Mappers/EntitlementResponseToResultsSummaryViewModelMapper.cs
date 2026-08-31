using System.Diagnostics;
using Dfe.Acec.RulesEngine.Dtos.Responses;
using Dfe.Acec.RulesEngine.Types;
using Dfe.Acec.Web.Controllers;
using Dfe.Acec.Web.Models.Results;
using Microsoft.Extensions.Localization;

namespace Dfe.Acec.Web.Mappers;

public class EntitlementResponseToResultsSummaryViewModelMapper(
    IStringLocalizerFactory stringLocalizerFactory)
{
    private const string _unknownSchemeCodeMessage = "Unknown scheme code";
    private readonly IStringLocalizer _localizer = stringLocalizerFactory.Create(
            "Views.Results.Results",
            typeof(ResultsController).Assembly.GetName().Name!);

    public ResultsSummaryViewModel Map(EntitlementResponse response) => new()
    {
        Children = [.. response.ChildResults.Select(MapChildResults)],
        HasAccessToPublicFunds = response.HasAccessToPublicFunds,
    };

    private ChildResultsViewModel MapChildResults(ChildResultDto childResult) => new()
    {
        ChildId = childResult.ChildId,
        Name = childResult.ChildName,
        ShowThirtyHourWarning = GetThirtyHourWarning(childResult),
        Schemes = [.. childResult.Schemes
                .OrderBy(s => GetSchemeOrder(s.SchemeCode))
                .Select(s => MapSchemeResult(s, childResult))]
    };

    private SchemeResultsViewModel MapSchemeResult(SchemeResultDto schemeResult, ChildResultDto childResult) => new()
    {
        SchemeCode = schemeResult.SchemeCode,
        Name = GetSchemeName(schemeResult.SchemeCode),
        WhatYouGet = GetSchemeDescription(schemeResult.SchemeCode),
        WhenToApply = GetWhenToApply(schemeResult, childResult)
    };

    private string GetWhenToApply(SchemeResultDto schemeResult, ChildResultDto childResult)
    {
        var now = _localizer["WhenToApply_Now"];

        if (schemeResult.SchemeCode == SchemeCode.TaxFreeChildcare)
        {
            return GetTaxFreeChildcareWhenToApply(schemeResult, childResult);
        }

        if (schemeResult.SchemeCode == SchemeCode.FifteenHoursUniversal)
        {
            return _localizer["WhenToApply_AskProviderOrCouncil"];
        }

        if (schemeResult.SchemeCode == SchemeCode.UniversalCreditChildcare)
        {
            return schemeResult.EligibleNow ? now : _localizer["WhenToApply_WhenBorn"];
        }

        if (schemeResult.SchemeCode == SchemeCode.FifteenHoursForDisadvantagedChildren)
        {
            return schemeResult.EligibleNow ? now : _localizer["WhenToApply_FromDate", schemeResult.ApplyFromDate!.Value];
        }

        if (schemeResult.SchemeCode == SchemeCode.ThirtyHoursForWorkingFamilies)
        {
            return GetThirtyHoursWhenToApply(schemeResult, childResult);
        }

        throw new InvalidOperationException($"Unknown scheme code while mapping GetWhenToApply(): {schemeResult.SchemeCode}");
    }

    private string GetTaxFreeChildcareWhenToApply(SchemeResultDto schemeResult, ChildResultDto child) => schemeResult.ApplyAndStartAffectedByParentalLeave switch
    {
        ParentalLeaveParty.User => _localizer["WhenToApply_TaxFreeChildcare_UserParentalLeave"],

        ParentalLeaveParty.Partner => _localizer["WhenToApply_TaxFreeChildcare_PartnerParentalLeave"],

        ParentalLeaveParty.UserAndPartner => _localizer["WhenToApply_TaxFreeChildcare_UserAndPartnerParentalLeave"],

        null => child.IsBorn
            ? _localizer["WhenToApply_Now"]
            : _localizer["WhenToApply_WhenBorn"],

        _ => throw new UnreachableException(
            $"Unsupported parental leave party while mapping GetTaxFreeChildcareWhenToApply(): " +
            $"{schemeResult.ApplyAndStartAffectedByParentalLeave}")
    };

    private string GetThirtyHoursWhenToApply(SchemeResultDto schemeResult, ChildResultDto child) => schemeResult.ApplyAndStartAffectedByParentalLeave switch
    {
        ParentalLeaveParty.User => _localizer["WhenToApply_ThirtyHours_UserParentalLeave"],

        ParentalLeaveParty.Partner => _localizer["WhenToApply_ThirtyHours_PartnerParentalLeave"],

        ParentalLeaveParty.UserAndPartner => _localizer["WhenToApply_ThirtyHours_UserAndPartnerParentalLeave"],

        null => GetStandardThirtyHoursWhenToApply(schemeResult, child),

        _ => throw new UnreachableException(
            $"Unsupported parental leave party while mapping GetThirtyHoursWhenToApply(): " +
            $"{schemeResult.ApplyAndStartAffectedByParentalLeave}")
    };

    private string GetStandardThirtyHoursWhenToApply(SchemeResultDto schemeResult, ChildResultDto child)
    {
        if (!child.IsBorn)
        {
            return _localizer["WhenToApply_WhenTwentyThreeWeeksOld"];
        }

        var today = DateOnly.FromDateTime(DateTime.Today);

        if (schemeResult.ApplyFromDate!.Value <= today)
        {
            return _localizer["WhenToApply_Now"];
        }

        return _localizer["WhenToApply_FromDate", schemeResult.ApplyFromDate.Value];
    }

    private static bool GetThirtyHourWarning(ChildResultDto childResult)
    {
        var schemes = childResult.Schemes
            .Select(s => s.SchemeCode)
            .ToList();

        return schemes.Contains(SchemeCode.ThirtyHoursForWorkingFamilies) &&
               (
                   schemes.Contains(SchemeCode.FifteenHoursUniversal) ||
                   schemes.Contains(SchemeCode.FifteenHoursForDisadvantagedChildren)
               );

    }

    private static int GetSchemeOrder(SchemeCode schemeCode) => schemeCode switch
    {
        SchemeCode.TaxFreeChildcare => 1,
        SchemeCode.UniversalCreditChildcare => 2,
        SchemeCode.ThirtyHoursForWorkingFamilies => 3,
        SchemeCode.FifteenHoursForDisadvantagedChildren => 4,
        SchemeCode.FifteenHoursUniversal => 5,

        _ => 999
    };

    private string GetSchemeName(SchemeCode schemeCode) => schemeCode switch
    {
        SchemeCode.TaxFreeChildcare => _localizer["TaxFreeChildcare_Name"],
        SchemeCode.FifteenHoursUniversal => _localizer["FifteenHoursUniversal_Name"],
        SchemeCode.FifteenHoursForDisadvantagedChildren => _localizer["FifteenHoursForDisadvantagedChildren_Name"],
        SchemeCode.UniversalCreditChildcare => _localizer["UniversalCreditChildcare_Name"],
        SchemeCode.ThirtyHoursForWorkingFamilies => _localizer["ThirtyHoursForWorkingFamilies_Name"],

        _ => throw UnknownSchemeCode(schemeCode)
    };

    private string GetSchemeDescription(SchemeCode schemeCode) => schemeCode switch
    {
        SchemeCode.TaxFreeChildcare => _localizer["TaxFreeChildcare_Description"],
        SchemeCode.FifteenHoursUniversal => _localizer["FifteenHoursUniversal_Description"],
        SchemeCode.FifteenHoursForDisadvantagedChildren => _localizer["FifteenHoursForDisadvantagedChildren_Description"],
        SchemeCode.UniversalCreditChildcare => _localizer["UniversalCreditChildcare_Description"],
        SchemeCode.ThirtyHoursForWorkingFamilies => _localizer["ThirtyHoursForWorkingFamilies_Description"],

        _ => throw UnknownSchemeCode(schemeCode)
    };

    private static ArgumentOutOfRangeException UnknownSchemeCode(SchemeCode schemeCode) =>
        new(
            nameof(schemeCode),
            schemeCode,
            _unknownSchemeCodeMessage);
}
