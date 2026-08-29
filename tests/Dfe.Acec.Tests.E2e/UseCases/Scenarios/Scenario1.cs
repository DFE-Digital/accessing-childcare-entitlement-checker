using Dfe.Acec.Tests.E2e.UseCases.Builders;

namespace Dfe.Acec.Tests.E2e.UseCases.Scenarios;

internal sealed class Scenario1 : IUseCase
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
                .WithBirthDate(addYears: -13, addMonths: -7, addDays: -14)
                .WithSupport("No, none of these apply"))

            // Action: Add another child
            .Action("Add another child")

            // Child 2
            .AddChild(child => child
                .WithName("Frankie")
                .IsBorn("Yes")
                .WithBirthDate(addYears: -2, addMonths: -11, addDays: 1)
                .WithSupport("No, none of these apply"))

            // Action: Add another child
            .Action("Add another child")

            // Child 3
            .AddChild(child => child
                .WithName("Baby")
                .IsBorn("No")
                .WithDueDate(addMonths: 2))

            // Complete child details loop
            .Action("Continue")

            .SetUserAge("21 or over")
            .SetNationality("British or Irish citizen")
            .SetPaidWork("Yes, I am currently in work")
            .SetWorkStatus("Paid employment")
            .SetWeeklyEarnings("No")
            .SetUniversalCredit("Yes")
            .SetBenefits("No, I do not get any of these benefits")
            .SetChildcareSupport("No, I do not get any of these")
            .SetHasPartner("No")
            .Build();
    }
}
