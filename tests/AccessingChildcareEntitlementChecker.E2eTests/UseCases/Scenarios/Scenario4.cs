using AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases.Scenarios;

internal class Scenario4 : IUseCase
{
    public string Name => "One parent aged 18-20, child not yet born";

    public IEnumerable<JourneyStep> GetJourney()
    {
        return new JourneyBuilder()
            .StartInLocation("England")

            // Child 1
            .AddChild(child => child
                .WithName("Daphne")
                .IsBorn("Yes")
                .WithBirthDate("03 JAN 2019") //TODO : need a better way to handle these dates. Test will become flaky over time
                .WithRelationship("Parent")
                .WithSupport("No, none of these apply"))

            // Action: Add another child
            .Action("Add another child")

            // Child 2
            .AddChild(child => child
                .WithName("Baby")
                .IsBorn("No")
                .WithDueDate("17 OCT 2026") //TODO : need a better way to handle these dates. Test will become flaky over time
                .WithExpectingRelationship("Parent"))

            // Complete child details loop
            .Action("Continue")

            .SetUserAge("18 to 20")
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
            .SetPartnerPaidWork("Yes")
            .SetPartnerWorkStatus("Paid employment")
            .SetPartnerWeeklyEarnings("Yes")
            .SetPartnerYearlyEarnings("No")
            .SetPartnerBenefits("No, they do not get any of these benefits")
            .SetPartnerChildcareSupport("No, they do not get any of this childcare support")

            .Build();
    }
}
