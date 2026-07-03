Feature: How do you receive your childcare vouchers?

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I answer questions as follows:
		| Question                                                   | Answer                   |
		| What is your age?                                          | Under 18                 |
		| What is your nationality?                                  | British or Irish citizen |
		| Are you in paid work?                                      | No, I am not in work     |
		| Does your household receive universal credit?              | Yes                      |
		| Do you get any of these benefits?                          | Carer's Allowance        |
		| Do you already get any of these to help pay for childcare? | Childcare vouchers       |

Scenario: Page load
	When the page header is "How do you receive your childcare vouchers?"
	Then I should see 3 radio buttons with the following options:
		| Option                                 |
		| A workplace nursery scheme             |
		| Your employer arranges with a provider |
		| Through a salary sacrifice scheme      |
	And no radio buttons are selected

Scenario: Radio button selection
	When I select the "A workplace nursery scheme" radio button
	And I select the "Your employer arranges with a provider" radio button
	Then the "Your employer arranges with a provider" radio button should be selected
	And all other options should be deselected

Scenario: Continue without selection
	When I do not select a radio button
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select how you receive your childcare vouchers"

Scenario: Continue with A workplace nursery scheme
	When I select the "A workplace nursery scheme" radio button
	And I click on Continue
	Then the page header is "Do you live with a partner?"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Do you already get any of these to help pay for childcare?"
