using Reqnroll;

namespace AccessingChildcareEntitlementChecker.Tests.E2e.Steps.DeferredBackground
{
    public static class DataTableExtensions
    {
        public static IEnumerable<(string Question, string Answer)> ToQuestionAnswerPairs(this DataTable dataTable)
        {
            return dataTable
                .Rows
                .Select(row => (Question: row["Question"].ToString(), Answer: row["Answer"].ToString()));
        }
    }
}
