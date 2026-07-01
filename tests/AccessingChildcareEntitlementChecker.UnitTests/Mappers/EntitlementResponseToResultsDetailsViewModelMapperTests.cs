using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;
using AccessingChildcareEntitlementChecker.RulesEngine.Helpers;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;
using AccessingChildcareEntitlementChecker.Web.Mappers;
using AccessingChildcareEntitlementChecker.Web.Models.Results;
using Microsoft.Extensions.Localization;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Mappers;

public class EntitlementResponseToResultsDetailsViewModelMapperTests
{
    private readonly EntitlementResponseToResultsDetailsViewModelMapper _mapper;

    private static DateOnly GetThirtyHoursApplyFrom(DateOnly dateOfBirth) =>
        dateOfBirth.AddDays(23 * 7);

    private static DateOnly GetThirtyHoursUseFrom(DateOnly dateOfBirth) =>
        TermDateCalculator.GetNextTermStartDate(dateOfBirth.AddMonths(9));

    private static DateOnly GetFifteenHoursUniversalUseFrom(DateOnly dateOfBirth) =>
        TermDateCalculator.GetNextTermStartDate(dateOfBirth.AddYears(3));

    private static DateOnly GetDisadvantagedTwoYearOldUseFrom(DateOnly dateOfBirth) =>
        TermDateCalculator.GetNextTermStartDate(dateOfBirth.AddYears(2));


    public EntitlementResponseToResultsDetailsViewModelMapperTests()
    {
        var localizerFactory = Substitute.For<IStringLocalizerFactory>();
        var localizer = Substitute.For<IStringLocalizer>();

        localizerFactory.Create(Arg.Any<string>(), Arg.Any<string>()).Returns(localizer);

        localizer[Arg.Any<string>()]
            .Returns(call =>
            {
                var key = call.Arg<string>();

                return new LocalizedString(
                    key,
                    key);
            });

        localizer[Arg.Any<string>(), Arg.Any<object[]>()]
            .Returns(call =>
            {
                var key = call.ArgAt<string>(0);

                return new LocalizedString(
                    key,
                    key);
            });

        _mapper = new EntitlementResponseToResultsDetailsViewModelMapper(localizerFactory);
    }

    private static EntitlementResponse CreateTestEntitlementResponse()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var katieDob = today.AddYears(-3);
        var jackDob = today.AddMonths(-6);
        var emilyDob = today.AddYears(-1);
        var miaDob = today.AddYears(-2);
        var alfieDob = today.AddYears(-1);
        var oliverDob = today.AddMonths(-1);
        var tomDob = today.AddYears(-3);

        var useFromDate = TermDateCalculator.GetNextTermStartDate(
            today.AddMonths(-6));

        return new EntitlementResponse()
        {
            ChildResults =
            [
                // Child 1 - eligible now, triggers 30-hour warning
                new ChildResultDto
                {
                    ChildId = "child-1",
                    ChildName = "Katie",
                    IsBorn = true,
                    AgeInYears = 3,
                    Schemes =
                    [
                        new SchemeResultDto
                        {
                            SchemeCode = SchemeCode.ThirtyHoursForWorkingFamilies,
                            EligibleNow = true,
                            ApplyFromDate = GetThirtyHoursApplyFrom(katieDob),
                            UseFromDate = GetThirtyHoursUseFrom(katieDob)
                        },
                        new SchemeResultDto
                        {
                            SchemeCode = SchemeCode.FifteenHoursUniversal,
                            EligibleNow = true,
                            UseFromDate = GetFifteenHoursUniversalUseFrom(katieDob)
                        },
                        new SchemeResultDto
                        {
                            SchemeCode = SchemeCode.TaxFreeChildcare,
                            EligibleNow = true
                        }
                    ]
                },

                // Child 2 - born but not yet eligible for FCWP
                new ChildResultDto
                {
                    ChildId = "child-2",
                    ChildName = "Jack",
                    IsBorn = true,
                    AgeInYears = 0,
                    Schemes =
                    [
                        new SchemeResultDto
                        {
                            SchemeCode = SchemeCode.ThirtyHoursForWorkingFamilies,
                            EligibleInFuture = true,
                            ApplyFromDate = GetThirtyHoursApplyFrom(jackDob),
                            UseFromDate = GetThirtyHoursUseFrom(jackDob)
                        },
                        new SchemeResultDto
                        {
                            SchemeCode = SchemeCode.TaxFreeChildcare,
                            EligibleNow = true
                        }
                    ]
                },

                // Child 3 - unborn
                new ChildResultDto
                {
                    ChildId = "child-3",
                    ChildName = "Baby Smith",
                    IsBorn = false,
                    Schemes =
                    [
                        new SchemeResultDto
                        {
                            SchemeCode = SchemeCode.ThirtyHoursForWorkingFamilies,
                            EligibleInFuture = true
                        },
                        new SchemeResultDto
                        {
                            SchemeCode = SchemeCode.FifteenHoursUniversal,
                            EligibleInFuture = true
                        },
                        new SchemeResultDto
                        {
                            SchemeCode = SchemeCode.TaxFreeChildcare,
                            EligibleInFuture = true
                        },
                        new SchemeResultDto
                        {
                            SchemeCode = SchemeCode.UniversalCreditChildcare,
                            EligibleInFuture = true
                        }
                    ]
                },

                // Child 4 - disadvantaged 2 yr old Eligible in future
                new ChildResultDto
                {
                    ChildId = "child-4",
                    ChildName = "Emily",
                    IsBorn = true,
                    AgeInYears = 1,
                    Schemes =
                    [
                        new SchemeResultDto
                        {
                            SchemeCode = SchemeCode.FifteenHoursForDisadvantagedChildren,
                            EligibleInFuture = true,
                            ApplyFromDate = emilyDob.AddYears(2),
                            UseFromDate = GetDisadvantagedTwoYearOldUseFrom(emilyDob)
                        },
                        new SchemeResultDto
                        {
                            SchemeCode = SchemeCode.ThirtyHoursForWorkingFamilies,
                            EligibleNow = true,
                            ApplyFromDate = GetThirtyHoursApplyFrom(emilyDob),
                            UseFromDate = GetThirtyHoursUseFrom(emilyDob)
                        },
                    ]
                },

                // Child 5 - disadvantaged 2 yr old Eligible now
                new ChildResultDto
                {
                    ChildId = "child-5",
                    ChildName = "Mia",
                    IsBorn = true,
                    AgeInYears = 2,
                    Schemes =
                    [
                        new SchemeResultDto
                        {
                            SchemeCode = SchemeCode.FifteenHoursForDisadvantagedChildren,
                            EligibleNow = true,
                            ApplyFromDate = miaDob.AddYears(2),
                            UseFromDate = GetDisadvantagedTwoYearOldUseFrom(miaDob)
                        },
                        new SchemeResultDto
                        {
                            SchemeCode = SchemeCode.ThirtyHoursForWorkingFamilies,
                            EligibleNow = true,
                            ApplyFromDate = GetThirtyHoursApplyFrom(miaDob),
                            UseFromDate = GetThirtyHoursUseFrom(miaDob)
                        },
                    ]
                },
                // Child 6 - UC Eligible now
                new ChildResultDto
                {
                    ChildId = "child-6",
                    ChildName = "Alfie",
                    IsBorn = true,
                    AgeInYears = 1,
                    Schemes =
                    [
                        new SchemeResultDto
                        {
                            SchemeCode = SchemeCode.UniversalCreditChildcare,
                            EligibleNow = true
                        },
                    ]
                },
                // Child 7 - born, less than 23 weeks old
                new ChildResultDto
                {
                    ChildId = "child-7",
                    ChildName = "Oliver",
                    IsBorn = true,
                    AgeInYears = 0,
                    Schemes =
                    [
                        new SchemeResultDto
                        {
                            SchemeCode = SchemeCode.ThirtyHoursForWorkingFamilies,
                            EligibleInFuture = true,
                            ApplyFromDate = GetThirtyHoursApplyFrom(oliverDob),
                            UseFromDate = GetThirtyHoursUseFrom(oliverDob)
                        }
                    ]
                },
                // Child 8 - born, 15 hours eligible now
                new ChildResultDto
                {
                    ChildId = "child-8",
                    ChildName = "Tom",
                    IsBorn = true,
                    AgeInYears = 0,
                    Schemes =
                    [
                        new SchemeResultDto
                        {
                            SchemeCode = SchemeCode.FifteenHoursUniversal,
                            EligibleNow = true,
                            UseFromDate = GetFifteenHoursUniversalUseFrom(tomDob)
                        }
                    ]
                }
            ]
        };
    }

    [Fact]
    public void Map_ChildName()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-1"), false);

        Assert.Equal("Katie", result.ChildName);
    }

    [Fact]
    public void Map_SchemeName()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-2"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single();

        Assert.Equal(SchemeCode.ThirtyHoursForWorkingFamilies, scheme.SchemeCode);
    }

    [Fact]
    public void Map_SchemeUrl()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-6"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.HelpWithChildcareCosts)
            .Schemes
            .Single();

        Assert.Equal("https://www.gov.uk/help-with-childcare-costs/universal-credit", scheme.Url);
    }

    [Fact]
    public void Map_ShowsThirtyHourWarning_WhenThirtyHoursAndFifteenHoursUniversalPresent()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-1"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours);

        Assert.True(scheme.ShowThirtyHourWarning);
    }

    [Fact]
    public void Map_ShowsThirtyHourWarning_WhenThirtyHoursAndDisadvantagedFifteenHoursPresent()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-4"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours);

        Assert.True(scheme.ShowThirtyHourWarning);
    }

    [Fact]
    public void Map_FifteenHoursUniversal_ReturnsAskYourChildcareProviderOrLocalCouncil()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-1"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.FifteenHoursUniversal);

        Assert.Equal("WhenToApply_AskProviderOrCouncil", scheme.WhenToApply);
    }

    [Fact]
    public void Map_FifteenHoursForDisadvantagedChildren_EligibleNow_ReturnsNow()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-5"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.FifteenHoursForDisadvantagedChildren);

        Assert.Equal("WhenToApply_Now", scheme.WhenToApply);

    }

    [Fact]
    public void Map_FifteenHoursForDisadvantagedChildren_EligibleInFuture_ReturnsApplyFromDate()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-4"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.FifteenHoursForDisadvantagedChildren);

        Assert.Equal("WhenToApply_FromDate", scheme.WhenToApply);

    }

    [Fact]
    public void Map_ThirtyHoursForWorkingFamilies_EligibleToApplyNow_ReturnsApplyBy()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-1"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.ThirtyHoursForWorkingFamilies);

        Assert.Equal("WhenToApply_ByDate", scheme.WhenToApply);
    }

    [Fact]
    public void Map_ThirtyHoursForWorkingFamilies_BornAndEligibleToApplyInFuture_ReturnsApplyFromToDate()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-7"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.ThirtyHoursForWorkingFamilies);

        Assert.Equal("WhenToApply_FromToDate", scheme.WhenToApply);
    }

    [Fact]
    public void Map_ThirtyHoursForWorkingFamilies_UnbornAndEligibleInFuture_ReturnsWhenTheyAreTwentyThreeWeeksOld()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-3"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.ThirtyHoursForWorkingFamilies);

        Assert.Equal("WhenToApply_WhenTwentyThreeWeeksOld", scheme.WhenToApply);
    }

    [Fact]
    public void Map_TaxFreeChildcare_EligibleNow_ReturnsNow()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-1"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.HelpWithChildcareCosts)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.TaxFreeChildcare);

        Assert.Equal("WhenToApply_Now", scheme.WhenToApply);

    }

    [Fact]
    public void Map_TaxFreeChildcare_EligibleInFuture_ReturnsWhenTheyAreBorn()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-3"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.HelpWithChildcareCosts)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.TaxFreeChildcare);

        Assert.Equal("WhenToApply_WhenBorn", scheme.WhenToApply);

    }

    [Fact]
    public void Map_UniversalCreditChildcare_UnbornEligibleInFuture_ReturnsWhenTheyAreBorn()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-3"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.HelpWithChildcareCosts)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.UniversalCreditChildcare);

        Assert.Equal("WhenToApply_WhenBorn", scheme.WhenToApply);
    }

    [Fact]
    public void Map_UniversalCreditChildcare_EligibleNow_ReturnsNow()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-6"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.HelpWithChildcareCosts)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.UniversalCreditChildcare);

        Assert.Equal("WhenToApply_Now", scheme.WhenToApply);
    }

    [Fact]
    public void Map_DoesNotShowThirtyHourWarning_WhenThirtyHoursSchemeMissing()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-8"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours);

        Assert.False(scheme.ShowThirtyHourWarning);
    }

    [Fact]
    public void Map_ThirtyHoursForWorkingFamilies_EligibleNow_StartsNow()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-1"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.ThirtyHoursForWorkingFamilies);

        Assert.Equal("Starts_Now", scheme.Starts);
    }

    [Fact]
    public void Map_ThirtyHoursForWorkingFamilies_EligibleInFuture_StartsFromDate()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-2"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.ThirtyHoursForWorkingFamilies);

        Assert.Equal("Starts_FromDate", scheme.Starts);
    }

    [Fact]
    public void Map_ThirtyHoursForWorkingFamilies_Unborn_ReturnsWhenChildTurnsNineMonths()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-3"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.ThirtyHoursForWorkingFamilies);

        Assert.Equal("Starts_ThirtyHours_WhenChildTurnsNineMonths", scheme.Starts);
    }

    [Fact]
    public void Map_TaxFreeChildcare_EligibleNow_StartsNow()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-1"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.HelpWithChildcareCosts)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.TaxFreeChildcare);

        Assert.Equal("Starts_Now", scheme.Starts);
    }

    [Fact]
    public void Map_TaxFreeChildcare_Unborn_StartsWhenReturnToWork()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-3"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.HelpWithChildcareCosts)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.TaxFreeChildcare);

        Assert.Equal("Starts_WhenReturnToWork", scheme.Starts);
    }

    [Fact]
    public void Map_UniversalCreditChildcare_EligibleNow_StartsNow()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-6"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.HelpWithChildcareCosts)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.UniversalCreditChildcare);

        Assert.Equal("Starts_Now", scheme.Starts);
    }

    [Fact]
    public void Map_UniversalCreditChildcare_Unborn_StartsWhenReturnToWork()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-3"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.HelpWithChildcareCosts)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.UniversalCreditChildcare);

        Assert.Equal("Starts_WhenReturnToWork", scheme.Starts);
    }

    [Fact]
    public void Map_FifteenHoursForDisadvantagedChildren_EligibleNow_StartsNow()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-5"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.FifteenHoursForDisadvantagedChildren);

        Assert.Equal("Starts_Now", scheme.Starts);
    }

    [Fact]
    public void Map_FifteenHoursForDisadvantagedChildren_EligibleInFuture_StartsFromDate()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-4"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.FifteenHoursForDisadvantagedChildren);

        Assert.Equal("Starts_FromDate", scheme.Starts);
    }

    [Fact]
    public void Map_FifteenHoursUniversal_EligibleNow_StartsNow()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-1"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.FifteenHoursUniversal);

        Assert.Equal("Starts_Now", scheme.Starts);
    }

    [Fact]
    public void Map_FifteenHoursUniversal_Unborn_StartsTermAfterChildTurnsThree()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-3"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.FifteenHoursUniversal);

        Assert.Equal("Starts_FifteenHoursUniversal_TermAfterTurnsThree", scheme.Starts);
    }

    [Fact]
    public void Map_TaxFreeChildcare_ReturnsCorrectEndsText()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-1"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.HelpWithChildcareCosts)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.TaxFreeChildcare);

        Assert.Equal("Ends_TaxFreeChildcare", scheme.Ends);
    }

    [Fact]
    public void Map_UniversalCreditChildcare_ReturnsCorrectEndsText()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-6"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.HelpWithChildcareCosts)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.UniversalCreditChildcare);

        Assert.Equal("Ends_UniversalCreditChildcare", scheme.Ends);
    }

    [Fact]
    public void Map_ThirtyHoursForWorkingFamilies_ReturnsCorrectEndsText()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-1"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.ThirtyHoursForWorkingFamilies);

        Assert.Equal("Ends_ThirtyHoursForWorkingFamilies", scheme.Ends);
    }

    [Fact]
    public void Map_FifteenHoursUniversal_ReturnsCorrectEndsText()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-1"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.FifteenHoursUniversal);

        Assert.Equal("Ends_FifteenHoursUniversal", scheme.Ends);
    }

    [Fact]
    public void Map_TaxFreeChildcare_ReturnsCompatibleEligibleSchemes()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-1"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.HelpWithChildcareCosts)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.TaxFreeChildcare);

        Assert.Equal(2, scheme.CanBeUsedWith.Count);
        Assert.Contains(SchemeCode.ThirtyHoursForWorkingFamilies, scheme.CanBeUsedWith);
        Assert.Contains(SchemeCode.FifteenHoursUniversal, scheme.CanBeUsedWith);
    }

    [Fact]
    public void Map_UniversalCreditChildcare_WithNoOtherEligibleSchemes_ReturnsEmptyList()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-6"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.HelpWithChildcareCosts)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.UniversalCreditChildcare);

        Assert.Empty(scheme.CanBeUsedWith);
    }

    [Fact]
    public void Map_ThirtyHoursForWorkingFamilies_ReturnsCompatibleEligibleSchemes()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-1"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.ThirtyHoursForWorkingFamilies);

        Assert.Equal(2, scheme.CanBeUsedWith.Count);
        Assert.Contains(SchemeCode.TaxFreeChildcare, scheme.CanBeUsedWith);
        Assert.Contains(SchemeCode.FifteenHoursUniversal, scheme.CanBeUsedWith);
    }

    [Fact]
    public void Map_FifteenHoursForDisadvantagedChildren_ReturnsCompatibleEligibleSchemes()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-4"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.FifteenHoursForDisadvantagedChildren);

        Assert.Single(scheme.CanBeUsedWith);
        Assert.Contains(SchemeCode.ThirtyHoursForWorkingFamilies, scheme.CanBeUsedWith);
    }

    [Fact]
    public void Map_FifteenHoursUniversal_ReturnsCompatibleEligibleSchemes()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response.ChildResults.Single(x => x.ChildId == "child-1"), false);

        var scheme = result
            .Sections
            .Single(x => x.SectionType == SchemeSectionType.FundedChildCareHours)
            .Schemes
            .Single(x => x.SchemeCode == SchemeCode.FifteenHoursUniversal);

        Assert.Equal(2, scheme.CanBeUsedWith.Count);
        Assert.Contains(SchemeCode.ThirtyHoursForWorkingFamilies, scheme.CanBeUsedWith);
        Assert.Contains(SchemeCode.TaxFreeChildcare, scheme.CanBeUsedWith);
    }

}