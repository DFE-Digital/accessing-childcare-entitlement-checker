using Dfe.Acec.Web.Tests.E2e.UseCases.Builders;

namespace Dfe.Acec.Web.Tests.E2e.UseCases.Scenarios;

internal sealed class Scenario4 : IUseCase
{
    public string Name => "One parent aged 18-20, child not yet born";

    public IEnumerable<JourneyStep> GetJourney()
    {
        return new JourneyBuilder()
            .StartInLocation("England")

            // Child 1
            .AddChild(child => child
                .WithName("Daphne")
                .IsBorn("Yes")
                .WithBirthDate(addYears: -7, addMonths: -7, addDays: -14)
                .WithSupport("No, none of these apply"))

            // Action: Add another child
            .Action("Add another child")

            // Child 2
            .AddChild(child => child
                .WithName("Baby")
                .IsBorn("No")
                .WithDueDate(addMonths: 2))

            // Complete child details loop
            .Action("Continue")

            .SetUserAge("18 to 20")
            .SetNationality("British or Irish citizen")
            .SetPaidWork("Yes, I am currently in work")
            .SetWorkStatus("Paid employment")
            .SetWeeklyEarnings("Yes")
            .SetYearlyEarnings("No")
            .SetUniversalCredit("No")
            .SetBenefits("No, I do not get any of these benefits")
            .SetChildcareSupport("No, I do not get any of these")
            .SetHasPartner("Yes")

            .SetPartnerAge("21 or over")
            .SetPartnerPaidWork("Yes, they are currently in work")
            .SetPartnerWorkStatus("Paid employment")
            .SetPartnerWeeklyEarnings("Yes")
            .SetPartnerYearlyEarnings("No")
            .SetPartnerBenefits("No, they do not get any of these benefits")
            .SetPartnerChildcareSupport("No, they do not get any of these")

            .Build();
    }
}
