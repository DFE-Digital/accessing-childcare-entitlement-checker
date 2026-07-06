using Reqnroll;

namespace AccessingChildcareEntitlementChecker.E2eTests.Extensions;

public static class ScenarioContextExtensions
{
    public static bool IsWithJavascript(this ScenarioContext scenarioContext)
    {
        return scenarioContext.ScenarioInfo.Tags.Contains("withJavascript");
    }
}
