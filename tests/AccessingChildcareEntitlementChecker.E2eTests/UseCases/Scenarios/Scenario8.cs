using AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases.Scenarios;

internal class Scenario8 : IUseCase
{
    public string Name => "Single parent on sick leave, parent is a citizen of a different country";

    public IEnumerable<JourneyStep> GetJourney()
    {
        return new JourneyBuilder()
            .StartInLocation("England")

            // Child 1
            .AddChild(child => child
                .WithName("Lee")
                .IsBorn("Yes")
                .WithBirthDate("03 MAR 2025") //TODO : need a better way to handle these dates. Test will become flaky over time
                .WithSupport("No, none of these apply"))

            // Complete child details loop
            .Action("Continue")

            .SetUserAge("21 or over")
            .SetNationality("Citizen of a different country")
            .SetPaidWork("Yes, but I am on sick leave")
            .SetWorkStatus("Paid employment")
            .SetYearlyEarnings("No")
            .SetUniversalCredit("Yes")
            .SetBenefits("No, I do not get any of these benefits")
            .SetChildcareSupport("No, I do not get any of these")
            .SetHasPartner("No")

            .Build();
    }
}
