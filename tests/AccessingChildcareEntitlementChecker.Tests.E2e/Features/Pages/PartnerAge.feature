Feature: Partner Age

Background:
	Given I am on the 'How old is your partner?' page

Scenario: Page load
	When the page header is "How old is your partner?"
	Then I should see 3 radio buttons with the following options:
		| Option               |
		| Under 18 years old   |
		| 18 to 20 years old   |
		| 21 years old or over |

Scenario: Radio button selection
	When I select the "Under 18 years old" radio button
	And I select the "18 to 20 years old" radio button
	Then the "18 to 20 years old" radio button should be selected
	And all other options should be deselected

Scenario: Continue without selection
	When I do not select a radio button
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select your partner's age"

Scenario: Continue with selection
	When I select the "Under 18 years old" radio button
	And I click on Continue
	Then the page header is "Next step placeholder"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Do you live with a partner?"