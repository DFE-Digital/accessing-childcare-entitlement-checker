using AccessingChildcareEntitlementChecker.Web.Models;
using AccessingChildcareEntitlementChecker.Web.Models.BornChildDetails;
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

        if (child.BirthStatus == BirthStatusOption.Born)
        {
            rows.Add(new SummaryRowViewModel(
                "What is {0}'s date of birth?",
                child.Name,
                child.BirthDate?.ToString("d MMMM yyyy") ?? string.Empty,
                "BornChildDetails", "ChildBirthDate", child.ChildId));
            rows.Add(new SummaryRowViewModel(
                "What is your relationship to {0}?",
                child.Name,
                DisplayName(child.BornRelationship),
                "BornChildDetails", "ChildRelationship", child.ChildId));
            rows.Add(new SummaryRowViewModel(
                "Does {0} get any of the following support?",
                child.Name,
                string.Join(", ", child.ChildSupportOptions.Select(cso => DisplayName(cso))),
                "BornChildDetails", "ChildSupport", child.ChildId));
        }

        if (child.BirthStatus == BirthStatusOption.Due)
        {
            rows.Add(new SummaryRowViewModel(
                "What is this child's due date?",
                child.Name,
                child.DueDate?.ToString("d MMMM yyyy") ?? string.Empty,
                "ExpectedChildDetails", "ChildDueDate", child.ChildId));
            rows.Add(new SummaryRowViewModel(
                "What will your relationship be to this child?",
                child.Name,
                DisplayName(child.ExpectedRelationship),
                "ExpectedChildDetails", "ExpectedChildRelationship", child.ChildId));
        }

        Rows = rows;
    }

    public string Title { get; }

    public string ChildId { get; }

    public IReadOnlyList<SummaryRowViewModel> Rows { get; }

    private static string DisplayName<TEnum>(TEnum? value)
        where TEnum : struct, Enum
    {
        return value.HasValue ? DisplayName(value.Value) : string.Empty;
    }

    private static string DisplayName<TEnum>(TEnum value)
        where TEnum : struct, Enum
    {
        return typeof(TEnum)
            .GetMember(value.ToString())
            .FirstOrDefault()?
            .GetCustomAttributes(typeof(DisplayAttribute), inherit: false)
            .Cast<DisplayAttribute>()
            .FirstOrDefault()?
            .Name ?? value.ToString();
    }

}
