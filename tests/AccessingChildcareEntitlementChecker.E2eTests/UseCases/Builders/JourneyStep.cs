namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

internal abstract record JourneyStep;
internal record AnswerStep(string PageName, string Answer) : JourneyStep;
internal record ActionStep(string ActionName) : JourneyStep;
