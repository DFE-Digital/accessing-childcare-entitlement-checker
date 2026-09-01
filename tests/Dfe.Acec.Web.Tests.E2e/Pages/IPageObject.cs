using JetBrains.Annotations;

namespace Dfe.Acec.Web.Tests.E2e.Pages;

internal interface IPageObject
{
    [UsedImplicitly]
    string PageTitle { get; }
    Task AnswerAsync(string answer);
    Task ContinueAsync();
    Task AssertHeaderAsync();
}
