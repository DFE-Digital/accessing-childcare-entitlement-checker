using AccessingChildcareEntitlementChecker.E2eTests.Helpers;
using Microsoft.Playwright;
using Reqnroll;
using System.Xml.Linq;
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
        var text = $"See the full details for {childName},";
        await page.GetByText(text).ClickAsync();
    }

    [Then("I can see that {string} is now eligible for {string}")]
    public async Task ThenICanSeeThatIsNowEligibleFor(string name, string scheme)
    {
        var eligibleSchemes = await GetEligibleSchemes(name);
        var result = Assert.Single(eligibleSchemes);
        Assert.Equal(scheme, result.Scheme);
        Assert.Equal(WhenEligible.Now, result.When);
    }

    [Then("I can see that {string} is eligible for:")]
    public async Task ThenICanSeeThatIsEligibleFor(string name, DataTable dataTable)
    {
        var expected = dataTable.CreateSet<SchemeEligibilityResult>();
        var actual = await GetEligibleSchemes(name);
        Assert.Equal(expected, actual);
    }

    private async Task<IReadOnlyList<SchemeEligibilityResult>> GetEligibleSchemes(string name)
    {
        var resultsSection = page.Locator(".app-results-section")
           .Filter(new()
           {
               Has = page.GetByRole(AriaRole.Heading, new() { Name = name })
           });

        var rows = resultsSection.Locator("tbody tr");
        var count = await rows.CountAsync();
        var results = new List<SchemeEligibilityResult>(count);
        for (var i = 0; i < count; i++)
        {
            var cells = rows.Nth(i).Locator("td");
            var scheme = await cells
                .Nth(0)
                .Locator("a")
                .EvaluateAsync<string>(@"a => a.childNodes[0].textContent");
            var whenToApply = await cells.Nth(2).InnerTextAsync();
            var whenEligible = whenToApply.Trim() == "Now"
                ? WhenEligible.Now
                : WhenEligible.InTheFuture;
            results.Add(new (scheme.Trim(), whenEligible));
        }

        return results;
    }

    private record SchemeEligibilityResult(string Scheme, WhenEligible When);

}
