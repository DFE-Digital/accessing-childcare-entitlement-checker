using Microsoft.Playwright;
using Reqnroll;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.steps
{
    [Binding]
    public class ErrorSteps
    {
        private Context _context;
        private IResponse? _lastResponse;

        public ErrorSteps(Context context)
        {
            _context = context;
        }

        [Given("I visit a non-existent page")]
        public async Task GivenIVisitANon_ExistentPage()
        {
            var url = new Uri(_context.Uri, "/ThisPageDoesNotExist");
            _lastResponse = await _context.Page.GotoAsync(url.AbsoluteUri);
        }

        [Given("I visit the development-only error test page")]
        public async Task GivenIVisitTheDevelopment_OnlyErrorTestPage()
        {
            var url = new Uri(_context.Uri, "/error");
            _lastResponse = await _context.Page.GotoAsync(url.AbsoluteUri);
        }

        [Then("I am not redirected to another page")]
        public void ThenIAmNotRedirectedToAnotherPage()
        {
            // Note: this step may fail when running the tests against HTTP; because
            // you may be redirected to HTTPs. We could consider improving this check,
            // maybe by just validating the url.
            Assert.NotNull(_lastResponse);
            Assert.Null(_lastResponse.Request.RedirectedFrom);
        }

        [Then("the HTTP status code is 404 Not Found")]
        public void ThenTheHTTPStatusCodeIs404NotFound()
        {
            Assert.Equal(404, _lastResponse?.Status);
        }

        [Then("the HTTP status code is 500 Internal Server Error")]
        public void ThenTheHTTPStatusCodeIs500InternalServerError()
        {
            Assert.Equal(500, _lastResponse?.Status);
        }
    }
}
