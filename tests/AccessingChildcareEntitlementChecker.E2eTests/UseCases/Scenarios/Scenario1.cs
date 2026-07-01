using AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases.Scenarios;

internal class Scenario1 : IUseCase
{
    public string Name => "Single parent earning below the threshold, household receives Universal Credit, child is not born yet";

    public IEnumerable<JourneyStep> GetJourney()
    {
        return new JourneyBuilder()
            .StartInLocation("England")

            // Child 1
            .AddChild(child => child
                .WithName("Simon")
                .IsBorn("Yes")
                .WithBirthDate("03 JAN 2013") //TODO : need a better way to handle these dates. Test will become flaky over time
                .WithRelationship("Parent")
                .WithSupport("No, none of these apply"))

            // Action: Add another child
            .Action("Add another child")

            // Child 2
            .AddChild(child => child
                .WithName("Frankie")
                .IsBorn("Yes")
                .WithBirthDate("18 SEP 2023") //TODO : need a better way to handle these dates. Test will become flaky over time
                .WithRelationship("Parent")
                .WithSupport("No, none of these apply"))

            // Action: Add another child
            .Action("Add another child")

            // Child 3
            .AddChild(child => child
                .WithName("Baby")
                .IsBorn("No")
                .WithDueDate("17 OCT 2026") //TODO : need a better way to handle these dates. Test will become flaky over time
                .WithExpectingRelationship("Parent"))

            // Complete child details loop
            .Action("Continue")

            .SetUserAge("21 or over")
            .SetNationality("British or Irish citizen")
            .SetPaidWork("Yes")
            .SetWorkStatus("Paid employment")
            .SetWeeklyEarnings("No")
            .SetUniversalCredit("Yes")
            .SetBenefits("No, I do not get any of these benefits")
            .SetChildcareSupport("No, I do not get any of this childcare support")
            .SetHasPartner("No")
            .Build();
    }
}
