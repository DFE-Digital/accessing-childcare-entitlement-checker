using System.Collections.Generic;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation;
using AccessingChildcareEntitlementChecker.Web.Services.Navigation.Steps;
using Stateless;
using Xunit;

namespace AccessingChildcareEntitlementChecker.UnitTests.Services.Navigation.Steps;

public class UserWorkflowStepsTests
{
    private readonly UserWorkflowSteps _step = new();
    private readonly JourneyState _state = new();

    private StateMachine<Page, NavigationAction> GetConfiguredMachine(Page startState)
    {
        var machine = new StateMachine<Page, NavigationAction>(startState);
        _step.Configure(machine, _state, null);
        return machine;
    }

    [Fact]
    public void UserAge_Transitions()
    {
        var machineNext = GetConfiguredMachine(Page.UserAge);
        machineNext.Fire(NavigationAction.Next);
        Assert.Equal(Page.Nationality, machineNext.State);

        var machineBack = GetConfiguredMachine(Page.UserAge);
        machineBack.Fire(NavigationAction.Back);
        Assert.Equal(Page.CheckChildDetails, machineBack.State);
    }

    [Theory]
    [InlineData(NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, Page.SettledStatus)]
    [InlineData(NationalityOption.BritishOrIrishCitizen, Page.PaidWork)]
    [InlineData(NationalityOption.CitizenOfADifferentCountry, Page.PaidWork)]
    public void Nationality_Next_Transitions(NationalityOption nationality, Page expectedPage)
    {
        _state.Nationality = nationality;
        var machine = GetConfiguredMachine(Page.Nationality);
        machine.Fire(NavigationAction.Next);
        Assert.Equal(expectedPage, machine.State);
    }

    [Fact]
    public void Nationality_Back_ReturnsUserAge()
    {
        var machine = GetConfiguredMachine(Page.Nationality);
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.UserAge, machine.State);
    }

    [Fact]
    public void SettledStatus_Transitions()
    {
        var machineNext = GetConfiguredMachine(Page.SettledStatus);
        machineNext.Fire(NavigationAction.Next);
        Assert.Equal(Page.PaidWork, machineNext.State);

        var machineBack = GetConfiguredMachine(Page.SettledStatus);
        machineBack.Fire(NavigationAction.Back);
        Assert.Equal(Page.Nationality, machineBack.State);
    }

    [Theory]
    [InlineData(PaidWorkOption.Yes, Page.WorkStatus)]
    [InlineData(PaidWorkOption.OnLeave, Page.TypeOfLeave)]
    [InlineData(PaidWorkOption.No, Page.UniversalCredit)]
    public void PaidWork_Next_Transitions(PaidWorkOption paidWork, Page expectedPage)
    {
        _state.PaidWork = paidWork;
        var machine = GetConfiguredMachine(Page.PaidWork);
        machine.Fire(NavigationAction.Next);
        Assert.Equal(expectedPage, machine.State);
    }

    [Theory]
    [InlineData(NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, Page.SettledStatus)]
    [InlineData(NationalityOption.BritishOrIrishCitizen, Page.Nationality)]
    public void PaidWork_Back_Transitions(NationalityOption nationality, Page expectedPage)
    {
        _state.Nationality = nationality;
        var machine = GetConfiguredMachine(Page.PaidWork);
        machine.Fire(NavigationAction.Back);
        Assert.Equal(expectedPage, machine.State);
    }

    [Theory]
    [InlineData(true, Page.SelfEmployedDuration)]
    [InlineData(false, Page.WeeklyEarnings)]
    public void WorkStatus_Next_Transitions(bool isSelfEmployed, Page expectedPage)
    {
        if (isSelfEmployed)
        {
            _state.WorkStatus = [WorkStatusOption.SelfEmployed];
        }
        else
        {
            _state.WorkStatus = [WorkStatusOption.PaidEmployment];
        }

        var machine = GetConfiguredMachine(Page.WorkStatus);
        machine.Fire(NavigationAction.Next);
        Assert.Equal(expectedPage, machine.State);
    }

    [Fact]
    public void WorkStatus_Back_ReturnsPaidWork()
    {
        var machine = GetConfiguredMachine(Page.WorkStatus);
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.PaidWork, machine.State);
    }

    [Theory]
    [InlineData(SelfEmployedDurationOption.LessThan12Months, Page.UniversalCredit)]
    [InlineData(SelfEmployedDurationOption.NotLessThan12Months, Page.WeeklyEarnings)]
    public void SelfEmployedDuration_Next_Transitions(SelfEmployedDurationOption duration, Page expectedPage)
    {
        _state.SelfEmployedDuration = duration;
        var machine = GetConfiguredMachine(Page.SelfEmployedDuration);
        machine.Fire(NavigationAction.Next);
        Assert.Equal(expectedPage, machine.State);
    }

    [Fact]
    public void SelfEmployedDuration_Back_ReturnsWorkStatus()
    {
        var machine = GetConfiguredMachine(Page.SelfEmployedDuration);
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.WorkStatus, machine.State);
    }

    [Theory]
    [InlineData(WeeklyEarningsOption.AboveThreshold, Page.YearlyEarnings)]
    [InlineData(WeeklyEarningsOption.BelowThreshold, Page.UniversalCredit)]
    public void WeeklyEarnings_Next_Transitions(WeeklyEarningsOption earnings, Page expectedPage)
    {
        _state.WeeklyEarnings = earnings;
        var machine = GetConfiguredMachine(Page.WeeklyEarnings);
        machine.Fire(NavigationAction.Next);
        Assert.Equal(expectedPage, machine.State);
    }

    [Theory]
    [InlineData(true, Page.SelfEmployedDuration)]
    [InlineData(false, Page.WorkStatus)]
    public void WeeklyEarnings_Back_Transitions(bool isSelfEmployed, Page expectedPage)
    {
        if (isSelfEmployed)
        {
            _state.WorkStatus = [WorkStatusOption.SelfEmployed];
        }
        else
        {
            _state.WorkStatus = [WorkStatusOption.PaidEmployment];
        }

        var machine = GetConfiguredMachine(Page.WeeklyEarnings);
        machine.Fire(NavigationAction.Back);
        Assert.Equal(expectedPage, machine.State);
    }

    [Theory]
    [InlineData(YearlyEarningsOption.AboveThreshold, Page.Benefits)]
    [InlineData(YearlyEarningsOption.BelowThreshold, Page.UniversalCredit)]
    public void YearlyEarnings_Next_Transitions(YearlyEarningsOption earnings, Page expectedPage)
    {
        _state.YearlyEarnings = earnings;
        var machine = GetConfiguredMachine(Page.YearlyEarnings);
        machine.Fire(NavigationAction.Next);
        Assert.Equal(expectedPage, machine.State);
    }

    [Fact]
    public void YearlyEarnings_Back_ReturnsWeeklyEarnings()
    {
        var machine = GetConfiguredMachine(Page.YearlyEarnings);
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.WeeklyEarnings, machine.State);
    }

    [Fact]
    public void UniversalCredit_Next_ReturnsBenefits()
    {
        var machine = GetConfiguredMachine(Page.UniversalCredit);
        machine.Fire(NavigationAction.Next);
        Assert.Equal(Page.Benefits, machine.State);
    }

    [Theory]
    // 1. PaidWork == No -> PaidWork
    [InlineData(PaidWorkOption.No, null, null, Page.PaidWork)]
    // 2. SelfEmployedDuration == LessThan12Months -> SelfEmployedDuration
    [InlineData(PaidWorkOption.Yes, SelfEmployedDurationOption.LessThan12Months, null, Page.SelfEmployedDuration)]
    // 3. WeeklyEarnings == AboveThreshold -> YearlyEarnings
    [InlineData(PaidWorkOption.Yes, SelfEmployedDurationOption.NotLessThan12Months, WeeklyEarningsOption.AboveThreshold, Page.YearlyEarnings)]
    // 4. Fallback -> WeeklyEarnings
    [InlineData(PaidWorkOption.Yes, SelfEmployedDurationOption.NotLessThan12Months, WeeklyEarningsOption.BelowThreshold, Page.WeeklyEarnings)]
    public void UniversalCredit_Back_Transitions(PaidWorkOption paidWork, SelfEmployedDurationOption? duration, WeeklyEarningsOption? earnings, Page expectedPage)
    {
        _state.PaidWork = paidWork;
        _state.SelfEmployedDuration = duration;
        _state.WeeklyEarnings = earnings;

        var machine = GetConfiguredMachine(Page.UniversalCredit);
        machine.Fire(NavigationAction.Back);
        Assert.Equal(expectedPage, machine.State);
    }

    [Fact]
    public void Benefits_Next_ReturnsChildcareSupport()
    {
        var machine = GetConfiguredMachine(Page.Benefits);
        machine.Fire(NavigationAction.Next);
        Assert.Equal(Page.ChildcareSupport, machine.State);
    }

    [Theory]
    [InlineData(YearlyEarningsOption.AboveThreshold, Page.YearlyEarnings)]
    [InlineData(YearlyEarningsOption.BelowThreshold, Page.UniversalCredit)]
    public void Benefits_Back_Transitions(YearlyEarningsOption yearlyEarnings, Page expectedPage)
    {
        _state.YearlyEarnings = yearlyEarnings;
        var machine = GetConfiguredMachine(Page.Benefits);
        machine.Fire(NavigationAction.Back);
        Assert.Equal(expectedPage, machine.State);
    }

    [Theory]
    [InlineData(true, Page.ChildcareVoucherReceipt)]
    [InlineData(false, Page.HasPartner)]
    public void ChildcareSupport_Next_Transitions(bool hasVouchers, Page expectedPage)
    {
        if (hasVouchers)
        {
            _state.ChildcareSupport = [ChildcareSupportOption.ChildcareVouchers];
        }
        else
        {
            _state.ChildcareSupport = [ChildcareSupportOption.None];
        }

        var machine = GetConfiguredMachine(Page.ChildcareSupport);
        machine.Fire(NavigationAction.Next);
        Assert.Equal(expectedPage, machine.State);
    }

    [Fact]
    public void ChildcareSupport_Back_ReturnsBenefits()
    {
        var machine = GetConfiguredMachine(Page.ChildcareSupport);
        machine.Fire(NavigationAction.Back);
        Assert.Equal(Page.Benefits, machine.State);
    }

    [Fact]
    public void ChildcareVoucherReceipt_Transitions()
    {
        var machineNext = GetConfiguredMachine(Page.ChildcareVoucherReceipt);
        machineNext.Fire(NavigationAction.Next);
        Assert.Equal(Page.HasPartner, machineNext.State);

        var machineBack = GetConfiguredMachine(Page.ChildcareVoucherReceipt);
        machineBack.Fire(NavigationAction.Back);
        Assert.Equal(Page.ChildcareSupport, machineBack.State);
    }

    [Theory]
    [InlineData(true, Page.PartnerAge)]
    [InlineData(false, Page.CheckAnswers)]
    public void HasPartner_Next_Transitions(bool hasPartner, Page expectedPage)
    {
        _state.HasPartner = hasPartner;
        var machine = GetConfiguredMachine(Page.HasPartner);
        machine.Fire(NavigationAction.Next);
        Assert.Equal(expectedPage, machine.State);
    }

    [Theory]
    [InlineData(true, Page.ChildcareVoucherReceipt)]
    [InlineData(false, Page.ChildcareSupport)]
    public void HasPartner_Back_Transitions(bool hasVouchers, Page expectedPage)
    {
        if (hasVouchers)
        {
            _state.ChildcareSupport = [ChildcareSupportOption.ChildcareVouchers];
        }
        else
        {
            _state.ChildcareSupport = [ChildcareSupportOption.None];
        }

        var machine = GetConfiguredMachine(Page.HasPartner);
        machine.Fire(NavigationAction.Back);
        Assert.Equal(expectedPage, machine.State);
    }
}
