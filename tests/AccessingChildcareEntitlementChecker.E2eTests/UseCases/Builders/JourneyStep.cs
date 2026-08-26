namespace AccessingChildcareEntitlementChecker.E2eTests.UseCases.Builders;

internal abstract record JourneyStep;
internal sealed record AnswerStep(string PageName, string Answer) : JourneyStep;
internal sealed record ActionStep(string ActionName) : JourneyStep;
