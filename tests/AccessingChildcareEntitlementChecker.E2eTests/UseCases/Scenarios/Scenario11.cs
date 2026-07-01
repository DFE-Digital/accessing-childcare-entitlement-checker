using AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases.Scenarios;

internal class Scenario11 : IUseCase
{
    public string Name => "Single parent not working, on carer's allowance";

    public IEnumerable<JourneyStep> GetJourney()
    {
        return new JourneyBuilder()
            .StartInLocation("England")

            // Child 1
            .AddChild(child => child
                .WithName("Kurt")
                .IsBorn("Yes")
                .WithBirthDate("03 JAN 2025") //TODO : need a better way to handle these dates. Test will become flaky over time
                .WithSupport("No, none of these apply"))

            // Complete child details loop
            .Action("Continue")

            .SetUserAge("21 or over")
            .SetNationality("British or Irish citizen")
            .SetPaidWork("No, I am not in work")
            .SetUniversalCredit("Yes")
            .SetBenefits("Carer's Allowance")
            .SetChildcareSupport("No, I do not get any of this childcare support")
            .SetHasPartner("No")

            .Build();
    }
}
