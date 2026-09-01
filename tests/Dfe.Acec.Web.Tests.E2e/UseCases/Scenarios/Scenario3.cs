using Dfe.Acec.Web.Tests.E2e.UseCases.Builders;
using JetBrains.Annotations;

namespace Dfe.Acec.Web.Tests.E2e.UseCases.Scenarios;

[UsedImplicitly]
internal sealed class Scenario3 : IUseCase
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
                .WithBirthDate(addYears: -2, addMonths: -3, addDays: -14)
                .WithSupport("No, none of these apply"))

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
