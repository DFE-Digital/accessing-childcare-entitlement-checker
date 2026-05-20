Feature: Has Partner

Background:
	Given I am on the 'Do you live with a partner?' page

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
	Then the page header is "How old is your partner?"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Where do you live?"