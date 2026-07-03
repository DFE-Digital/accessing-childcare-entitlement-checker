namespace AccessingChildcareEntitlementChecker.E2eTests.Pages;

internal static class PageNames
{
    // User Pages
    public const string StartPage = "Check what help you could get with childcare costs";
    public const string Location = "Where do you live?";
    public const string UserAge = "What is your age?";
    public const string Nationality = "What is your nationality?";
    public const string PaidWork = "Are you in paid work?";
    public const string ParentalLeave = "Which child are you on leave for?";
    public const string WorkStatus = "How would you describe your work status?";
    public const string SelfEmployedDuration = "Have you been self-employed for less than 12 months?";
    public const string WeeklyEarnings = "On average, __PLACEHOLDER__ you expect to earn __PLACEHOLDER__ a week or more __PLACEHOLDER__";
    public const string YearlyEarnings = "Do you expect your adjusted net income to be more than £100,000 for the current tax year?";
    public const string UniversalCredit = "Does your household receive universal credit?";
    public const string Benefits = "Do you get any of these benefits?";
    public const string ChildcareSupport = "Do you already get any of these to help pay for childcare?";
    public const string ChildcareVoucherReceipt = "How do you receive your childcare vouchers?";
    public const string UserSettledStatus = "Do you have settled or pre-settled status under the EU Settlement Scheme?";
    public const string TypeOfLeave = "Which child are you on leave for?";

    // Partner Pages
    public const string HasPartner = "Do you live with a partner?";
    public const string PartnerAge = "What is your partner's age?";
    public const string PartnerPaidWork = "Is your partner in paid work?";
    public const string PartnerParentalLeave = "Which child is your partner on leave for?";
    public const string PartnerWorkStatus = "How would you describe your partner's work status?";
    public const string PartnerSelfEmployedDuration = "Has your partner been self-employed for less than 12 months?";
    public const string PartnerWeeklyEarnings = "On average, __PLACEHOLDER__ your partner expect to earn __PLACEHOLDER__ a week or more __PLACEHOLDER__";
    public const string PartnerYearlyEarnings = "Does your partner expect their adjusted net income to be more than £100,000 for the current tax year?";
    public const string PartnerNationality = "Which of these best describes your partners nationality?";
    public const string PartnerSettledStatus = "Does your partner have settled or pre-settled status under the EU Settlement Scheme?";
    public const string PartnerBenefits = "Does your partner get any of these benefits?";
    public const string PartnerChildcareSupport = "Does your partner already get any of these to help pay for childcare?";
    public const string PartnerChildcareVoucherReceipt = "How does your partner receive childcare vouchers?";
    public const string PartnerLeaveType = "Which child is your partner on leave for?";
    public const string PartnerLeaveWeeklyEarnings = "On average, will your partner expect to earn £__PLACEHOLDER__ a week or more before tax when their parental leave ends?";

    // Child Pages
    public const string ChildName = "Add details about your children";
    public const string ChildIsBorn = "Has this child been born yet?";
    public const string ChildDueDate = "What is this child's due date?";
    public const string ChildBirthDate = "What is __PLACEHOLDER__'s date of birth?";
    public const string ChildSupport = "Does __PLACEHOLDER__ get any of the following support?";

    public static System.Text.RegularExpressions.Regex GetRegexForPattern(string pattern)
    {
        var escaped = System.Text.RegularExpressions.Regex.Escape(pattern);
        var regexPattern = @"^\s*" + escaped.Replace("__PLACEHOLDER__", "(.*?)") + @"\s*$";
        return new System.Text.RegularExpressions.Regex(regexPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
    }
}
