using Reqnroll;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.steps
{
    [Binding]
    [Scope(Feature = "Location")]
    public class LocationSteps
    {
        private readonly Context _context;

        public LocationSteps(Context context)
        {
            _context = context;
        }

        [Given("I am on the 'Where do you live?' page")]
        public async Task GivenIAmOnTheLocationPage()
        {
            var url = new Uri(_context.Uri, "/Home/Location");
            await _context.Page.GotoAsync(url.AbsoluteUri);
        }
    }
}