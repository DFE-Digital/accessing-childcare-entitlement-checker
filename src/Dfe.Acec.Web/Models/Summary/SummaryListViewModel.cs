namespace Dfe.Acec.Web.Models.Summary;

public record SummaryListViewModel(
    IEnumerable<SummaryRowViewModel> Rows,
    bool IsUserDetails = false);
