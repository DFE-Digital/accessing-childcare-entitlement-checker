namespace Dfe.Acec.Tests.E2e.Pages;

internal interface IPageObject
{
    string PageTitle { get; }
    Task AnswerAsync(string answer);
    Task ContinueAsync();
    Task AssertHeaderAsync();
}
