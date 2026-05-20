using Microsoft.Playwright;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.Steps.DeferredBackground
{
    public class DeferredLinkClick(string title, string label) : IDeferredAction
    {
        private readonly string _label = label;

        public string Title { get; } = title;

        public async Task Execute(IPage page)
        {
            var heading = page.GetByRole(AriaRole.Heading, new() { Level = 1 });
            await Expect(heading).ToHaveTextAsync(Title);
            await page
                .GetByRole(AriaRole.Link, new() { Name = _label })
                .ClickAsync();
            await Expect(heading).Not.ToHaveTextAsync(Title);
        }
    }
}
