using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.Partner;
using AccessingChildcareEntitlementChecker.Web.Models.User;
using AccessingChildcareEntitlementChecker.Web.Services;
using FluentValidation;

namespace AccessingChildcareEntitlementChecker.Web.Validators;

public class JourneyStateValidator : AbstractValidator<JourneyState>
{
    public const string CheckChildDetailsRuleSet = "CheckChildDetails";
    public const string CheckAnswersRuleSet = "CheckAnswers";

    public JourneyStateValidator()
    {
        RuleSet(CheckChildDetailsRuleSet, () =>
        {
            RuleForEach(x => x.Children.Values)
               .ChildRules(child =>
               {
                   child.RuleFor(x => x.Name)
                       .NotEmpty()
                       .WithState(x => x.ChildId);

                   child.RuleFor(x => x.BirthStatus)
                       .NotNull()
                       .WithState(x => x.ChildId);

                   child.When(x => x.BirthStatus == BirthStatus.Born, () =>
                   {
                       child.RuleFor(x => x.BirthDate)
                           .NotNull()
                           .WithState(x => x.ChildId);

                       child.RuleFor(x => x.ChildSupportOptions)
                           .NotEmpty()
                           .WithState(x => x.ChildId);
                   });

                   child.When(x => x.BirthStatus == BirthStatus.Due, () =>
                   {
                       child.RuleFor(x => x.DueDate).NotNull().WithState(x => x.ChildId);
                   });
               });
        });

        RuleSet(CheckAnswersRuleSet, () =>
        {
            RuleFor(x => x.CountryOfResidence)
                .NotNull();

            RuleFor(x => x.UserAge)
                .NotNull();

            RuleFor(x => x.Nationality)
                .NotNull();

            When(x =>
                    x.Nationality == NationalityOption.CitizenOfAnEuCountryEeaCountryOrSwitzerland,
                () =>
                {
                    RuleFor(x => x.SettledStatus)
                        .NotNull();
                });

            RuleFor(x => x.PaidWork)
                .NotNull();

            When(x =>
                    x.PaidWork == PaidWorkOption.Yes,
                () =>
                {
                    RuleFor(x => x.WorkStatus)
                        .NotEmpty();

                    When(x => x.WorkStatus.Contains(WorkStatusOption.SelfEmployed), () =>
                    {
                        RuleFor(x => x.SelfEmployedDuration)
                            .NotNull();

                        When(x => x.SelfEmployedDuration == SelfEmployedDurationOption.NotLessThan12Months, () =>
                        {
                            RuleFor(x => x.YearlyEarnings)
                                .NotNull();
                        });
                    });

                    When(x =>
                            !x.WorkStatus.Contains(WorkStatusOption.SelfEmployed) &&
                            (x.WorkStatus.Contains(WorkStatusOption.PaidEmployment) ||
                             x.WorkStatus.Contains(WorkStatusOption.Apprentice)),
                        () =>
                        {
                            RuleFor(x => x.WeeklyEarnings)
                                .NotNull();

                            When(x => x.WeeklyEarnings == WeeklyEarningsOption.AboveThreshold, () =>
                            {
                                RuleFor(x => x.YearlyEarnings)
                                    .NotNull();
                            });
                        });

                });

            When(x =>
                    x.PaidWork == PaidWorkOption.SickLeave,
                () =>
                {
                    RuleFor(x => x.WorkStatus)
                        .NotEmpty();

                    When(x => x.WorkStatus.Contains(WorkStatusOption.SelfEmployed), () =>
                    {
                        RuleFor(x => x.SelfEmployedDuration)
                            .NotNull();

                        When(x => x.SelfEmployedDuration == SelfEmployedDurationOption.NotLessThan12Months, () =>
                        {
                            RuleFor(x => x.YearlyEarnings)
                                .NotNull();
                        });
                    });

                    When(x =>
                            !x.WorkStatus.Contains(WorkStatusOption.SelfEmployed) &&
                            (x.WorkStatus.Contains(WorkStatusOption.PaidEmployment) ||
                             x.WorkStatus.Contains(WorkStatusOption.Apprentice)),
                        () =>
                        {
                            RuleFor(x => x.YearlyEarnings)
                                .NotNull();
                        });

                });


            RuleFor(x => x.UniversalCredit)
                .NotNull();

            RuleFor(x => x.Benefits)
                .NotEmpty();

            RuleFor(x => x.ChildcareSupport)
                .NotEmpty();

            When(x =>
                    x.ChildcareSupport.Contains(ChildcareSupportOption.ChildcareVouchers),
                () =>
                {
                    RuleFor(x => x.ChildcareVoucherReceipt)
                        .NotNull();
                });

            RuleFor(x => x.HasPartner)
                .NotNull();

            When(x =>
                    x.HasPartner == true,
                () =>
                {
                    RuleFor(x => x.PartnerAge)
                        .NotNull();

                    RuleFor(x => x.PartnerPaidWork)
                        .NotNull();

                    When(x =>
                            x.PartnerPaidWork == PartnerPaidWorkOption.Yes,
                        () =>
                        {
                            RuleFor(x => x.PartnerWorkStatus)
                                .NotEmpty();

                            When(x => x.PartnerWorkStatus.Contains(WorkStatusOption.SelfEmployed), () =>
                            {
                                RuleFor(x => x.PartnerSelfEmployedDuration)
                                    .NotNull();

                                When(x => x.PartnerSelfEmployedDuration == SelfEmployedDurationOption.NotLessThan12Months, () =>
                                {
                                    RuleFor(x => x.PartnerYearlyEarnings)
                                        .NotNull();
                                });
                            });

                            When(x =>
                                    !x.PartnerWorkStatus.Contains(WorkStatusOption.SelfEmployed) &&
                                    (x.PartnerWorkStatus.Contains(WorkStatusOption.PaidEmployment) ||
                                     x.PartnerWorkStatus.Contains(WorkStatusOption.Apprentice)),
                                () =>
                                {
                                    RuleFor(x => x.PartnerWeeklyEarnings)
                                        .NotNull();

                                    When(x => x.PartnerWeeklyEarnings == WeeklyEarningsOption.AboveThreshold, () =>
                                    {
                                        RuleFor(x => x.PartnerYearlyEarnings)
                                            .NotNull();
                                    });
                                });
                        });

                    When(x =>
                            x.PartnerPaidWork == PartnerPaidWorkOption.SickLeave,
                        () =>
                        {
                            RuleFor(x => x.PartnerWorkStatus)
                                .NotEmpty();

                            When(x => x.PartnerWorkStatus.Contains(WorkStatusOption.SelfEmployed), () =>
                            {
                                RuleFor(x => x.PartnerSelfEmployedDuration)
                                    .NotNull();

                                When(x => x.PartnerSelfEmployedDuration == SelfEmployedDurationOption.NotLessThan12Months, () =>
                                {
                                    RuleFor(x => x.PartnerYearlyEarnings)
                                        .NotNull();
                                });
                            });

                            When(x =>
                                    !x.PartnerWorkStatus.Contains(WorkStatusOption.SelfEmployed) &&
                                    (x.PartnerWorkStatus.Contains(WorkStatusOption.PaidEmployment) ||
                                     x.PartnerWorkStatus.Contains(WorkStatusOption.Apprentice)),
                                () =>
                                {
                                    RuleFor(x => x.PartnerYearlyEarnings)
                                        .NotNull();
                                });

                        });

                    RuleFor(x => x.PartnerBenefits)
                        .NotEmpty();

                    RuleFor(x => x.PartnerChildcareSupport)
                        .NotEmpty();

                    When(x =>
                            x.PartnerChildcareSupport.Contains(PartnerChildcareSupportOption.ChildcareVouchers),
                        () =>
                        {
                            RuleFor(x => x.PartnerChildcareVoucherReceipt)
                                .NotNull();
                        });
                });
        });
    }
}
