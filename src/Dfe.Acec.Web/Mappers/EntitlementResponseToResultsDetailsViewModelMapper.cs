using System.Diagnostics;
using Dfe.Acec.RulesEngine.Dtos.Responses;
using Dfe.Acec.RulesEngine.Helpers;
using Dfe.Acec.RulesEngine.Types;
using Dfe.Acec.Web.Controllers;
using Dfe.Acec.Web.Models.Results;
using Microsoft.Extensions.Localization;

namespace Dfe.Acec.Web.Mappers;

public class EntitlementResponseToResultsDetailsViewModelMapper(
    IStringLocalizerFactory stringLocalizerFactory)
{
    private const string _unknownSchemeCodeMessage = "Unknown scheme code";
    private const string _startsNowKey = "Starts_Now";
    private readonly IStringLocalizer _localizer = stringLocalizerFactory.Create(
            "Views.Results.ResultsDetailed",
            typeof(ResultsController).Assembly.GetName().Name!);

    public ResultsDetailsViewModel Map(ChildResultDto child, bool householdHasAccessToPublicFunds) => new()
    {
        ChildId = child.ChildId,
        ChildName = child.ChildName,
        Sections = GetSections(child),
        HouseholdHasAccessToPublicFunds = householdHasAccessToPublicFunds,
    };

    private List<SchemeSectionViewModel> GetSections(ChildResultDto child)
    {
        var sections = new List<SchemeSectionViewModel>();

        var helpWithCosts = child.Schemes
            .Where(x =>
                x.SchemeCode is SchemeCode.TaxFreeChildcare or
                SchemeCode.UniversalCreditChildcare)
            .ToList();

        if (helpWithCosts.Count > 0)
        {
            sections.Add(new SchemeSectionViewModel
            {
                SectionType = SchemeSectionType.HelpWithChildcareCosts,
                ShowThirtyHourWarning = false,
                Schemes = [.. helpWithCosts
                    .OrderBy(s => GetSchemeOrder(s.SchemeCode))
                    .Select(x => MapSchemeResult(x, child))]
            });
        }

        var fundedHours = child.Schemes
            .Where(x =>
                x.SchemeCode is SchemeCode.ThirtyHoursForWorkingFamilies or
                SchemeCode.FifteenHoursForDisadvantagedChildren or
                SchemeCode.FifteenHoursUniversal)
            .ToList();

        var showWarning =
            child.Schemes.Any(x => x.SchemeCode == SchemeCode.ThirtyHoursForWorkingFamilies) &&
            (
                child.Schemes.Any(x => x.SchemeCode == SchemeCode.FifteenHoursUniversal) ||
                child.Schemes.Any(x => x.SchemeCode == SchemeCode.FifteenHoursForDisadvantagedChildren)
            );


        if (fundedHours.Count > 0)
        {
            sections.Add(new SchemeSectionViewModel
            {
                SectionType = SchemeSectionType.FundedChildCareHours,
                ShowThirtyHourWarning = showWarning,
                Schemes = [.. fundedHours
                    .OrderBy(s => GetSchemeOrder(s.SchemeCode))
                    .Select(x => MapSchemeResult(x, child))]
            });
        }

        return sections;
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

    private SchemeDetailsViewModel MapSchemeResult(
        SchemeResultDto schemeResult,
        ChildResultDto child) => new()
        {
            SchemeCode = schemeResult.SchemeCode,
            Name = GetSchemeName(schemeResult.SchemeCode),
            Url = GetSchemeUrl(schemeResult.SchemeCode),
            WhenToApply = GetWhenToApply(schemeResult, child),
            Starts = GetStarts(schemeResult, child),
            Ends = GetEnds(schemeResult, child),
            CanBeUsedWith = GetCanBeUsedWith(schemeResult, child)
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

    private static string GetSchemeUrl(SchemeCode schemeCode) => schemeCode switch
    {
        SchemeCode.TaxFreeChildcare => "https://www.gov.uk/tax-free-childcare",
        SchemeCode.FifteenHoursUniversal =>
            "https://www.gov.uk/help-with-childcare-costs/free-childcare-and-education-for-3-to-4-year-olds",
        SchemeCode.FifteenHoursForDisadvantagedChildren =>
            "https://www.gov.uk/help-with-childcare-costs/free-childcare-2-year-olds-extra-support",
        SchemeCode.UniversalCreditChildcare => "https://www.gov.uk/help-with-childcare-costs/universal-credit",
        SchemeCode.ThirtyHoursForWorkingFamilies => "https://www.gov.uk/free-childcare-if-working",

        _ => throw UnknownSchemeCode(schemeCode)
    };

    private string GetWhenToApply(SchemeResultDto schemeResult, ChildResultDto child) => schemeResult.SchemeCode switch
    {
        SchemeCode.TaxFreeChildcare => GetTaxFreeChildcareWhenToApply(schemeResult, child),

        SchemeCode.UniversalCreditChildcare => child.IsBorn
            ? _localizer["WhenToApply_Now"]
            : _localizer["WhenToApply_WhenBorn"],

        SchemeCode.FifteenHoursUniversal => _localizer["WhenToApply_AskProviderOrCouncil"],

        SchemeCode.FifteenHoursForDisadvantagedChildren => schemeResult.EligibleNow
            ? _localizer["WhenToApply_Now"]
            : _localizer[
                "WhenToApply_FromDate",
                schemeResult.ApplyFromDate
                ?? throw new InvalidOperationException(
                    $"{schemeResult.SchemeCode} must have an ApplyFromDate.")
            ],

        SchemeCode.ThirtyHoursForWorkingFamilies => GetThirtyHoursWhenToApply(schemeResult, child),

        _ => throw new InvalidOperationException($"{_unknownSchemeCodeMessage} {schemeResult.SchemeCode}")
    };

    private string GetTaxFreeChildcareWhenToApply(SchemeResultDto schemeResult, ChildResultDto child) => schemeResult.ApplyAndStartAffectedByParentalLeave switch
    {
        ParentalLeaveParty.User => _localizer["WhenToApply_TaxFreeChildcare_UserParentalLeave"],

        ParentalLeaveParty.Partner => _localizer["WhenToApply_TaxFreeChildcare_PartnerParentalLeave"],

        ParentalLeaveParty.UserAndPartner => _localizer["WhenToApply_TaxFreeChildcare_UserAndPartnerParentalLeave"],

        null => child.IsBorn ? _localizer["WhenToApply_Now"] : _localizer["WhenToApply_WhenBorn"],

        _ => throw InvalidParentalLeaveParty(
            schemeResult.ApplyAndStartAffectedByParentalLeave,
            nameof(GetTaxFreeChildcareWhenToApply))
    };

    private string GetThirtyHoursWhenToApply(SchemeResultDto schemeResult, ChildResultDto child) => schemeResult.ApplyAndStartAffectedByParentalLeave switch
    {
        ParentalLeaveParty.User => _localizer["WhenToApply_ThirtyHours_UserParentalLeave"],

        ParentalLeaveParty.Partner => _localizer["WhenToApply_ThirtyHours_PartnerParentalLeave"],

        ParentalLeaveParty.UserAndPartner => _localizer["WhenToApply_ThirtyHours_UserAndPartnerParentalLeave"],

        null => GetStandardThirtyHoursWhenToApply(schemeResult, child),

        _ => throw InvalidParentalLeaveParty(
            schemeResult.ApplyAndStartAffectedByParentalLeave,
            nameof(GetThirtyHoursWhenToApply))
    };

    private string GetStandardThirtyHoursWhenToApply(SchemeResultDto schemeResult, ChildResultDto child)
    {
        if (!child.IsBorn)
        {
            return _localizer["WhenToApply_WhenTwentyThreeWeeksOld"];
        }

        var today = DateOnly.FromDateTime(DateTime.Today);

        var applyFrom = schemeResult.ApplyFromDate
                        ?? throw new InvalidOperationException(
                            $"{schemeResult.SchemeCode} must have an ApplyFromDate.");

        var useFrom = schemeResult.UseFromDate
                      ?? throw new InvalidOperationException(
                          $"{schemeResult.SchemeCode} must have a UseFromDate.");


        if (applyFrom <= today && useFrom > today)
        {
            return _localizer["WhenToApply_ByDate", GetApplicationWindowEnd(useFrom)];
        }

        // Advance to the next relevant application window
        while (GetApplicationWindowEnd(useFrom) < today)
        {
            useFrom = TermDateCalculator.GetNextTermStartDate(useFrom);
        }

        var windowStart = GetApplicationWindowStart(useFrom);
        var windowEnd = GetApplicationWindowEnd(useFrom);

        if (today >= windowStart)
        {
            return _localizer["WhenToApply_ByDate", windowEnd];
        }

        // Child is less than 23 weeks old
        if (applyFrom >= today)
        {
            return _localizer["WhenToApply_FromToDate", applyFrom, windowEnd];
        }

        return _localizer["WhenToApply_FromToDate", windowStart, windowEnd];
    }

    private static DateOnly GetApplicationWindowStart(DateOnly useFromDate) => useFromDate.Month switch
    {
        1 => new DateOnly(useFromDate.Year - 1, 9, 1),
        4 => new DateOnly(useFromDate.Year, 1, 1),
        9 => new DateOnly(useFromDate.Year, 4, 1),

        _ => throw new InvalidOperationException($"Unexpected term start date: {useFromDate}")
    };

    private static DateOnly GetApplicationWindowEnd(DateOnly useFromDate) => useFromDate.Month switch
    {
        1 => new DateOnly(useFromDate.Year - 1, 12, 31),
        4 => new DateOnly(useFromDate.Year, 3, 31),
        9 => new DateOnly(useFromDate.Year, 8, 31),

        _ => throw new InvalidOperationException($"Unexpected term start date: {useFromDate}")
    };

    private string GetStarts(SchemeResultDto schemeResult, ChildResultDto child)
    {
        var startsNow = _localizer[_startsNowKey];

        return schemeResult.SchemeCode switch
        {
            SchemeCode.TaxFreeChildcare => GetTaxFreeChildcareStarts(schemeResult, child),

            SchemeCode.UniversalCreditChildcare => child.IsBorn
                ? startsNow
                : _localizer["Starts_WhenReturnToWork"],

            SchemeCode.ThirtyHoursForWorkingFamilies => GetThirtyHoursStarts(schemeResult, child),

            SchemeCode.FifteenHoursForDisadvantagedChildren => GetFifteenHoursForDisadvantagedChildrenStarts(schemeResult),

            SchemeCode.FifteenHoursUniversal => GetFifteenHoursUniversalStarts(schemeResult, child),

            _ => throw new InvalidOperationException($"{_unknownSchemeCodeMessage} {schemeResult.SchemeCode}")
        };
    }

    private string GetTaxFreeChildcareStarts(SchemeResultDto schemeResult, ChildResultDto child) => schemeResult.ApplyAndStartAffectedByParentalLeave switch
    {
        ParentalLeaveParty.User => _localizer["Starts_TaxFreeChildcare_UserParentalLeave"],

        ParentalLeaveParty.Partner => _localizer["Starts_TaxFreeChildcare_PartnerParentalLeave"],

        ParentalLeaveParty.UserAndPartner => _localizer["Starts_TaxFreeChildcare_UserAndPartnerParentalLeave"],

        null => child.IsBorn ? _localizer[_startsNowKey] : _localizer["Starts_WhenReturnToWork"],

        _ => throw InvalidParentalLeaveParty(
            schemeResult.ApplyAndStartAffectedByParentalLeave,
            nameof(GetTaxFreeChildcareStarts))
    };

    private string GetThirtyHoursStarts(SchemeResultDto schemeResult, ChildResultDto child) => schemeResult.ApplyAndStartAffectedByParentalLeave switch
    {
        ParentalLeaveParty.User => _localizer["Starts_ThirtyHours_UserParentalLeave"],

        ParentalLeaveParty.Partner => _localizer["Starts_ThirtyHours_PartnerParentalLeave"],

        ParentalLeaveParty.UserAndPartner => _localizer["Starts_ThirtyHours_UserAndPartnerParentalLeave"],

        null => GetStandardThirtyHoursStarts(schemeResult, child),

        _ => throw InvalidParentalLeaveParty(
            schemeResult.ApplyAndStartAffectedByParentalLeave,
            nameof(GetThirtyHoursStarts))
    };

    private string GetStandardThirtyHoursStarts(SchemeResultDto schemeResult, ChildResultDto child)
    {
        if (!child.IsBorn)
        {
            return _localizer["Starts_ThirtyHours_WhenChildTurnsNineMonths", child.ChildName];
        }

        var today = DateOnly.FromDateTime(DateTime.Today);

        var useFrom = schemeResult.UseFromDate
                      ?? throw new InvalidOperationException(
                          $"{schemeResult.SchemeCode} must have a UseFromDate.");

        while (useFrom < today)
        {
            useFrom = TermDateCalculator.GetNextTermStartDate(useFrom);
        }

        return _localizer["Starts_FromDate", useFrom];
    }

    private string GetFifteenHoursForDisadvantagedChildrenStarts(SchemeResultDto schemeResult)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var useFrom = schemeResult.UseFromDate
                      ?? throw new InvalidOperationException(
                          $"{schemeResult.SchemeCode} must have a UseFromDate.");

        if (schemeResult.EligibleNow && useFrom < today)
        {
            return _localizer[_startsNowKey];
        }

        return _localizer["Starts_FromDate", useFrom];
    }

    private string GetFifteenHoursUniversalStarts(SchemeResultDto schemeResult, ChildResultDto child)
    {
        if (!child.IsBorn)
        {
            return _localizer["Starts_FifteenHoursUniversal_TermAfterTurnsThree", child.ChildName];
        }

        if (schemeResult.EligibleNow)
        {
            return _localizer[_startsNowKey];
        }

        if (schemeResult.EligibleInFuture)
        {
            return _localizer["Starts_FifteenHoursUniversal_FromDate", child.ChildName, schemeResult.UseFromDate!.Value];
        }

        throw new InvalidOperationException("Unexpected eligibility state for Fifteen Hours Universal.");
    }

    private string GetEnds(SchemeResultDto schemeResult, ChildResultDto child) => schemeResult.SchemeCode switch
    {
        SchemeCode.TaxFreeChildcare => GetTaxFreeChildcareEnds(schemeResult, child),

        SchemeCode.UniversalCreditChildcare => _localizer["Ends_UniversalCreditChildcare", child.ChildName],

        SchemeCode.FifteenHoursUniversal => _localizer["Ends_FifteenHoursUniversal", child.ChildName],

        SchemeCode.FifteenHoursForDisadvantagedChildren => _localizer["Ends_FifteenHoursForDisadvantagedChildren",
            child.ChildName],

        SchemeCode.ThirtyHoursForWorkingFamilies => GetThirtyHoursEnds(schemeResult, child),

        _ => throw new InvalidOperationException($"{_unknownSchemeCodeMessage} {schemeResult.SchemeCode}")
    };

    private string GetTaxFreeChildcareEnds(SchemeResultDto schemeResult, ChildResultDto child) => schemeResult.EligibilityEndsWithParentalLeaveFor switch
    {
        ParentalLeaveParty.User => _localizer["Ends_TaxFreeChildcare_UserParentalLeave"],

        ParentalLeaveParty.Partner => _localizer["Ends_TaxFreeChildcare_PartnerParentalLeave"],

        ParentalLeaveParty.UserAndPartner => _localizer["Ends_TaxFreeChildcare_UserAndPartnerParentalLeave"],

        null => _localizer["Ends_TaxFreeChildcare", child.ChildName],

        _ => throw InvalidParentalLeaveParty(
            schemeResult.EligibilityEndsWithParentalLeaveFor,
            nameof(GetTaxFreeChildcareEnds))
    };

    private string GetThirtyHoursEnds(SchemeResultDto schemeResult, ChildResultDto child) => schemeResult.EligibilityEndsWithParentalLeaveFor switch
    {
        ParentalLeaveParty.User => _localizer["Ends_ThirtyHours_UserParentalLeave"],

        ParentalLeaveParty.Partner => _localizer["Ends_ThirtyHours_PartnerParentalLeave"],

        ParentalLeaveParty.UserAndPartner => _localizer["Ends_ThirtyHours_UserAndPartnerParentalLeave"],

        null => _localizer["Ends_ThirtyHoursForWorkingFamilies", child.ChildName],

        _ => throw InvalidParentalLeaveParty(
            schemeResult.EligibilityEndsWithParentalLeaveFor,
            nameof(GetThirtyHoursEnds))
    };

    private static List<SchemeCode> GetCanBeUsedWith(SchemeResultDto schemeResult, ChildResultDto child)
    {
        List<SchemeCode> compatibleSchemes = schemeResult.SchemeCode switch
        {
            SchemeCode.TaxFreeChildcare =>
            [
                SchemeCode.ThirtyHoursForWorkingFamilies,
                SchemeCode.FifteenHoursForDisadvantagedChildren,
                SchemeCode.FifteenHoursUniversal
            ],

            SchemeCode.UniversalCreditChildcare =>
            [
                SchemeCode.ThirtyHoursForWorkingFamilies,
                SchemeCode.FifteenHoursForDisadvantagedChildren,
                SchemeCode.FifteenHoursUniversal
            ],

            SchemeCode.ThirtyHoursForWorkingFamilies =>
            [
                SchemeCode.TaxFreeChildcare,
                SchemeCode.UniversalCreditChildcare,
                SchemeCode.FifteenHoursForDisadvantagedChildren,
                SchemeCode.FifteenHoursUniversal
            ],

            SchemeCode.FifteenHoursForDisadvantagedChildren =>
            [
                SchemeCode.TaxFreeChildcare,
                SchemeCode.UniversalCreditChildcare,
                SchemeCode.ThirtyHoursForWorkingFamilies
            ],

            SchemeCode.FifteenHoursUniversal =>
            [
                SchemeCode.TaxFreeChildcare,
                SchemeCode.UniversalCreditChildcare,
                SchemeCode.ThirtyHoursForWorkingFamilies
            ],

            _ => throw new InvalidOperationException($"{_unknownSchemeCodeMessage} {schemeResult.SchemeCode}")
        };

        var eligibleSchemes = child.Schemes
            .Select(x => x.SchemeCode)
            .ToHashSet();

        return [.. compatibleSchemes.Where(eligibleSchemes.Contains)];
    }

    private static ArgumentOutOfRangeException UnknownSchemeCode(SchemeCode schemeCode) =>
        new(nameof(schemeCode),
            schemeCode,
            _unknownSchemeCodeMessage);

    private static UnreachableException InvalidParentalLeaveParty(ParentalLeaveParty? value, string context) => new($"Unsupported parental leave party in {context}: {value}");
}
