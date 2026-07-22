Feature: Has this child been born yet?

Background:
	Given I am on the childcare entitlement checker website
	And I click the link to start the journey
	And I answer "Where do you live?" as "England"
	And I answer questions as follows:
		| Question                        | Answer |
		| Add details about your children | Sara   |

Scenario: Page load
	When the page header is "Has this child been born yet?"
	Then I should see 2 radio buttons with the following options:
		| Option |
		| Yes    |
		| No     |
	And no radio buttons are selected

Scenario: Radio button selection
	When I select the "Yes" radio button
	And I select the "No" radio button
	Then the "No" radio button should be selected
	And all other options should be deselected

Scenario: Continue without selection
	When I do not select a radio button
	And I click on Continue
	Then the page header is "Has this child been born yet?"
	And an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select if this child has been born"

Scenario: Continue with yes
	When I select the "Yes" radio button
	And I click on Continue
	Then the page header is "What is Sara's date of birth?"

Scenario: Continue with no
	When I select the "No" radio button
	And I click on Continue
	Then the page header is "What is this child's due date?"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Add details about your children"