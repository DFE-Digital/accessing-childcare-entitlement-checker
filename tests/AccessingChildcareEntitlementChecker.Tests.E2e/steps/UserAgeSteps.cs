using Reqnroll;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.steps
{
    [Binding]
    [Scope(Feature = "User Age")]
    public class UserAgeSteps
    {
        private Context _context;

        public UserAgeSteps(Context context)
        {
            _context = context;
        }

        [Given("I am on the 'How old are you?' page")]
        public async Task GivenIAmOnTheHowOldAreYouPage()
        {
            var url = new Uri(_context.Uri, "/User/UserAge");
            await _context.Page.GotoAsync(url.AbsoluteUri);
        }
    }
}
