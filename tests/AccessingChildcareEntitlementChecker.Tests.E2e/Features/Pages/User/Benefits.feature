Feature: Do you get any of these benefits?

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I answer questions as follows:
		| Question                                      | Answer                   |
		| What is your age?                             | Under 18                 |
		| What is your nationality?                     | British or Irish citizen |
		| Are you in paid work?                         | No                       |
		| Does your household receive universal credit? | Yes                      |

Scenario: Page load
	When the page header is "Do you get any of these benefits?"
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
		| No, I do not get any of these benefits               |

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
	And the error summary and inline validation should be "Select any benefits you get, or select 'No, I do not get any of these benefits'"

Scenario: Continue with Carer's Allowance
	When I select the "Carer's Allowance" checkbox
	And I click on Continue
	Then the page header is "Do you already get any of this child care support?"

@ignore
Scenario: Back navigation from Is your adjusted net income more than £100,000 a year?
	Given I answer "Are you in paid work?" as "Yes"
	And I answer "How would you describe your work status?" as "Paid employment"
	And I answer "On average, do you earn £203 a week or more before tax?" as "Yes"
	And I answer "Is your adjusted net income more than £100,000 a year?" as "Yes"
	When I click the back link
	Then the page header is "Is your adjusted net income more than £100,000 a year?"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Does your household receive universal credit?"
