using Dfe.Acec.Web.Models.Summary;

namespace Dfe.Acec.Web.Services.Summary;


public interface IChildSummaryBuilder
{
    ChildSummaryViewModel BuildChildSummary(Child child, string returnTo);
}
