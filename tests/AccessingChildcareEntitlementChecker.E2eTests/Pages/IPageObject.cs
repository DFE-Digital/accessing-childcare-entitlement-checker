namespace AccessingChildcareEntitlementChecker.E2eTests.Pages;

internal interface IPageObject
{
    string PageTitle { get; }
    Task AnswerAsync(string answer);
    Task ContinueAsync();
}
