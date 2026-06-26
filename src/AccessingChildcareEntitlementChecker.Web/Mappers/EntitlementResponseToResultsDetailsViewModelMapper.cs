using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;
using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models.Results;
using Microsoft.Extensions.Localization;

namespace AccessingChildcareEntitlementChecker.Web.Mappers;

public class EntitlementResponseToResultsDetailsViewModelMapper
{
    private const string UnknownSchemeCodeMessage = "Unknown scheme code";
    private readonly IStringLocalizer _localizer;

    public EntitlementResponseToResultsDetailsViewModelMapper(
        IStringLocalizerFactory stringLocalizerFactory)
    {
        _localizer = stringLocalizerFactory.Create(
            "Views.Results.ResultsDetailed",
            typeof(ResultsController).Assembly.GetName().Name!);
    }

    public ResultsDetailsViewModel Map(EntitlementResponse response, string childId)
    {
        var child = response.ChildResults.Single(x => x.ChildId == childId);

        return new ResultsDetailsViewModel()
        {
            ChildId = child.ChildId,
            ChildName = child.ChildName,
            Sections = GetSections(child)
        };

    }

    private List<SchemeSectionViewModel> GetSections(ChildResultDto child)
    {
        var sections = new List<SchemeSectionViewModel>();

        var helpWithCosts = child.Schemes
            .Where(x =>
                x.SchemeCode == SchemeCode.TaxFreeChildcare ||
                x.SchemeCode == SchemeCode.UniversalCreditChildcare)
            .ToList();

        if (helpWithCosts.Count > 0)
        {
            sections.Add(new SchemeSectionViewModel
            {
                SectionType = SchemeSectionType.HelpWithChildcareCosts,
                ShowThirtyHourWarning = false,
                Schemes = helpWithCosts
                    .Select(x => MapSchemeResult(x, child))
                    .ToList()
            });
        }

        var fundedHours = child.Schemes
            .Where(x =>
                x.SchemeCode == SchemeCode.ThirtyHoursForWorkingFamilies ||
                x.SchemeCode == SchemeCode.FifteenHoursForDisadvantagedChildren ||
                x.SchemeCode == SchemeCode.FifteenHoursUniversal)
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
                Schemes = fundedHours
                    .Select(x => MapSchemeResult(x, child))
                    .ToList()
            });
        }

        return sections;
    }

    private SchemeDetailsViewModel MapSchemeResult(
        SchemeResultDto schemeResult,
        ChildResultDto child)
    {
        return new SchemeDetailsViewModel
        {
            SchemeCode = schemeResult.SchemeCode,
            Name = GetSchemeName(schemeResult.SchemeCode),
            Url = GetSchemeUrl(schemeResult.SchemeCode),
            EligibleNow = schemeResult.EligibleNow,
            EligibleInFuture = schemeResult.EligibleInFuture,
            ApplyFromDate = schemeResult.ApplyFromDate,
            UseFromDate = schemeResult.UseFromDate,
            WhenToApply = GetWhenToApply(schemeResult, child),
            Starts = GetStarts(schemeResult, child),
            Ends = GetEnds(schemeResult, child),
            CanBeUsedWith = GetCanBeUsedWith(schemeResult, child)
        };
    }

    private string GetSchemeName(SchemeCode schemeCode)
    {
        return schemeCode switch
        {
            SchemeCode.TaxFreeChildcare => _localizer["TaxFreeChildcare_Name"],
            SchemeCode.FifteenHoursUniversal => _localizer["FifteenHoursUniversal_Name"],
            SchemeCode.FifteenHoursForDisadvantagedChildren => _localizer["FifteenHoursForDisadvantagedChildren_Name"],
            SchemeCode.UniversalCreditChildcare => _localizer["UniversalCreditChildcare_Name"],
            SchemeCode.ThirtyHoursForWorkingFamilies => _localizer["ThirtyHoursForWorkingFamilies_Name"],

            _ => throw UnknownSchemeCode(schemeCode)
        };
    }

    private static string GetSchemeUrl(SchemeCode schemeCode)
    {
        return schemeCode switch
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
    }

    private string GetWhenToApply(SchemeResultDto schemeResult, ChildResultDto child)
    {
        return schemeResult.SchemeCode switch
        {
            SchemeCode.TaxFreeChildcare => child.IsBorn
                ? _localizer["WhenToApply_Now"]
                : _localizer["WhenToApply_WhenBorn"],

            SchemeCode.UniversalCreditChildcare => child.IsBorn
                ? _localizer["WhenToApply_Now"]
                : _localizer["WhenToApply_WhenBorn"],

            SchemeCode.FifteenHoursUniversal => _localizer["WhenToApply_AskProviderOrCouncil"],

            SchemeCode.FifteenHoursForDisadvantagedChildren => schemeResult.EligibleNow
                ? _localizer["WhenToApply_Now"]
                : _localizer["WhenToApply_FromDate", schemeResult.ApplyFromDate!.Value],

            SchemeCode.ThirtyHoursForWorkingFamilies => GetThirtyHoursWhenToApply(schemeResult, child),

            _ => throw new InvalidOperationException($"{UnknownSchemeCodeMessage} {schemeResult.SchemeCode}")
        };
    }

    private string GetThirtyHoursWhenToApply(SchemeResultDto schemeResult, ChildResultDto child)
    {
        if (!child.IsBorn)
        {
            return _localizer["WhenToApply_WhenTwentyThreeWeeksOld"];
        }

        var today = DateOnly.FromDateTime(DateTime.Today);

        var canApplyNow = schemeResult.ApplyFromDate!.Value <= today;

        var windowStart = GetApplicationWindowStart(schemeResult.UseFromDate!.Value);
        var windowEnd = GetApplicationWindowEnd(schemeResult.UseFromDate!.Value);

        if (canApplyNow)
        {
            return _localizer["WhenToApply_ByDate", windowEnd];
        }

        return _localizer["WhenToApply_FromToDate", windowStart, windowEnd];
    }

    private static DateOnly GetApplicationWindowStart(DateOnly useFromDate)
    {
        return useFromDate.Month switch
        {
            1 => new DateOnly(useFromDate.Year - 1, 9, 1),
            4 => new DateOnly(useFromDate.Year, 1, 1),
            9 => new DateOnly(useFromDate.Year, 4, 1),

            _ => throw new InvalidOperationException($"Unexpected term start date: {useFromDate}")
        };
    }

    private static DateOnly GetApplicationWindowEnd(DateOnly useFromDate)
    {
        return useFromDate.Month switch
        {
            1 => new DateOnly(useFromDate.Year - 1, 12, 31),
            4 => new DateOnly(useFromDate.Year, 3, 31),
            9 => new DateOnly(useFromDate.Year, 8, 31),

            _ => throw new InvalidOperationException($"Unexpected term start date: {useFromDate}")
        };
    }

    private string GetStarts(SchemeResultDto schemeResult, ChildResultDto child)
    {
        var startsNow = _localizer["Starts_Now"];

        return schemeResult.SchemeCode switch
        {
            SchemeCode.TaxFreeChildcare => child.IsBorn
                ? startsNow
                : _localizer["Starts_WhenReturnToWork"],

            SchemeCode.UniversalCreditChildcare => child.IsBorn
                ? startsNow
                : _localizer["Starts_WhenReturnToWork"],

            SchemeCode.ThirtyHoursForWorkingFamilies => GetThirtyHoursStarts(schemeResult, child),

            SchemeCode.FifteenHoursForDisadvantagedChildren => schemeResult.EligibleNow
                ? startsNow
                : _localizer["Starts_FromDate", schemeResult.UseFromDate!.Value],

            SchemeCode.FifteenHoursUniversal => GetFifteenHoursUniversalStarts(schemeResult, child),

            _ => throw new InvalidOperationException($"{UnknownSchemeCodeMessage} {schemeResult.SchemeCode}")
        };
    }

    private string GetThirtyHoursStarts(SchemeResultDto schemeResult, ChildResultDto child)
    {
        if (!child.IsBorn)
        {
            return _localizer["Starts_ThirtyHours_WhenChildTurnsNineMonths", child.ChildName];
        }

        if (schemeResult.EligibleNow)
        {
            return _localizer["Starts_Now"];
        }

        return _localizer["Starts_FromDate", schemeResult.UseFromDate!.Value];
    }

    private string GetFifteenHoursUniversalStarts(SchemeResultDto schemeResult, ChildResultDto child)
    {
        if (!child.IsBorn)
        {
            return _localizer["Starts_FifteenHoursUniversal_TermAfterTurnsThree", child.ChildName];
        }

        if (schemeResult.EligibleNow)
        {
            return _localizer["Starts_Now"];
        }

        if (schemeResult.EligibleInFuture)
        {
            return _localizer["Starts_FifteenHoursUniversal_FromDate", schemeResult.UseFromDate!.Value];
        }

        throw new InvalidOperationException("Unexpected eligibility state for Fifteen Hours Universal.");
    }

    private string GetEnds(SchemeResultDto schemeResult, ChildResultDto child)
    {
        return schemeResult.SchemeCode switch
        {
            SchemeCode.TaxFreeChildcare => _localizer["Ends_TaxFreeChildcare", child.ChildName],

            SchemeCode.UniversalCreditChildcare => _localizer["Ends_UniversalCreditChildcare", child.ChildName],

            SchemeCode.FifteenHoursUniversal => _localizer["Ends_FifteenHoursUniversal", child.ChildName],

            SchemeCode.FifteenHoursForDisadvantagedChildren => _localizer["Ends_FifteenHoursForDisadvantagedChildren",
                child.ChildName],

            SchemeCode.ThirtyHoursForWorkingFamilies => _localizer["Ends_ThirtyHoursForWorkingFamilies",
                child.ChildName],

            _ => throw new InvalidOperationException($"{UnknownSchemeCodeMessage} {schemeResult.SchemeCode}")
        };
    }

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

            _ => throw new InvalidOperationException($"{UnknownSchemeCodeMessage} {schemeResult.SchemeCode}")
        };

        var eligibleSchemes = child.Schemes
            .Select(x => x.SchemeCode)
            .ToHashSet();

        return compatibleSchemes
            .Where(eligibleSchemes.Contains)
            .ToList();
    }

    private static ArgumentOutOfRangeException UnknownSchemeCode(SchemeCode schemeCode) =>
        new(nameof(schemeCode),
            schemeCode,
            UnknownSchemeCodeMessage);
}