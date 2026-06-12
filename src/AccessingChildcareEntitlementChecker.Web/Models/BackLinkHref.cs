namespace AccessingChildcareEntitlementChecker.Web.Models
{
    public readonly record struct BackLinkHref(string Value)
    {
        public override string ToString() => Value;
    }
}
