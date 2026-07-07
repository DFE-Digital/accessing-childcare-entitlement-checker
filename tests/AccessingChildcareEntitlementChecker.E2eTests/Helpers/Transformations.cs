using Reqnroll;

namespace AccessingChildcareEntitlementChecker.E2eTests.Helpers;

[Binding]
public class Transformations
{
    [StepArgumentTransformation("(now|in the future)")]
    public WhenEligible TransformWhen(string value)
    {
        return ParseWhenEligible(value);
    }

    public static WhenEligible ParseWhenEligible(string value)
    {
        return value switch
        {
            "now" => WhenEligible.Now,
            "in the future" => WhenEligible.InTheFuture,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
}
