using AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases.Scenarios;

internal class EligibleSingleParentWith2Children : IUseCase
{
    public string Name => "Single parent with 2 children (Eligible)";

    public IEnumerable<JourneyStep> GetJourney()
    {
        return new JourneyBuilder()
            .StartInLocation("England")

            // Child 1
            .AddChild(child => child
                .WithName("Aydin")
                .IsBorn("Yes")
                .WithBirthDate("Yesterday")
                .WithRelationship("Parent")
                .WithSupport("No, none of these apply"))

            // Action: Add another child
            .Action("Add another child")

            // Child 2
            .AddChild(child => child
                .WithName("Sara")
                .IsBorn("No")
                .WithDueDate("Tomorrow")
                .WithExpectingRelationship("Parent"))

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
            .SetHasPartner("No")
            .Build();
    }
}
