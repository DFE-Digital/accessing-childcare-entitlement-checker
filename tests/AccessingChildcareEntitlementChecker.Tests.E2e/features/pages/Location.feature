Feature: Location

Background:
	Given I am on the childcare entitlement checker website
	And I click the Start now link

Scenario: Page load
	When the page header is "Where do you live?"
	Then I should see 4 radio buttons with the following options:
		| Option           |
		| England          |
		| Scotland         |
		| Wales            |
		| Northern Ireland |

Scenario: Radio button selection
	When I select the "Scotland" radio button
	And I select the "England" radio button
	Then the "England" radio button should be selected
	And all other options should be deselected

Scenario: Continue without selection
	When I do not select a radio button
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select where you live"

Scenario: Continue with selection
	When I select the "England" radio button
	And I click on Continue
	Then the page header is "Add details of a child"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Check what help you could get with childcare costs"