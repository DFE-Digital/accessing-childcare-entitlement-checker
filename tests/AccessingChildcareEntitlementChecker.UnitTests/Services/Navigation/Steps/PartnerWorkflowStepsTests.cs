using System.Collections.Generic;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation.Steps;
using Stateless;
using Xunit;

namespace AccessingChildcareEntitlementChecker.UnitTests.Services.Navigation.Steps;

public class PartnerWorkflowStepsTests
{
    private readonly PartnerWorkflowSteps _step = new();
    private readonly JourneyState _state = new();

    private StateMachine<Page, NavigationAction> GetConfiguredMachine(Page startState)
    {
        var machine = new StateMachine<Page, NavigationAction>(startState);
        _step.Configure(machine, _state, null);
        return machine;
    }

    [Fact]
    public void PartnerAge_Transitions()
    {
        var machineNext = GetConfiguredMachine(Page.PartnerAge);
        machineNext.Fire(NavigationAction.Next);
        Assert.Equal(Page.PartnerNationality, machineNext.State);

        var machineBack = GetConfiguredMachine(Page.PartnerAge);
        machineBack.Fire(NavigationAction.Back);
        Assert.Equal(Page.HasPartner, machineBack.State);
    }

    [Theory]
    [InlineData(NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, Page.PartnerSettledStatus)]
    [InlineData(NationalityOption.BritishOrIrishCitizen, Page.PartnerPaidWork)]
    [InlineData(NationalityOption.CitizenOfADifferentCountry, Page.PartnerPaidWork)]
    public void PartnerNationality_Next_Transitions(NationalityOption nationality, Page expectedPage)
    {
        _state.PartnerNationality = nationality;
        var machine = GetConfiguredMachine(Page.PartnerNationality);
        machine.Fire(NavigationAction.Next);
        Assert.Equal(expectedPage, machine.State);
    }

    [Fact]
    public void PartnerNationality_Back_ReturnsPartnerAge()
    {
        var machine = GetConfiguredMachine(Page.PartnerNationality);
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.PartnerAge, machine.State);
    }

    [Fact]
    public void PartnerSettledStatus_Transitions()
    {
        var machineNext = GetConfiguredMachine(Page.PartnerSettledStatus);
        machineNext.Fire(NavigationAction.Next);
        Assert.Equal(Page.PartnerPaidWork, machineNext.State);

        var machineBack = GetConfiguredMachine(Page.PartnerSettledStatus);
        machineBack.Fire(NavigationAction.Back);
        Assert.Equal(Page.PartnerNationality, machineBack.State);
    }

    [Theory]
    [InlineData(PartnerPaidWorkOption.Yes, Page.PartnerWorkStatus)]
    [InlineData(PartnerPaidWorkOption.OnLeave, Page.PartnerTypeOfLeave)]
    [InlineData(PartnerPaidWorkOption.No, Page.PartnerBenefits)]
    public void PartnerPaidWork_Next_Transitions(PartnerPaidWorkOption paidWork, Page expectedPage)
    {
        _state.PartnerPaidWork = paidWork;
        var machine = GetConfiguredMachine(Page.PartnerPaidWork);
        machine.Fire(NavigationAction.Next);
        Assert.Equal(expectedPage, machine.State);
    }

    [Theory]
    [InlineData(NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, Page.PartnerSettledStatus)]
    [InlineData(NationalityOption.BritishOrIrishCitizen, Page.PartnerNationality)]
    public void PartnerPaidWork_Back_Transitions(NationalityOption nationality, Page expectedPage)
    {
        _state.PartnerNationality = nationality;
        var machine = GetConfiguredMachine(Page.PartnerPaidWork);
        machine.Fire(NavigationAction.Back);
        Assert.Equal(expectedPage, machine.State);
    }

    [Theory]
    [InlineData(true, Page.PartnerSelfEmployedDuration)]
    [InlineData(false, Page.PartnerWeeklyEarnings)]
    public void PartnerWorkStatus_Next_Transitions(bool isSelfEmployed, Page expectedPage)
    {
        if (isSelfEmployed)
        {
            _state.PartnerWorkStatus = [WorkStatusOption.SelfEmployed];
        }
        else
        {
            _state.PartnerWorkStatus = [WorkStatusOption.PaidEmployment];
        }

        var machine = GetConfiguredMachine(Page.PartnerWorkStatus);
        machine.Fire(NavigationAction.Next);
        Assert.Equal(expectedPage, machine.State);
    }

    [Fact]
    public void PartnerWorkStatus_Back_ReturnsPartnerPaidWork()
    {
        var machine = GetConfiguredMachine(Page.PartnerWorkStatus);
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.PartnerPaidWork, machine.State);
    }

    [Theory]
    [InlineData(SelfEmployedDurationOption.LessThan12Months, Page.PartnerBenefits)]
    [InlineData(SelfEmployedDurationOption.NotLessThan12Months, Page.PartnerWeeklyEarnings)]
    public void PartnerSelfEmployedDuration_Next_Transitions(SelfEmployedDurationOption duration, Page expectedPage)
    {
        _state.PartnerSelfEmployedDuration = duration;
        var machine = GetConfiguredMachine(Page.PartnerSelfEmployedDuration);
        machine.Fire(NavigationAction.Next);
        Assert.Equal(expectedPage, machine.State);
    }

    [Fact]
    public void PartnerSelfEmployedDuration_Back_ReturnsPartnerWorkStatus()
    {
        var machine = GetConfiguredMachine(Page.PartnerSelfEmployedDuration);
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.PartnerWorkStatus, machine.State);
    }

    [Theory]
    [InlineData(WeeklyEarningsOption.AboveThreshold, Page.PartnerYearlyEarnings)]
    [InlineData(WeeklyEarningsOption.BelowThreshold, Page.PartnerBenefits)]
    public void PartnerWeeklyEarnings_Next_Transitions(WeeklyEarningsOption earnings, Page expectedPage)
    {
        _state.PartnerWeeklyEarnings = earnings;
        var machine = GetConfiguredMachine(Page.PartnerWeeklyEarnings);
        machine.Fire(NavigationAction.Next);
        Assert.Equal(expectedPage, machine.State);
    }

    [Theory]
    [InlineData(true, Page.PartnerSelfEmployedDuration)]
    [InlineData(false, Page.PartnerWorkStatus)]
    public void PartnerWeeklyEarnings_Back_Transitions(bool isSelfEmployed, Page expectedPage)
    {
        if (isSelfEmployed)
        {
            _state.PartnerWorkStatus = [WorkStatusOption.SelfEmployed];
        }
        else
        {
            _state.PartnerWorkStatus = [WorkStatusOption.PaidEmployment];
        }

        var machine = GetConfiguredMachine(Page.PartnerWeeklyEarnings);
        machine.Fire(NavigationAction.Back);
        Assert.Equal(expectedPage, machine.State);
    }

    [Fact]
    public void PartnerYearlyEarnings_Transitions()
    {
        var machineNext = GetConfiguredMachine(Page.PartnerYearlyEarnings);
        machineNext.Fire(NavigationAction.Next);
        Assert.Equal(Page.PartnerBenefits, machineNext.State);

        var machineBack = GetConfiguredMachine(Page.PartnerYearlyEarnings);
        machineBack.Fire(NavigationAction.Back);
        Assert.Equal(Page.PartnerWeeklyEarnings, machineBack.State);
    }

    [Fact]
    public void PartnerBenefits_Next_ReturnsPartnerChildcareSupport()
    {
        var machine = GetConfiguredMachine(Page.PartnerBenefits);
        machine.Fire(NavigationAction.Next);
        Assert.Equal(Page.PartnerChildcareSupport, machine.State);
    }

    [Theory]
    // 1. PartnerYearlyEarnings == AboveThreshold -> PartnerYearlyEarnings
    [InlineData(PartnerPaidWorkOption.Yes, null, null, YearlyEarningsOption.AboveThreshold, Page.PartnerYearlyEarnings)]
    // 2. PartnerWeeklyEarnings == AboveThreshold -> PartnerYearlyEarnings
    [InlineData(PartnerPaidWorkOption.Yes, WeeklyEarningsOption.AboveThreshold, null, YearlyEarningsOption.BelowThreshold, Page.PartnerYearlyEarnings)]
    // 3. PartnerSelfEmployedDuration == LessThan12Months -> PartnerSelfEmployedDuration
    [InlineData(PartnerPaidWorkOption.Yes, WeeklyEarningsOption.BelowThreshold, SelfEmployedDurationOption.LessThan12Months, YearlyEarningsOption.BelowThreshold, Page.PartnerSelfEmployedDuration)]
    // 4. PartnerPaidWork == No -> PartnerPaidWork
    [InlineData(PartnerPaidWorkOption.No, WeeklyEarningsOption.BelowThreshold, SelfEmployedDurationOption.NotLessThan12Months, YearlyEarningsOption.BelowThreshold, Page.PartnerPaidWork)]
    // 5. Fallback -> PartnerWeeklyEarnings
    [InlineData(PartnerPaidWorkOption.Yes, WeeklyEarningsOption.BelowThreshold, SelfEmployedDurationOption.NotLessThan12Months, YearlyEarningsOption.BelowThreshold, Page.PartnerWeeklyEarnings)]
    public void PartnerBenefits_Back_Transitions(
        PartnerPaidWorkOption paidWork,
        WeeklyEarningsOption? earnings,
        SelfEmployedDurationOption? duration,
        YearlyEarningsOption? yearlyEarnings,
        Page expectedPage)
    {
        _state.PartnerPaidWork = paidWork;
        _state.PartnerWeeklyEarnings = earnings;
        _state.PartnerSelfEmployedDuration = duration;
        _state.PartnerYearlyEarnings = yearlyEarnings;

        var machine = GetConfiguredMachine(Page.PartnerBenefits);
        machine.Fire(NavigationAction.Back);
        Assert.Equal(expectedPage, machine.State);
    }

    [Theory]
    [InlineData(true, Page.PartnerChildcareVoucherReceipt)]
    [InlineData(false, Page.CheckAnswers)]
    public void PartnerChildcareSupport_Next_Transitions(bool hasVouchers, Page expectedPage)
    {
        if (hasVouchers)
        {
            _state.PartnerChildcareSupport = [PartnerChildcareSupportOption.ChildcareVouchers];
        }
        else
        {
            _state.PartnerChildcareSupport = [PartnerChildcareSupportOption.None];
        }

        var machine = GetConfiguredMachine(Page.PartnerChildcareSupport);
        machine.Fire(NavigationAction.Next);
        Assert.Equal(expectedPage, machine.State);
    }

    [Fact]
    public void PartnerChildcareSupport_Back_ReturnsPartnerBenefits()
    {
        var machine = GetConfiguredMachine(Page.PartnerChildcareSupport);
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.PartnerBenefits, machine.State);
    }

    [Fact]
    public void PartnerChildcareVoucherReceipt_Transitions()
    {
        var machineNext = GetConfiguredMachine(Page.PartnerChildcareVoucherReceipt);
        machineNext.Fire(NavigationAction.Next);
        Assert.Equal(Page.CheckAnswers, machineNext.State);

        var machineBack = GetConfiguredMachine(Page.PartnerChildcareVoucherReceipt);
        machineBack.Fire(NavigationAction.Back);
        Assert.Equal(Page.PartnerChildcareSupport, machineBack.State);
    }
}
