using Reqnroll;

namespace Dfe.Acec.Web.Tests.E2e.Extensions;

internal static class DataTableExtensions
{
    public static IEnumerable<(string PageName, string Answer)> ToPageAnswerPairs(this DataTable dataTable) => dataTable.Rows.Select(row =>
                                                                                                                    {
                                                                                                                        if (!row.TryGetValue("Page", out var pageName) && !row.TryGetValue("Question", out pageName))
                                                                                                                        {
                                                                                                                            throw new InvalidOperationException("DataTable row must contain either 'Page' or 'Question' column.");
                                                                                                                        }

                                                                                                                        if (!row.TryGetValue("Answer", out var answer))
                                                                                                                        {
                                                                                                                            throw new InvalidOperationException("DataTable row must contain 'Answer' column.");
                                                                                                                        }

                                                                                                                        return (PageName: pageName, Answer: answer);
                                                                                                                    });
}
