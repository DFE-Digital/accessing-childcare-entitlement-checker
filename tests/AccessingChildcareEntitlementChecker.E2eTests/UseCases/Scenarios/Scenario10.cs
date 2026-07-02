using AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases.Scenarios;

internal class Scenario10 : IUseCase
{
    public string Name => "Parent is a non-UK national without pre-settled or settled status";

    public IEnumerable<JourneyStep> GetJourney()
    {
        return new JourneyBuilder()
            .StartInLocation("England")

            // Child 1
            .AddChild(child => child
                .WithName("Louise")
                .IsBorn("Yes")
                .WithBirthDate("03 MAR 2026") //TODO : need a better way to handle these dates. Test will become flaky over time
                .WithSupport("No, none of these apply"))

            // Action: Add another child
            .Action("Add another child")

            // Child 2
            .AddChild(child => child
                .WithName("Jeremy")
                .IsBorn("Yes")
                .WithBirthDate("03 MAR 2013") //TODO : need a better way to handle these dates. Test will become flaky over time
                .WithSupport("No, none of these apply"))

            // Complete child details loop
            .Action("Continue")

            .SetUserAge("21 or over")
            .SetNationality("Citizen of an EU country, EEA country or Switzerland")
            .SetSettledStatus("No")
            .SetPaidWork("Yes")
            .SetWorkStatus("Paid employment")
            .SetWeeklyEarnings("Yes")
            .SetYearlyEarnings("No")
            .SetUniversalCredit("No")
            .SetBenefits("No, I do not get any of these benefits")
            .SetChildcareSupport("No, I do not get any of these")
            .SetHasPartner("No")

            .Build();
    }
}
