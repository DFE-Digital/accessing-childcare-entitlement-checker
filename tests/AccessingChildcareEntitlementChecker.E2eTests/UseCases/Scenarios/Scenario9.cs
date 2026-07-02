using AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases.Scenarios;

internal class Scenario9 : IUseCase
{
    public string Name => "One parent not working, one parent receiving ESA";

    public IEnumerable<JourneyStep> GetJourney()
    {
        return new JourneyBuilder()
            .StartInLocation("England")

            // Child 1
            .AddChild(child => child
                .WithName("Isabel")
                .IsBorn("Yes")
                .WithBirthDate("01 MAR 2024") //TODO : need a better way to handle these dates. Test will become flaky over time
                .WithSupport("No, none of these apply"))

            // Action: Add another child
            .Action("Add another child")

            // Child 2
            .AddChild(child => child
                .WithName("Mary")
                .IsBorn("Yes")
                .WithBirthDate("01 APR 2026") //TODO : need a better way to handle these dates. Test will become flaky over time
                .WithSupport("No, none of these apply"))

            // Complete child details loop
            .Action("Continue")

            .SetUserAge("21 or over")
            .SetNationality("British or Irish citizen")
            .SetPaidWork("No, I am not in work")
            .SetUniversalCredit("Yes")
            .SetBenefits("Contribution-based Employment and Support Allowance")
            .SetChildcareSupport("No, I do not get any of this childcare support")
            .SetHasPartner("Yes")

            .SetPartnerAge("21 or over")
            .SetPartnerPaidWork("Yes")
            .SetPartnerWorkStatus("Paid employment")
            .SetPartnerWeeklyEarnings("Yes")
            .SetPartnerYearlyEarnings("No")
            .SetPartnerBenefits("No, they do not get any of these benefits")
            .SetPartnerChildcareSupport("No, they do not get any of this childcare support")

            .Build();
    }
}
