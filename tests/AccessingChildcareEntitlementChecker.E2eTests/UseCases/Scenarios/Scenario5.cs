using AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases.Scenarios;

internal class Scenario5 : IUseCase
{
    public string Name => "Single parent who is self employed, child is not born yet";

    public IEnumerable<JourneyStep> GetJourney()
    {
        return new JourneyBuilder()
            .StartInLocation("England")

            // Child 1
            .AddChild(child => child
                .WithName("Baby")
                .IsBorn("No")
                .WithDueDate("17 AUG 2026") //TODO : need a better way to handle these dates. Test will become flaky over time
                .WithExpectingRelationship("Parent"))

            // Complete child details loop
            .Action("Continue")

            .SetUserAge("21 or over")
            .SetNationality("British or Irish citizen")
            .SetPaidWork("Yes")
            .SetWorkStatus("Self-employed")
            .SetSelfEmployedDuration("Yes")
            .SetUniversalCredit("No")
            .SetBenefits("No, I do not get any of these benefits")
            .SetChildcareSupport("No, I do not get any of this childcare support")
            .SetHasPartner("No")

            .Build();
    }
}
