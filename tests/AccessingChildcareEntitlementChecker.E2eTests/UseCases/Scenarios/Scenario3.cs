using AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases.Scenarios;

internal class Scenario3 : IUseCase
{
    public string Name => "One parent is earning under the threshold, household receives Universal Credit";

    public IEnumerable<JourneyStep> GetJourney()
    {
        return new JourneyBuilder()
            .StartInLocation("England")

            // Child 1
            .AddChild(child => child
                .WithName("Rosa")
                .IsBorn("Yes")
                .WithBirthDate("03 MAY 2024") //TODO : need a better way to handle these dates. Test will become flaky over time
                .WithSupport("No, none of these apply"))

            // Complete child details loop
            .Action("Continue")

            .SetUserAge("21 or over")
            .SetNationality("British or Irish citizen")
            .SetPaidWork("Yes")
            .SetWorkStatus("Paid employment")
            .SetWeeklyEarnings("No")
            .SetUniversalCredit("Yes")
            .SetBenefits("No, I do not get any of these benefits")
            .SetChildcareSupport("No, I do not get any of these")
            .SetHasPartner("Yes")

            .SetPartnerAge("21 or over")
            .SetPartnerPaidWork("Yes")
            .SetPartnerWorkStatus("Paid employment")
            .SetPartnerWeeklyEarnings("Yes")
            .SetPartnerYearlyEarnings("No")
            .SetPartnerBenefits("No, they do not get any of these benefits")
            .SetPartnerChildcareSupport("No, they do not get any of these")

            .Build();
    }
}
