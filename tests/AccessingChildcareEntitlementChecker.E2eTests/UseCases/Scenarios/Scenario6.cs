using AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases.Scenarios;

internal class Scenario6 : IUseCase
{
    public string Name => "Both parents under 18, one parent an apprentice, one parent earning under the threshold";

    public IEnumerable<JourneyStep> GetJourney()
    {
        return new JourneyBuilder()
            .StartInLocation("England")

            // Child 1
            .AddChild(child => child
                .WithName("Winston")
                .IsBorn("Yes")
                .WithBirthDate("03 JAN 2025") //TODO : need a better way to handle these dates. Test will become flaky over time
                .WithSupport("No, none of these apply"))

            // Complete child details loop
            .Action("Continue")

            .SetUserAge("Under 18")
            .SetNationality("British or Irish citizen")
            .SetPaidWork("Yes")
            .SetWorkStatus("Apprentice")
            .SetWeeklyEarnings("Yes")
            .SetYearlyEarnings("No")
            .SetUniversalCredit("Yes")
            .SetBenefits("No, I do not get any of these benefits")
            .SetChildcareSupport("No, I do not get any of these")
            .SetHasPartner("Yes")

            .SetPartnerAge("Under 18")
            .SetPartnerPaidWork("Yes")
            .SetPartnerWorkStatus("Paid employment")
            .SetPartnerWeeklyEarnings("No")
            .SetPartnerBenefits("No, they do not get any of these benefits")
            .SetPartnerChildcareSupport("No, they do not get any of these")

            .Build();
    }
}
