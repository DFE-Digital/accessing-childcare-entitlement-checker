using Microsoft.Playwright;
using Reqnroll;
using static Microsoft.Playwright.Assertions;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.steps
{
    [Binding]
    [Scope(Feature = "Partner Age")]
    public class PartnerAgeSteps
    {
        private readonly Context _context;

        public PartnerAgeSteps(Context context)
        {
            _context = context;
        }

        [Given("I am on the 'How old is your partner?' page")]
        public async Task GivenIAmOnThePartnerAgePage()
        {
            var url = new Uri(_context.Uri, "/Partner/PartnerAge");
            await _context.Page.GotoAsync(url.AbsoluteUri);
        }
    }
}