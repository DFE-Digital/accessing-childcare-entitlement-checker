Feature: Does your partner get any of these benefits?

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I fill in my own details
	And I answer questions as follows:
		| Question                      | Answer                   |
		| Do you live with a partner?   | Yes                      |
		| What is your partner's age?   | 21 or over               |
		| Is your partner in paid work? | No, they are not in work |

Scenario: Page load
	When the page header is "Does your partner get any of these benefits?"
	Then I should see 9 checkboxes with the following options:
		| Checkbox                                             |
		| Carer's Allowance                                    |
		| Contribution-based Employment and Support Allowance  |
		| Employment and support allowance (ESA)               |
		| Guaranteed element of Pension Credit                 |
		| Incapacity Benefit                                   |
		| Limited capability for work (LCW)                    |
		| Limited capability for work related activity (LCWRA) |
		| Severe Disablement Allowance                         |
		| No, they do not get any of these benefits            |

Scenario: Checkbox selection
	When I select the "Carer's Allowance" checkbox
	And I select the "Contribution-based Employment and Support Allowance" checkbox
	Then the following checkboxes should be selected:
		| Checkbox                                            |
		| Carer's Allowance                                   |
		| Contribution-based Employment and Support Allowance |

Scenario: Continue without selection
	When I do not select a checkbox
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select any benefits your partner gets, or select 'No, they do not get any of these benefits'"

Scenario: Continue with Carer's Allowance
	When I select the "Carer's Allowance" checkbox
	And I click on Continue
	Then the page header is "Does your partner already get any of this childcare support?"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Is your partner in paid work?"

Scenario: Back navigation from Has your partner been self-employed for less than 12 months?
	Given I answer "Is your partner in paid work?" as "Yes"
	And I answer "How would you describe your partner's work status?" as "Self-employed"
	And I answer "Has your partner been self-employed for less than 12 months?" as "Yes"
	When I click the back link
	Then the page header is "Has your partner been self-employed for less than 12 months?"

Scenario: Back navigation from On average, does your partner earn £203 a week or more before tax?
	Given I answer "Is your partner in paid work?" as "Yes"
	And I answer "How would you describe your partner's work status?" as "Paid employment"
	And I answer "On average, does your partner earn £203 a week or more before tax?" as "No"
	When I click the back link
	Then the page header is "On average, does your partner earn £203 a week or more before tax?"

Scenario: Back navigation from Is your partner's adjusted net income more than £100,000 a year?
	Given I answer "Is your partner in paid work?" as "Yes"
	And I answer "How would you describe your partner's work status?" as "Paid employment"
	And I answer "On average, does your partner earn £203 a week or more before tax?" as "Yes"
	And I answer "Is your partner's adjusted net income more than £100,000 a year?" as "Yes"
	When I click the back link
	Then the page header is "Is your partner's adjusted net income more than £100,000 a year?"
