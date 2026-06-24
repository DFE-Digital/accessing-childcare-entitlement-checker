using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;
using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models.Results;
using Microsoft.Extensions.Localization;

namespace AccessingChildcareEntitlementChecker.Web.Mappers;

public class EntitlementResponseToResultsViewModelMapper
{
    private const string UnknownSchemeCodeMessage = "Unknown scheme code";
    private readonly IStringLocalizer _localizer;

    public EntitlementResponseToResultsViewModelMapper(
        IStringLocalizerFactory stringLocalizerFactory)
    {
        _localizer = stringLocalizerFactory.Create(
            "Views.Results.Results",
            typeof(ResultsController).Assembly.GetName().Name!);
    }


    public ResultsViewModel Map(EntitlementResponse response)
    {
        return new ResultsViewModel()
        {
            Children = response.ChildResults.Select(MapChildResults).ToList(),
        };

    }

    private ChildResultsViewModel MapChildResults(ChildResultDto childResult)
    {
        return new ChildResultsViewModel()
        {
            Name = childResult.ChildName,
            ShowThirtyHourWarning = GetThirtyHourWarning(childResult),
            IsBorn = childResult.IsBorn,
            Schemes = childResult.Schemes
                .OrderBy(s => GetSchemeOrder(s.SchemeCode))
                .Select(s => MapSchemeResult(s, childResult))
                .ToList()
        };

    }

    private SchemeResultsViewModel MapSchemeResult(SchemeResultDto schemeResult, ChildResultDto childResult)
    {
        return new SchemeResultsViewModel()
        {
            SchemeCode = schemeResult.SchemeCode,
            Name = GetSchemeName(schemeResult.SchemeCode),
            Url = GetSchemeUrl(schemeResult.SchemeCode),
            WhatYouGet = GetSchemeDescription(schemeResult.SchemeCode),
            EligibleNow = schemeResult.EligibleNow,
            EligibleInFuture = schemeResult.EligibleInFuture,
            ApplyFromDate = schemeResult.ApplyFromDate,
            UseFromDate = schemeResult.UseFromDate,
            WhenToApply = GetWhenToApply(schemeResult, childResult)
        };

    }

    private string GetWhenToApply(SchemeResultDto schemeResult, ChildResultDto childResult)
    {
        var now = _localizer["WhenToApply_Now"];

        if (schemeResult.SchemeCode == SchemeCode.FifteenHoursUniversal)
        {
            return _localizer["WhenToApply_AskProviderOrCouncil"];
        }

        if (schemeResult.SchemeCode == SchemeCode.TaxFreeChildcare)
        {
            return schemeResult.EligibleNow ? now : _localizer["WhenToApply_WhenBorn"];
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
            if (schemeResult.EligibleNow)
            {
                return now;
            }

            if (!childResult.IsBorn)
            {
                return _localizer["WhenToApply_WhenTwentyThreeWeeksOld"];

            }

            return _localizer["WhenToApply_FromDate", schemeResult.ApplyFromDate!.Value];

        }

        throw new InvalidOperationException($"{UnknownSchemeCodeMessage}: {schemeResult.SchemeCode}");
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

    private static int GetSchemeOrder(SchemeCode schemeCode)
    {
        return schemeCode switch
        {
            SchemeCode.TaxFreeChildcare => 1,
            SchemeCode.UniversalCreditChildcare => 2,
            SchemeCode.ThirtyHoursForWorkingFamilies => 3,
            SchemeCode.FifteenHoursForDisadvantagedChildren => 4,
            SchemeCode.FifteenHoursUniversal => 5,

            _ => 999
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

            _ => throw new ArgumentOutOfRangeException(
                nameof(schemeCode),
                schemeCode,
                UnknownSchemeCodeMessage)
        };
    }

    private static string GetSchemeUrl(SchemeCode schemeCode)
    {
        return schemeCode switch
        {
            SchemeCode.TaxFreeChildcare => "https://www.gov.uk/tax-free-childcare",
            SchemeCode.FifteenHoursUniversal => "https://www.gov.uk/help-with-childcare-costs/free-childcare-and-education-for-3-to-4-year-olds",
            SchemeCode.FifteenHoursForDisadvantagedChildren => "https://www.gov.uk/help-with-childcare-costs/free-childcare-2-year-olds-extra-support",
            SchemeCode.UniversalCreditChildcare => "https://www.gov.uk/help-with-childcare-costs/universal-credit",
            SchemeCode.ThirtyHoursForWorkingFamilies => "https://www.gov.uk/free-childcare-if-working",

            _ => throw new ArgumentOutOfRangeException(
                nameof(schemeCode),
                schemeCode,
                UnknownSchemeCodeMessage)
        };
    }

    private string GetSchemeDescription(SchemeCode schemeCode)
    {
        return schemeCode switch
        {
            SchemeCode.TaxFreeChildcare => _localizer["TaxFreeChildcare_Description"],
            SchemeCode.FifteenHoursUniversal => _localizer["FifteenHoursUniversal_Description"],
            SchemeCode.FifteenHoursForDisadvantagedChildren => _localizer["FifteenHoursForDisadvantagedChildren_Description"],
            SchemeCode.UniversalCreditChildcare => _localizer["UniversalCreditChildcare_Description"],
            SchemeCode.ThirtyHoursForWorkingFamilies => _localizer["ThirtyHoursForWorkingFamilies_Description"],

            _ => throw new ArgumentOutOfRangeException(
                nameof(schemeCode),
                schemeCode,
                UnknownSchemeCodeMessage)
        };
    }

}