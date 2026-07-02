Feature: Do you already get any of these to help pay for childcare?

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I answer questions as follows:
		| Question                                      | Answer                   |
		| What is your age?                             | Under 18                 |
		| What is your nationality?                     | British or Irish citizen |
		| Are you in paid work?                         | No, I am not in work     |
		| Does your household receive universal credit? | Yes                      |
		| Do you get any of these benefits?             | Carer's Allowance        |

Scenario: Page load
	When the page header is "Do you already get any of these to help pay for childcare?"
	Then I should see 3 checkboxes with the following options:
		| Checkbox                      |
		| Childcare vouchers            |
		| A childcare bursary or grant  |
		| No, I do not get any of these |
	And no checkboxes are selected

Scenario: Checkbox selection
	When I select the "Childcare vouchers" checkbox
	And I select the "A childcare bursary or grant" checkbox
	Then the following checkboxes should be selected:
		| Checkbox                     |
		| Childcare vouchers           |
		| A childcare bursary or grant |

Scenario: Continue without selection
	When I do not select a checkbox
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select any of this childcare support you already get, or select 'No, I do not get any of these'"

Scenario: Continue with Childcare vouchers
	When I select the "Childcare vouchers" checkbox
	And I click on Continue
	Then the page header is "How do you receive your childcare vouchers?"

Scenario: Continue with A childcare bursary or grant
	When I select the "A childcare bursary or grant" checkbox
	And I click on Continue
	Then the page header is "Do you live with a partner?"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Do you get any of these benefits?"
