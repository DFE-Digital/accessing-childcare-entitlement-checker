namespace Dfe.Acec.Web.Tests.E2e.Pages;

internal interface IPageObject
{
    public string PageTitle { get; }
    public Task AnswerAsync(string answer);
    public Task ContinueAsync();
    public Task AssertHeaderAsync();
}
