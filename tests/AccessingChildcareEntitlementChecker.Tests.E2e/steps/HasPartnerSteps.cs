using Reqnroll;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.steps
{
    [Binding]
    [Scope(Feature = "Has Partner")]
    public class HasPartnerSteps
    {
        private readonly Context _context;

        public HasPartnerSteps(Context context)
        {
            _context = context;
        }

        [Given("I am on the 'Do you live with a partner?' page")]
        public async Task GivenIAmOnTheLocationPage()
        {
            var url = new Uri(_context.Uri, "/User/HasPartner");
            await _context.Page.GotoAsync(url.AbsoluteUri);
        }
    }
}