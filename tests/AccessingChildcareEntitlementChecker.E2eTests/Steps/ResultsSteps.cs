using AccessingChildcareEntitlementChecker.E2eTests.Helpers;
using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.E2eTests.Steps;

[Binding]
public class ResultsSteps(IPage page)
{
    [Then(@"I {word} see the public funds warning")]
    public async Task ThenICanOrCannotSeeThePublicFundsWarning(string canOrCannot)
    {
        var shouldSeeWarning = canOrCannot == "can";
        var expectation = Expect(page.GetByText("You need to check if you can access public funds"));

        if (shouldSeeWarning)
        {
            await expectation.ToBeVisibleAsync();
        }
        else
        {
            await expectation.Not.ToBeVisibleAsync();
        }
    }

    [When("I click the details link for {string}")]
    public async Task WhenIClickTheDetailsLinkForString(string childName)
    {
        var text = $"View detailed information about {childName}'s childcare support";
        var link = page.GetByRole(AriaRole.Link, new() { Name = text, Exact = true });
        await link.ClickAsync();
        await page.WaitForURLAsync("**/Results/ResultsDetailed**");
    }

    [Then("I can see that {string} is now eligible for {string}")]
    public async Task ThenICanSeeThatIsNowEligibleFor(string name, string scheme)
    {
        var eligibleSchemes = await GetEligibleSchemes(name);
        var result = Assert.Single(eligibleSchemes);
        Assert.Equal(scheme, result.Item1);
        Assert.Equal(WhenEligible.Now.ToString(), result.Item2);
    }

    [Then("I can see that {string} is eligible for:")]
    public async Task ThenICanSeeThatIsEligibleFor(string name, DataTable dataTable)
    {
        var expecteds = dataTable.CreateSet<SchemeEligibilityResult>().ToList();
        var actuals = await GetEligibleSchemes(name);
        Assert.Equal(expecteds.Count, actuals.Count);
        foreach (var (actual, expected) in actuals.Zip(expecteds))
        {
            Assert.Equal(expected.Scheme, actual.Item1);

            switch (expected.When)
            {
                case WhenEligible.Now:
                    Assert.Equal("Now", actual.Item2);
                    break;
                case WhenEligible.Birth:
                    Assert.Equal("When they are born", actual.Item2);
                    break;
                case WhenEligible.WhenPartnerReturnsFromParentalLeave:
                    Assert.StartsWith("The date your partner returns from parental leave", actual.Item2);
                    break;
                case WhenEligible.NineMonthsOld:
                    if (expected.IsBorn == true)
                    {
                        Assert.StartsWith("From", actual.Item2);
                    }
                    else
                    {
                        Assert.Equal("When they are 23 weeks old", actual.Item2);
                    }
                    break;
                case WhenEligible.TwoYearsOld:
                    Assert.StartsWith("From", actual.Item2);
                    break;
                case WhenEligible.ThreeYearsOld:
                    Assert.Equal("Ask your childcare provider or local council", actual.Item2);
                    break;
                case WhenEligible.InTheFuture:
                    Assert.Equal("Ask your childcare provider or local council", actual.Item2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected value: {expected.When}");
            }
        }
    }

    [Then("I can see that {string} is not eligible for any childcare entitlement schemes")]
    public async Task ThenICanSeeThatIsNotEligibleForAnyChildcareEntitlementSchemes(string name)
    {
        var resultsSection = page.Locator(".app-results-section")
           .Filter(new()
           {
               Has = page.GetByRole(AriaRole.Heading, new() { Name = name })
           });

        var textContents = await resultsSection.AllTextContentsAsync();
        var text = string.Join(" ", textContents);
        Assert.Contains($"You cannot currently get any of the childcare support this service checks for {name}.", text);
    }

    private async Task<IReadOnlyList<(string, string)>> GetEligibleSchemes(string name)
    {
        var resultsSection = page.Locator(".app-results-section")
           .Filter(new()
           {
               Has = page.GetByRole(AriaRole.Heading, new() { Name = name })
           });

        var rows = resultsSection.Locator("tbody tr");
        var count = await rows.CountAsync();
        var results = new List<(string, string)>(count);
        for (var i = 0; i < count; i++)
        {
            var cells = rows.Nth(i).Locator("td");
            var scheme = await cells
                .Nth(0)
                .Locator("a")
                .EvaluateAsync<string>(@"a => a.childNodes[0].textContent");
            var whenToApply = await cells.Nth(2).InnerTextAsync();
            results.Add((scheme.Trim(), whenToApply.Trim()));
        }

        return results;
    }

    private record SchemeEligibilityResult(string Scheme, WhenEligible When, bool? IsBorn = null);
}
