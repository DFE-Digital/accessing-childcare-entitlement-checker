Feature: Has Partner

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue

Scenario: Page load
	Given I fill in my own details
	When the page header is "Do you live with a partner?"
	Then I should see 2 radio buttons with the following options:
		| Option |
		| Yes    |
		| No     |
	And no radio buttons are selected

Scenario: Radio button selection
	Given I fill in my own details
	When I select the "No" radio button
	And I select the "Yes" radio button
	Then the "Yes" radio button should be selected
	And all other options should be deselected

Scenario: Continue without selection
	Given I fill in my own details
	When I do not select a radio button
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select if you live with a partner"

Scenario: Continue with selection
	Given I fill in my own details
	When I select the "Yes" radio button
	And I click on Continue
	Then the page header is "What is your partner's age?"

Scenario: Back navigation from How do you receive your childcare vouchers?
	Given I fill in my own details
	When I click the back link
	Then the page header is "How do you receive your childcare vouchers?"

Scenario: Back navigation from Do you already get any of this childcare support?
	Given I answer the following pages:
		| Page                                                    | Answer                                         |
		| What is your age?                                       | Under 18                                       |
		| What is your nationality?                               | British or Irish citizen                       |
		| Are you in paid work?                                   | Yes                                            |
		| How would you describe your work status?                | Self-employed                                  |
		| Have you been self-employed for less than 12 months?    | No                                             |
		| On average, do you earn £203 a week or more before tax? | Yes                                            |
		| Is your adjusted net income more than £100,000 a year?  | No                                             |
		| Does your household receive universal credit?           | Yes                                            |
		| Do you get any of these benefits?                       | Carer's Allowance                              |
		| Do you already get any of this childcare support?       | No, I do not get any of this childcare support |
	When I click the back link
	Then the page header is "Do you already get any of this childcare support?"
