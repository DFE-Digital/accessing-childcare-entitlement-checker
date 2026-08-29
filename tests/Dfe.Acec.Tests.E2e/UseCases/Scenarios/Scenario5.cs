using Dfe.Acec.Tests.E2e.UseCases.Builders;

namespace Dfe.Acec.Tests.E2e.UseCases.Scenarios;

internal sealed class Scenario5 : IUseCase
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
                .WithDueDate(addMonths: 6))

            // Complete child details loop
            .Action("Continue")

            .SetUserAge("21 or over")
            .SetNationality("British or Irish citizen")
            .SetPaidWork("Yes, I am currently in work")
            .SetWorkStatus("Self-employed")
            .SetSelfEmployedDuration("Yes")
            .SetUniversalCredit("No")
            .SetBenefits("No, I do not get any of these benefits")
            .SetChildcareSupport("No, I do not get any of these")
            .SetHasPartner("No")

            .Build();
    }
}
