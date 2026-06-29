using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Requests;
using AccessingChildcareEntitlementChecker.RulesEngine.Dtos.Responses;
using AccessingChildcareEntitlementChecker.RulesEngine.Helpers;
using AccessingChildcareEntitlementChecker.RulesEngine.Types;
using AccessingChildcareEntitlementChecker.Web.Mappers;
using Microsoft.Extensions.Localization;
using NSubstitute;

namespace AccessingChildcareEntitlementChecker.UnitTests.Mappers;

public class EntitlementResponseToResultsSummaryViewModelMapperTests
{
    private readonly EntitlementResponseToResultsSummaryViewModelMapper _mapper;

    private static DateOnly GetThirtyHoursApplyFrom(DateOnly dateOfBirth) =>
        dateOfBirth.AddDays(23 * 7);

    private static DateOnly GetThirtyHoursUseFrom(DateOnly dateOfBirth) =>
        TermDateCalculator.GetNextTermStartDate(dateOfBirth.AddMonths(9));

    private static DateOnly GetFifteenHoursUniversalUseFrom(DateOnly dateOfBirth) =>
        TermDateCalculator.GetNextTermStartDate(dateOfBirth.AddYears(3));

    private static DateOnly GetDisadvantagedTwoYearOldUseFrom(DateOnly dateOfBirth) =>
        TermDateCalculator.GetNextTermStartDate(dateOfBirth.AddYears(2));

    public EntitlementResponseToResultsSummaryViewModelMapperTests()
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

        _mapper = new EntitlementResponseToResultsSummaryViewModelMapper(localizerFactory);
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

        var result = _mapper.Map(response);

        var child = result.Children.Single(x => x.Name == "Katie");

        Assert.Equal("Katie", child.Name);
    }

    [Fact]
    public void Map_SchemeName()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response);

        var child = result.Children.Single(x => x.Name == "Katie");

        var scheme = child.Schemes.Single(x => x.SchemeCode == SchemeCode.TaxFreeChildcare);

        Assert.Equal("TaxFreeChildcare_Name", scheme.Name);
    }

    [Fact]
    public void Map_SchemeUrl()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response);

        var child = result.Children.Single(x => x.Name == "Katie");

        var scheme = child.Schemes.Single(x => x.SchemeCode == SchemeCode.TaxFreeChildcare);

        Assert.Equal("https://www.gov.uk/tax-free-childcare", scheme.Url);
    }

    [Fact]
    public void Map_SchemeDescription()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response);

        var child = result.Children.Single(x => x.Name == "Katie");

        var scheme = child.Schemes.Single(x => x.SchemeCode == SchemeCode.TaxFreeChildcare);

        Assert.Equal("TaxFreeChildcare_Description", scheme.WhatYouGet);
    }

    [Fact]
    public void Map_OrdersSchemesInExpectedOrder()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response);

        var schemes = result.Children[0].Schemes;

        Assert.Collection(
            schemes,
            s => Assert.Equal(SchemeCode.TaxFreeChildcare, s.SchemeCode),
            s => Assert.Equal(SchemeCode.ThirtyHoursForWorkingFamilies, s.SchemeCode),
            s => Assert.Equal(SchemeCode.FifteenHoursUniversal, s.SchemeCode));
    }


    [Fact]
    public void Map_ShowsThirtyHourWarning_WhenThirtyHoursAndFifteenHoursUniversalPresent()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response);

        var child = result.Children.Single(x => x.Name == "Katie");

        Assert.True(child.ShowThirtyHourWarning);
    }

    [Fact]
    public void Map_ShowsThirtyHourWarning_WhenThirtyHoursAndDisadvantagedFifteenHoursPresent()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response);

        var child = result.Children.Single(x => x.Name == "Emily");

        Assert.True(child.ShowThirtyHourWarning);
    }

    [Fact]
    public void Map_FifteenHoursUniversal_ReturnsAskYourChildcareProviderOrLocalCouncil()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response);

        var child = result.Children.Single(x => x.Name == "Katie");

        var scheme = child.Schemes.Single(x => x.SchemeCode == SchemeCode.FifteenHoursUniversal);

        Assert.Equal("WhenToApply_AskProviderOrCouncil", scheme.WhenToApply);
    }

    [Fact]
    public void Map_FifteenHoursForDisadvantagedChildren_EligibleNow_ReturnsNow()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response);

        var child = result.Children.Single(x => x.Name == "Mia");

        var scheme = child.Schemes.Single(x => x.SchemeCode == SchemeCode.FifteenHoursForDisadvantagedChildren);

        Assert.Equal("WhenToApply_Now", scheme.WhenToApply);
    }

    [Fact]
    public void Map_FifteenHoursForDisadvantagedChildren_EligibleInFuture_ReturnsApplyFromDate()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response);

        var child = result.Children.Single(x => x.Name == "Emily");

        var scheme = child.Schemes.Single(x => x.SchemeCode == SchemeCode.FifteenHoursForDisadvantagedChildren);

        Assert.Equal("WhenToApply_FromDate", scheme.WhenToApply);
    }

    [Fact]
    public void Map_ThirtyHoursForWorkingFamilies_EligibleNow_ReturnsNow()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response);

        var child = result.Children.Single(x => x.Name == "Katie");

        var scheme = child.Schemes.Single(x => x.SchemeCode == SchemeCode.ThirtyHoursForWorkingFamilies);

        Assert.Equal("WhenToApply_Now", scheme.WhenToApply);
    }

    [Fact]
    public void Map_ThirtyHoursForWorkingFamilies_BornAndEligibleToApplyNow_ReturnsNow()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response);

        var child = result.Children.Single(x => x.Name == "Jack");

        var scheme = child.Schemes.Single(x => x.SchemeCode == SchemeCode.ThirtyHoursForWorkingFamilies);

        Assert.Equal("WhenToApply_Now", scheme.WhenToApply);
    }

    [Fact]
    public void Map_ThirtyHoursForWorkingFamilies_UnbornAndEligibleInFuture_ReturnsWhenTheyAreTwentyThreeWeeksOld()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response);

        var child = result.Children.Single(x => x.Name == "Baby Smith");

        var scheme = child.Schemes.Single(x => x.SchemeCode == SchemeCode.ThirtyHoursForWorkingFamilies);

        Assert.Equal("WhenToApply_WhenTwentyThreeWeeksOld", scheme.WhenToApply);
    }

    [Fact]
    public void Map_ThirtyHoursForWorkingFamilies_BornAndEligibleToApplyInFuture_ReturnsApplyFromDate()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response);

        var child = result.Children.Single(x => x.Name == "Oliver");

        var scheme = child.Schemes.Single(x =>
            x.SchemeCode == SchemeCode.ThirtyHoursForWorkingFamilies);

        Assert.Equal("WhenToApply_FromDate", scheme.WhenToApply);
    }

    [Fact]
    public void Map_TaxFreeChildcare_EligibleNow_ReturnsNow()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response);

        var child = result.Children.Single(x => x.Name == "Katie");

        var scheme = child.Schemes.Single(x => x.SchemeCode == SchemeCode.TaxFreeChildcare);

        Assert.Equal("WhenToApply_Now", scheme.WhenToApply);
    }

    [Fact]
    public void Map_TaxFreeChildcare_EligibleInFuture_ReturnsWhenTheyAreBorn()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response);

        var child = result.Children.Single(x => x.Name == "Baby Smith");

        var scheme = child.Schemes.Single(x => x.SchemeCode == SchemeCode.TaxFreeChildcare);

        Assert.Equal("WhenToApply_WhenBorn", scheme.WhenToApply);
    }

    [Fact]
    public void Map_UniversalCreditChildcare_UnbornEligibleInFuture_ReturnsWhenTheyAreBorn()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response);

        var child = result.Children.Single(x => x.Name == "Baby Smith");

        var scheme = child.Schemes.Single(x => x.SchemeCode == SchemeCode.UniversalCreditChildcare);

        Assert.Equal("WhenToApply_WhenBorn", scheme.WhenToApply);
    }

    [Fact]
    public void Map_UniversalCreditChildcare_EligibleNow_ReturnsNow()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response);

        var child = result.Children.Single(x => x.Name == "Alfie");

        var scheme = child.Schemes.Single(x => x.SchemeCode == SchemeCode.UniversalCreditChildcare);

        Assert.Equal("WhenToApply_Now", scheme.WhenToApply);
    }

    [Fact]
    public void Map_DoesNotShowThirtyHourWarning_WhenThirtyHoursSchemeMissing()
    {
        var response = CreateTestEntitlementResponse();

        var result = _mapper.Map(response);

        var child = result.Children.Single(x => x.Name == "Tom");

        Assert.False(child.ShowThirtyHourWarning);
    }
}



