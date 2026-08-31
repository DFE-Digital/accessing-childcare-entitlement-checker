using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.BornChildDetails;

namespace Dfe.Acec.Web.Services;

public class Child(string childId, string name)
{
    public string ChildId { get; set; } = childId;

    public string Name { get; set; } = name;

    public BirthStatus? BirthStatus { get; set; }

    public DateOnly? BirthDate { get; set; }

    public List<ChildSupport> ChildSupportOptions { get; set; } = [];

    public DateOnly? DueDate { get; set; }
}
