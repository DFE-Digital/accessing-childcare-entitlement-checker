using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using Stateless;

namespace AccessingChildcareEntitlementChecker.Web.Services.Navigation.Steps;

public class UserWorkflowSteps : IWorkflowStep
{
    public void Configure(StateMachine<Page, NavigationAction> machine, JourneyState state, string? childId)
    {
        machine.Configure(Page.UserAge)
            .Permit(NavigationAction.Next, Page.Nationality)
            .Permit(NavigationAction.Back, Page.CheckChildDetails);

        machine.Configure(Page.Nationality)
            .PermitDynamic(NavigationAction.Next, () => GetNationalityNextPage(state))
            .Permit(NavigationAction.Back, Page.UserAge);

        machine.Configure(Page.SettledStatus)
            .Permit(NavigationAction.Next, Page.PaidWork)
            .Permit(NavigationAction.Back, Page.Nationality);

        machine.Configure(Page.PaidWork)
            .PermitDynamic(NavigationAction.Next, () => GetPaidWorkNextPage(state))
            .PermitDynamic(NavigationAction.Back, () => GetPaidWorkBackPage(state));

        machine.Configure(Page.WorkStatus)
            .PermitDynamic(NavigationAction.Next, () => GetWorkStatusNextPage(state))
            .Permit(NavigationAction.Back, Page.PaidWork);

        machine.Configure(Page.SelfEmployedDuration)
            .PermitDynamic(NavigationAction.Next, () => GetSelfEmployedDurationNextPage(state))
            .Permit(NavigationAction.Back, Page.WorkStatus);

        machine.Configure(Page.WeeklyEarnings)
            .PermitDynamic(NavigationAction.Next, () => GetWeeklyEarningsNextPage(state))
            .PermitDynamic(NavigationAction.Back, () => GetWeeklyEarningsBackPage(state));

        machine.Configure(Page.YearlyEarnings)
            .PermitDynamic(NavigationAction.Next, () => GetYearlyEarningsNextPage(state))
            .Permit(NavigationAction.Back, Page.WeeklyEarnings);

        machine.Configure(Page.UniversalCredit)
            .Permit(NavigationAction.Next, Page.Benefits)
            .PermitDynamic(NavigationAction.Back, () => GetUniversalCreditBackPage(state));

        machine.Configure(Page.Benefits)
            .Permit(NavigationAction.Next, Page.ChildcareSupport)
            .PermitDynamic(NavigationAction.Back, () => GetBenefitsBackPage(state));

        machine.Configure(Page.ChildcareSupport)
            .PermitDynamic(NavigationAction.Next, () => GetChildcareSupportNextPage(state))
            .Permit(NavigationAction.Back, Page.Benefits);

        machine.Configure(Page.ChildcareVoucherReceipt)
            .Permit(NavigationAction.Next, Page.HasPartner)
            .Permit(NavigationAction.Back, Page.ChildcareSupport);

        machine.Configure(Page.HasPartner)
            .PermitDynamic(NavigationAction.Next, () => GetHasPartnerNextPage(state))
            .PermitDynamic(NavigationAction.Back, () => GetHasPartnerBackPage(state));
    }

    private static Page GetNationalityNextPage(JourneyState state) =>
        state.Nationality == NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland
            ? Page.SettledStatus
            : Page.PaidWork;

    private static Page GetPaidWorkNextPage(JourneyState state) =>
        state.PaidWork switch
        {
            PaidWorkOption.Yes => Page.WorkStatus,
            PaidWorkOption.OnLeave => Page.TypeOfLeave,
            _ => Page.UniversalCredit
        };

    private static Page GetPaidWorkBackPage(JourneyState state) =>
        state.Nationality == NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland
            ? Page.SettledStatus
            : Page.Nationality;

    private static Page GetWorkStatusNextPage(JourneyState state) =>
        state.WorkStatus.Contains(WorkStatusOption.SelfEmployed)
            ? Page.SelfEmployedDuration
            : Page.WeeklyEarnings;

    private static Page GetSelfEmployedDurationNextPage(JourneyState state) =>
        state.SelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months
            ? Page.UniversalCredit
            : Page.WeeklyEarnings;

    private static Page GetWeeklyEarningsNextPage(JourneyState state) =>
        state.WeeklyEarnings == WeeklyEarningsOption.AboveThreshold
            ? Page.YearlyEarnings
            : Page.UniversalCredit;

    private static Page GetWeeklyEarningsBackPage(JourneyState state) =>
        state.WorkStatus.Contains(WorkStatusOption.SelfEmployed)
            ? Page.SelfEmployedDuration
            : Page.WorkStatus;

    private static Page GetYearlyEarningsNextPage(JourneyState state) =>
        state.YearlyEarnings == YearlyEarningsOption.AboveThreshold
            ? Page.Benefits
            : Page.UniversalCredit;

    private static Page GetUniversalCreditBackPage(JourneyState state)
    {
        if (state.PaidWork == PaidWorkOption.No)
        {
            return Page.PaidWork;
        }
        if (state.SelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months)
        {
            return Page.SelfEmployedDuration;
        }
        if (state.WeeklyEarnings == WeeklyEarningsOption.AboveThreshold)
        {
            return Page.YearlyEarnings;
        }
        return Page.WeeklyEarnings;
    }

    private static Page GetBenefitsBackPage(JourneyState state) =>
        state.YearlyEarnings == YearlyEarningsOption.AboveThreshold
            ? Page.YearlyEarnings
            : Page.UniversalCredit;

    private static Page GetChildcareSupportNextPage(JourneyState state) =>
        state.ChildcareSupport.Contains(ChildcareSupportOption.ChildcareVouchers)
            ? Page.ChildcareVoucherReceipt
            : Page.HasPartner;

    private static Page GetHasPartnerNextPage(JourneyState state) =>
        state.HasPartner == true
            ? Page.PartnerAge
            : Page.CheckAnswers;

    private static Page GetHasPartnerBackPage(JourneyState state) =>
        state.ChildcareSupport.Contains(ChildcareSupportOption.ChildcareVouchers)
            ? Page.ChildcareVoucherReceipt
            : Page.ChildcareSupport;
}
