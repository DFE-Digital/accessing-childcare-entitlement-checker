using AccessingChildcareEntitlementChecker.Web.Controllers;
using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace AccessingChildcareEntitlementChecker.Web;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddJourneyServices(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeFactory, DateTimeFactory>();
        services.AddScoped<ITodayFactory, UkTodayFactory>();
        services.AddScoped<IJourneySession, JourneySession>();
        services.AddScoped(sp =>
        {
            var journeySession = sp.GetRequiredService<IJourneySession>();
            return journeySession.Get();
        });

        var builder = new JourneyBuilder();
        var home = builder.Add<HomeController>("/start-page", nameof(HomeController.Start));
        var location = builder.Add<HomeController>("/where-do-you-live", nameof(HomeController.Location));
        var childName = builder.Add<IntroductionController>("/children/{childId}/add-child-details", nameof(IntroductionController.ChildName));
        var isChildBorn = builder.Add<IntroductionController>("/children/{childId}/has-the-child-been-born", nameof(IntroductionController.IsChildBorn));

        var childBirthDate = builder.Add<BornChildDetailsController>("/children/{childId}/childs-date-of-birth", nameof(BornChildDetailsController.ChildBirthDate));
        var childRelationship = builder.Add<BornChildDetailsController>("/children/{childId}/relationship", nameof(BornChildDetailsController.ChildRelationship));
        var childSupport = builder.Add<BornChildDetailsController>("/children/{childId}/child-support", nameof(BornChildDetailsController.ChildSupport));

        var childDueDate = builder.Add<ExpectedChildDetailsController>("/children/{childId}/expectant-childs-due-date", nameof(ExpectedChildDetailsController.ChildDueDate));
        var expectedChildRelationship = builder.Add<ExpectedChildDetailsController>("/children/{childId}/relationship", nameof(ExpectedChildDetailsController.ExpectedChildRelationship));

        var checkChildren = builder.Add<SummaryController>("/children/check-childs-details", nameof(SummaryController.CheckChildDetails));

        var userAge = builder.Add<UserController>("/age", nameof(UserController.UserAge));
        var nationality = builder.Add<UserController>("/nationality", nameof(UserController.Nationality));
        var settledStatus = builder.Add<UserController>("/settled-status", nameof(UserController.SettledStatus));
        var paidWork = builder.Add<UserController>("/work-status/work", nameof(UserController.PaidWork));
        var workStatus = builder.Add<UserController>("/work-status/status", nameof(UserController.WorkStatus));
        var selfEmployedDuration = builder.Add<UserController>("/work-status/self-employed-duration", nameof(UserController.SelfEmployedDuration));
        var typeOfLeave = builder.Add<UserController>("/leave/type-of-leave", nameof(UserController.TypeOfLeave));
        var weeklyEarnings = builder.Add<UserController>("/leave/weekly-earnings", nameof(UserController.WeeklyEarnings));
        var yearlyEarnings = builder.Add<UserController>("/leave/yearly-earnings", nameof(UserController.YearlyEarnings));
        var universalCredit = builder.Add<UserController>("/universal-credit", nameof(UserController.UniversalCredit));
        var benefits = builder.Add<UserController>("/benefits", nameof(UserController.Benefits));
        var childcareSupport = builder.Add<UserController>("/childcare-support", nameof(UserController.ChildcareSupport));
        var childcareVouchers = builder.Add<UserController>("/childcare-vouchers", nameof(UserController.ChildcareVoucherReceipt));
        var hasPartner = builder.Add<UserController>("/has-partner", nameof(UserController.HasPartner));
        var partnerAge = builder.Add<PartnerController>("/partner-age", nameof(PartnerController.PartnerAge));
        var partnerNationality = builder.Add<PartnerController>("/partner-nationality", nameof(PartnerController.PartnerNationality));
        var partnerSettledStatus = builder.Add<PartnerController>("/partner-settled-status", nameof(PartnerController.PartnerSettledStatus));
        var partnerPaidWork = builder.Add<PartnerController>("/partner-work-status/work", nameof(PartnerController.PartnerPaidWork));
        var partnerWorkStatus = builder.Add<PartnerController>("/partner-work-status/status", nameof(PartnerController.PartnerWorkStatus));
        var partnerLeaveType = builder.Add<PartnerController>("/partner-leave/type-of-leave", nameof(PartnerController.PartnerTypeOfLeave));
        var partnerSelfEmployedDuration = builder.Add<PartnerController>("/partner-leave/self-employed-duration", nameof(PartnerController.PartnerSelfEmployedDuration));
        var partnerWeeklyEarnings = builder.Add<PartnerController>("/partner-leave/weekly-earnings", nameof(PartnerController.PartnerWeeklyEarnings));
        var partnerYearlyEarnings = builder.Add<PartnerController>("/partner-leave/yearly-earnings", nameof(PartnerController.PartnerYearlyEarnings));
        var partnerBenefits = builder.Add<PartnerController>("/partner-benefits", nameof(PartnerController.PartnerBenefits));
        var partnerChildcareSupport = builder.Add<PartnerController>("/partner-childcare-support", nameof(PartnerController.PartnerChildcareSupport));
        var partnerChildcareVouchers = builder.Add<PartnerController>("/partner-childcare-vouchers", nameof(PartnerController.PartnerChildcareVoucherReceipt));

        var checkAnswers = builder.Add<SummaryController>("/check-your-answers", nameof(SummaryController.CheckAnswers));
        var results = builder.Add<ResultsController>("/results", nameof(ResultsController.Results));

        home
            .Then(location)
            .Then(childName)
            .Then(isChildBorn);

        isChildBorn
            .When(s => s.Children, c => c.BirthStatus == BirthStatus.Born, childBirthDate)
            .Then(childDueDate);

        childBirthDate
            .Then(childRelationship)
            .Then(childSupport)
            .Then(checkChildren);

        childDueDate
            .Then(expectedChildRelationship)
            .Then(checkChildren);

        checkChildren
            .Then(userAge);

        userAge
            .Then(nationality);

        nationality
            .When(s => s.Nationality == NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, settledStatus)
            .Then(paidWork);

        settledStatus
            .Then(paidWork);

        paidWork
            .When(s => s.PaidWork == PaidWorkOption.Yes, workStatus)
            .When(s => s.PaidWork == PaidWorkOption.OnLeave, typeOfLeave)
            .Then(universalCredit);

        workStatus
            .When(s => s.WorkStatus.Contains(WorkStatusOption.SelfEmployed), selfEmployedDuration)
            .Then(weeklyEarnings);

        selfEmployedDuration
            .When(s => s.SelfEmployedDuration == SelfEmployedDurationOption.NotLessThan12Months, weeklyEarnings)
            .Then(universalCredit);

        weeklyEarnings
            .When(s => s.WeeklyEarnings == WeeklyEarningsOption.AboveThreshold, yearlyEarnings)
            .Then(universalCredit);

        yearlyEarnings
            .When(s => s.YearlyEarnings == YearlyEarningsOption.AboveThreshold, benefits)
            .Then(universalCredit);

        universalCredit
            .Then(benefits);

        benefits
            .Then(childcareSupport);

        childcareSupport
            .When(s => s.ChildcareSupport.Contains(ChildcareSupportOption.ChildcareVouchers), childcareVouchers)
            .Then(hasPartner);

        childcareVouchers
            .Then(hasPartner);

        hasPartner
            .When(s => s.HasPartner ?? false, partnerAge)
            .Then(checkAnswers);

        partnerAge
            .Then(partnerNationality);

        partnerNationality
            .When(s => s.PartnerNationality == NationalityOption.CitizenOfAnEUCountryEEACountryOrSwitzerland, partnerSettledStatus)
            .Then(partnerPaidWork);

        partnerSettledStatus
            .Then(partnerPaidWork);

        partnerPaidWork
            .When(s => s.PartnerPaidWork == PartnerPaidWorkOption.Yes, partnerWorkStatus)
            .When(s => s.PartnerPaidWork == PartnerPaidWorkOption.OnLeave, partnerLeaveType)
            .Then(partnerBenefits);

        partnerWorkStatus
            .When(s => s.PartnerWorkStatus.Contains(WorkStatusOption.SelfEmployed), partnerSelfEmployedDuration)
            .Then(partnerWeeklyEarnings);

        partnerSelfEmployedDuration
            .When(s => s.PartnerSelfEmployedDuration == SelfEmployedDurationOption.NotLessThan12Months, partnerWeeklyEarnings)
            .Then(partnerBenefits);

        partnerWeeklyEarnings
            .When(s => s.PartnerWeeklyEarnings == WeeklyEarningsOption.AboveThreshold, partnerYearlyEarnings)
            .Then(partnerBenefits);

        partnerYearlyEarnings
            .When(s => s.PartnerYearlyEarnings == YearlyEarningsOption.AboveThreshold, partnerBenefits)
            .Then(partnerBenefits);

        partnerBenefits
            .Then(partnerChildcareSupport);

        partnerChildcareSupport
            .When(s => s.PartnerChildcareSupport.Contains(PartnerChildcareSupportOption.ChildcareVouchers), partnerChildcareVouchers)
            .Then(checkAnswers);

        partnerChildcareVouchers
            .Then(checkAnswers);

        var journey = builder.Build(home);
        services.AddSingleton(sp => journey);

        return services;
    }
}
