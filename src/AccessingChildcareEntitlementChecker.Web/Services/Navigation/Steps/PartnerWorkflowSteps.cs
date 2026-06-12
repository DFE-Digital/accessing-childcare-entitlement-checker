using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using Stateless;

namespace AccessingChildcareEntitlementChecker.Web.Services.Navigation.Steps;

public class PartnerWorkflowSteps : IWorkflowStep
{
    public void Configure(StateMachine<Page, NavigationAction> machine, JourneyState state, string? childId)
    {
        machine.Configure(Page.PartnerAge)
            .Permit(NavigationAction.Next, Page.PartnerNationality)
            .Permit(NavigationAction.Back, Page.HasPartner);

        machine.Configure(Page.PartnerNationality)
            .PermitDynamic(NavigationAction.Next, () => GetPartnerNationalityNextPage(state))
            .Permit(NavigationAction.Back, Page.PartnerAge);

        machine.Configure(Page.PartnerSettledStatus)
            .Permit(NavigationAction.Next, Page.PartnerPaidWork)
            .Permit(NavigationAction.Back, Page.PartnerNationality);

        machine.Configure(Page.PartnerPaidWork)
            .PermitDynamic(NavigationAction.Next, () => GetPartnerPaidWorkNextPage(state))
            .PermitDynamic(NavigationAction.Back, () => GetPartnerPaidWorkBackPage(state));

        machine.Configure(Page.PartnerWorkStatus)
            .PermitDynamic(NavigationAction.Next, () => GetPartnerWorkStatusNextPage(state))
            .Permit(NavigationAction.Back, Page.PartnerPaidWork);

        machine.Configure(Page.PartnerSelfEmployedDuration)
            .PermitDynamic(NavigationAction.Next, () => GetPartnerSelfEmployedDurationNextPage(state))
            .Permit(NavigationAction.Back, Page.PartnerWorkStatus);

        machine.Configure(Page.PartnerWeeklyEarnings)
            .PermitDynamic(NavigationAction.Next, () => GetPartnerWeeklyEarningsNextPage(state))
            .PermitDynamic(NavigationAction.Back, () => GetPartnerWeeklyEarningsBackPage(state));

        machine.Configure(Page.PartnerYearlyEarnings)
            .Permit(NavigationAction.Next, Page.PartnerBenefits)
            .Permit(NavigationAction.Back, Page.PartnerWeeklyEarnings);

        machine.Configure(Page.PartnerBenefits)
            .Permit(NavigationAction.Next, Page.PartnerChildcareSupport)
            .PermitDynamic(NavigationAction.Back, () => GetPartnerBenefitsBackPage(state));

        machine.Configure(Page.PartnerChildcareSupport)
            .PermitDynamic(NavigationAction.Next, () => GetPartnerChildcareSupportNextPage(state))
            .Permit(NavigationAction.Back, Page.PartnerBenefits);

        machine.Configure(Page.PartnerChildcareVoucherReceipt)
            .Permit(NavigationAction.Next, Page.CheckAnswers)
            .Permit(NavigationAction.Back, Page.PartnerChildcareSupport);
    }

    private static Page GetPartnerNationalityNextPage(JourneyState state) =>
        state.PartnerNationality == NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland
            ? Page.PartnerSettledStatus
            : Page.PartnerPaidWork;

    private static Page GetPartnerPaidWorkNextPage(JourneyState state) =>
        state.PartnerPaidWork switch
        {
            PartnerPaidWorkOption.Yes => Page.PartnerWorkStatus,
            PartnerPaidWorkOption.OnLeave => Page.PartnerTypeOfLeave,
            PartnerPaidWorkOption.No => Page.PartnerBenefits,
            _ => Page.PartnerBenefits
        };

    private static Page GetPartnerPaidWorkBackPage(JourneyState state) =>
        state.PartnerNationality == NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland
            ? Page.PartnerSettledStatus
            : Page.PartnerNationality;

    private static Page GetPartnerWorkStatusNextPage(JourneyState state) =>
        state.PartnerWorkStatus.Contains(WorkStatusOption.SelfEmployed)
            ? Page.PartnerSelfEmployedDuration
            : Page.PartnerWeeklyEarnings;

    private static Page GetPartnerSelfEmployedDurationNextPage(JourneyState state) =>
        state.PartnerSelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months
            ? Page.PartnerBenefits
            : Page.PartnerWeeklyEarnings;

    private static Page GetPartnerWeeklyEarningsNextPage(JourneyState state) =>
        state.PartnerWeeklyEarnings == WeeklyEarningsOption.AboveThreshold
            ? Page.PartnerYearlyEarnings
            : Page.PartnerBenefits;

    private static Page GetPartnerWeeklyEarningsBackPage(JourneyState state) =>
        state.PartnerWorkStatus.Contains(WorkStatusOption.SelfEmployed)
            ? Page.PartnerSelfEmployedDuration
            : Page.PartnerWorkStatus;

    private static Page GetPartnerBenefitsBackPage(JourneyState state)
    {
        if (state.PartnerYearlyEarnings == YearlyEarningsOption.AboveThreshold)
        {
            return Page.PartnerYearlyEarnings;
        }
        if (state.PartnerWeeklyEarnings == WeeklyEarningsOption.AboveThreshold)
        {
            return Page.PartnerYearlyEarnings;
        }
        if (state.PartnerSelfEmployedDuration == SelfEmployedDurationOption.LessThan12Months)
        {
            return Page.PartnerSelfEmployedDuration;
        }
        if (state.PartnerPaidWork == PartnerPaidWorkOption.No)
        {
            return Page.PartnerPaidWork;
        }
        return Page.PartnerWeeklyEarnings;
    }

    private static Page GetPartnerChildcareSupportNextPage(JourneyState state) =>
        state.PartnerChildcareSupport.Contains(PartnerChildcareSupportOption.ChildcareVouchers)
            ? Page.PartnerChildcareVoucherReceipt
            : Page.CheckAnswers;
}
