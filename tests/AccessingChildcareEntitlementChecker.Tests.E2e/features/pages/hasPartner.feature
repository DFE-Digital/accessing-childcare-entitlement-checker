Feature: Has Partner

Scenario: Page load
	Given I am on the partner page
	Then the page header is "Do you live with a partner?"
	And I see "Yes"
	And I see "No"

Scenario: Radio button selection
	Given I am on the partner page
	When I select "Yes" for "HasPartner"
	Then "Yes" is selected for "HasPartner"
	And "No" is not selected for "HasPartner"

Scenario: Continue without selection
	Given I am on the partner page
	When I click on Continue
	Then the "HasPartner" error is "Select do you live with a partner"

Scenario: Continue with selection
	Given I am on the partner page
	When I select "Yes" for "HasPartner"
	And I click on Continue
	Then I see the text "Next step placeholder"

Scenario: Back navigation
	Given I am on the partner page
	When I click the back link
	Then the page header is "Where do you live?"