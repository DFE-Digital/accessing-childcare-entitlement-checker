using Dfe.Acec.Web.Tests.E2e.UseCases.Builders;

namespace Dfe.Acec.Web.Tests.E2e.UseCases.Scenarios;

internal sealed class Scenario6 : IUseCase
{
    public string Name => "Both parents under 18, one parent an apprentice, one parent earning under the threshold";

    public IEnumerable<JourneyStep> GetJourney() => new JourneyBuilder()
            .StartInLocation("England")

            // Child 1
            .AddChild(child => child
                .WithName("Winston")
                .IsBorn("Yes")
                .WithBirthDate(addYears: -1, addMonths: -7, addDays: -14)
                .WithSupport("No, none of these apply"))

            // Complete child details loop
            .Action("Continue")

            .SetUserAge("Under 18")
            .SetNationality("British or Irish citizen")
            .SetPaidWork("Yes, I am currently in work")
            .SetWorkStatus("Apprentice")
            .SetWeeklyEarnings("Yes")
            .SetYearlyEarnings("No")
            .SetUniversalCredit("Yes")
            .SetBenefits("No, I do not get any of these benefits")
            .SetChildcareSupport("No, I do not get any of these")
            .SetHasPartner("Yes")

            .SetPartnerAge("Under 18")
            .SetPartnerPaidWork("Yes, they are currently in work")
            .SetPartnerWorkStatus("Paid employment")
            .SetPartnerWeeklyEarnings("No")
            .SetPartnerBenefits("No, they do not get any of these benefits")
            .SetPartnerChildcareSupport("No, they do not get any of these")

            .Build();
}
