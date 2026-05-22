Feature: Has Partner

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I fill in my own details

Scenario: Page load
	When the page header is "Do you live with a partner?"
	Then I should see 2 radio buttons with the following options:
		| Option |
		| Yes    |
		| No     |

Scenario: Radio button selection
	When I select the "No" radio button
	And I select the "Yes" radio button
	Then the "Yes" radio button should be selected
	And all other options should be deselected

Scenario: Continue without selection
	When I do not select a radio button
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select do you live with a partner"

Scenario: Continue with selection
	When I select the "Yes" radio button
	And I click on Continue
	Then the page header is "What is your partner's age?"

Scenario: Back navigation from How do you receive your childcare vouchers?
	When I click the back link
	Then the page header is "How do you receive your childcare vouchers?"

Scenario: Back navigation from Do you already get any of this childcare support?
	Given I answer "Do you already get any of this childcare support?" as "No, I do not get any of this childcare support"
	When I click the back link
	Then the page header is "Do you already get any of this childcare support?"