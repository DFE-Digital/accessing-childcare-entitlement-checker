using Dfe.Acec.Web.Models;
using Dfe.Acec.Web.Models.BornChildDetails;

namespace Dfe.Acec.Web.Services;

public class Child
{
    public string ChildId { get; set; }

    public string Name { get; set; }

    public BirthStatus? BirthStatus { get; set; }

    public DateOnly? BirthDate { get; set; }

    public List<ChildSupport> ChildSupportOptions { get; set; } = [];

    public DateOnly? DueDate { get; set; }

    public Child(string childId, string name)
    {
        ChildId = childId;
        Name = name;
    }
}
