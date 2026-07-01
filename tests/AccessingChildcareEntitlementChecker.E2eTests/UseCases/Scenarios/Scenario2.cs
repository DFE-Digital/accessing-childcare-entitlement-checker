using AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases.Scenarios;

internal class Scenario2 : IUseCase
{
    public string Name => "One parent on carer's allowance, Child receives DLA";

    public IEnumerable<JourneyStep> GetJourney()
    {
        return new JourneyBuilder()
            .StartInLocation("England")

            // Child 1
            .AddChild(child => child
                .WithName("Katherine")
                .IsBorn("Yes")
                .WithBirthDate("03 JAN 2017") //TODO : need a better way to handle these dates. Test will become flaky over time
                .WithRelationship("Parent")
                .WithSupport("No, none of these apply"))

            // Action: Add another child
            .Action("Add another child")

            // Child 2
            .AddChild(child => child
                .WithName("Tom")
                .IsBorn("Yes")
                .WithBirthDate("18 SEP 2023") //TODO : need a better way to handle these dates. Test will become flaky over time
                .WithRelationship("Parent")
                .WithSupport("Disability Living Allowance (DLA)"))

            // Complete child details loop
            .Action("Continue")

            .SetUserAge("21 or over")
            .SetNationality("British or Irish citizen")
            .SetPaidWork("No, I am not in work")
            .SetUniversalCredit("No")
            .SetBenefits("Carer's Allowance")
            .SetChildcareSupport("No, I do not get any of this childcare support")
            .SetHasPartner("Yes")

            .SetPartnerAge("21 or over")
            .SetPartnerPaidWork("Yes")
            .SetPartnerWorkStatus("Paid employment")
            .SetPartnerWeeklyEarnings("Yes")
            .SetPartnerYearlyEarnings("No")
            .SetPartnerBenefits("No, they do not get any of these benefits")
            .SetPartnerChildcareSupport("No, they do not get any of this childcare support")

            .Build();
    }
}
