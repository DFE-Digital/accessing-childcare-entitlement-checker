Feature: How does your partner receive childcare vouchers?

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I fill in my own details
	And I answer questions as follows:
		| Question                                                     | Answer                   |
		| Do you live with a partner?                                  | Yes                      |
		| What is your partner's age?                                  | 21 or over               |
		| Is your partner in paid work?                                | No, they are not in work |
		| Does your partner get any of these benefits?                 | Carer's Allowance        |
		| Does your partner already get any of this childcare support? | Childcare vouchers       |

Scenario: Page load
	When the page header is "How does your partner receive childcare vouchers?"
	Then I should see 3 radio buttons with the following options:
		| Option                                 |
		| A workplace nursery scheme             |
		| Your employer arranges with a provider |
		| Through a salary sacrifice scheme      |

Scenario: Radio button selection
	When I select the "A workplace nursery scheme" radio button
	And I select the "Your employer arranges with a provider" radio button
	Then the "Your employer arranges with a provider" radio button should be selected
	And all other options should be deselected

Scenario: Continue without selection
	When I do not select a radio button
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select how your partner receives their childcare vouchers"

Scenario: Continue with A workplace nursery scheme
	When I select the "A workplace nursery scheme" radio button
	And I click on Continue
	Then the page header is "Check your answers"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Does your partner already get any of this childcare support?"
