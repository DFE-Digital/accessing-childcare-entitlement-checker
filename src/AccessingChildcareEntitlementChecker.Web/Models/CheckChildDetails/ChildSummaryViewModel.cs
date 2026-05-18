using AccessingChildcareEntitlementChecker.Web.Services;
using System.ComponentModel.DataAnnotations;

namespace AccessingChildcareEntitlementChecker.Web.Models.CheckChildDetails;

public class ChildSummaryViewModel
{
    public ChildSummaryViewModel(Child child)
    {
        Title = child.Name;
        ChildId = child.ChildId;

        var rows = new List<SummaryRowViewModel>();

        if (child.BirthStatus == BirthStatus.Born)
        {
            rows.Add(new SummaryRowViewModel(
                "What is {0}'s date of birth?",
                child.Name,
                child.BirthDate!.Value.ToString("d MMMM yyyy"),
                "BornChildDetails", "ChildBirthDate", child.ChildId));
            rows.Add(new SummaryRowViewModel(
                "What is your relationship to {0}?",
                child.Name,
                DisplayName(child.BornRelationship!.Value),
                "BornChildDetails", "ChildRelationship", child.ChildId));
            rows.Add(new SummaryRowViewModel(
                "Does {0} get any of the following support?",
                child.Name,
                string.Join(", ", child.ChildSupportOptions.Select(cso => DisplayName(cso))),
                "BornChildDetails", "ChildSupport", child.ChildId));
        }

        if (child.BirthStatus == BirthStatus.Due)
        {
            rows.Add(new SummaryRowViewModel(
                "What is this child's due date?",
                child.Name,
                child.DueDate!.Value.ToString("d MMMM yyyy"),
                "ExpectedChildDetails", "ChildDueDate", child.ChildId));
            rows.Add(new SummaryRowViewModel(
                "What will your relationship be to this child?",
                child.Name,
                DisplayName(child.ExpectedRelationship!.Value),
                "ExpectedChildDetails", "ExpectedChildRelationship", child.ChildId));
        }

        Rows = rows;
    }

    public string Title { get; }

    public string ChildId { get; }

    public IReadOnlyList<SummaryRowViewModel> Rows { get; }

    private static string DisplayName<TEnum>(TEnum value)
        where TEnum : struct, Enum
    {
        return typeof(TEnum)
            .GetMember(value.ToString())
            .First()
            .GetCustomAttributes(typeof(DisplayAttribute), inherit: false)
            .Cast<DisplayAttribute>()
            .First()
            .Name!;
    }
}
