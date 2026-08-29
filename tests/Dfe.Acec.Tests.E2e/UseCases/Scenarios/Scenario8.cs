using Dfe.Acec.Tests.E2e.UseCases.Builders;

namespace Dfe.Acec.Tests.E2e.UseCases.Scenarios;

internal sealed class Scenario8 : IUseCase
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
                .WithBirthDate(addYears: -1, addMonths: -5, addDays: -14)
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
