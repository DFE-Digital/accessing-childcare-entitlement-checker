Feature: User Age

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue

Scenario: Page load
	When the page header is "What is your age?"
	Then I should see 3 radio buttons with the following options:
		| Option     |
		| Under 18   |
		| 18 to 20   |
		| 21 or over |

Scenario: Radio button selection
	When I select the "Under 18" radio button
	And I select the "18 to 20" radio button
	Then the "18 to 20" radio button should be selected
	And all other options should be deselected

Scenario: Continue without selection
	When I do not select a radio button
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select your age"

Scenario: Continue with selection
	When I select the "Under 18" radio button
	And I click on Continue
	Then the page header is "What is your nationality?"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Check your children's details"