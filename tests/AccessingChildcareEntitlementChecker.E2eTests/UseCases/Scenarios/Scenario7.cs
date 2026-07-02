using AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases.Scenarios;

internal class Scenario7 : IUseCase
{
    public string Name => "One parent on parental leave";

    public IEnumerable<JourneyStep> GetJourney()
    {
        return new JourneyBuilder()
            .StartInLocation("England")

            // Child 1
            .AddChild(child => child
                .WithName("Paula")
                .IsBorn("Yes")
                .WithBirthDate("17 JUN 2026") //TODO : need a better way to handle these dates. Test will become flaky over time
                .WithSupport("No, none of these apply"))

            // Action: Add another child
            .Action("Add another child")

            // Child 2
            .AddChild(child => child
                .WithName("Nicky")
                .IsBorn("Yes")
                .WithBirthDate("17 SEP 2018") //TODO : need a better way to handle these dates. Test will become flaky over time
                .WithSupport("No, none of these apply"))

            // Complete child details loop
            .Action("Continue")

            .SetUserAge("21 or over")
            .SetNationality("British or Irish citizen")
            .SetPaidWork("Yes")
            .SetWorkStatus("Paid employment")
            .SetWeeklyEarnings("Yes")
            .SetYearlyEarnings("No")
            .SetUniversalCredit("No")
            .SetBenefits("No, I do not get any of these benefits")
            .SetChildcareSupport("No, I do not get any of this childcare support")
            .SetHasPartner("Yes")

            .SetPartnerAge("21 or over")
            .SetPartnerPaidWork("Yes, but they are on parental leave")
            .SetPartnerChildLeave("Paula")
            .SetPartnerWorkStatus("Paid employment")
            .SetPartnerLeaveWeeklyEarnings("Yes")
            .SetPartnerYearlyEarnings("No")
            .SetPartnerBenefits("No, they do not get any of these benefits")
            .SetPartnerChildcareSupport("No, they do not get any of this childcare support")

            .Build();
    }
}
