using Reqnroll;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.steps;

[Binding]
[Scope(Feature = "Child Date Of Birth")]
public class ChildDateOfBirthSteps
{
    private readonly Context _context;

    public ChildDateOfBirthSteps(Context context)
    {
        _context = context;
    }

    [Given("I am on the 'What is Jack''s date of birth?' page")]
    public async Task GivenIAmOnTheChildDateOfBirthPage()
    {
        var url = new Uri(_context.Uri, "/User/ChildDateOfBirth");
        await _context.Page.GotoAsync(url.AbsoluteUri);
    }
}
