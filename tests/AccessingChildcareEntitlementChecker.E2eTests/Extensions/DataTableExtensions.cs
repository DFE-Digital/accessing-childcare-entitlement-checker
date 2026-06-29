using Reqnroll;

namespace AccessingChildcareEntitlementChecker.E2eTests.Extensions;

internal static class DataTableExtensions
{
    public static IEnumerable<(string PageName, string Answer)> ToPageAnswerPairs(this DataTable dataTable)
    {
        return dataTable.Rows.Select(row =>
        {
            var pageName = row.TryGetValue("Page", out var value) ? value : row["Question"];
            var answer = row["Answer"];
            return (PageName: pageName, Answer: answer);
        });
    }
}
