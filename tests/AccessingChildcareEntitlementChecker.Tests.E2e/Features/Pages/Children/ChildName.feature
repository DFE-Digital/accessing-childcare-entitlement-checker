Feature: Add details of a child

Background:
	Given I am on the childcare entitlement checker website
	And I click the Start now link
	And I answer "Where do you live?" as "England"

Scenario: Page load
	When the page header is "Add details about your children"
	Then I should see a text box with the label "What name should we use for this child?"

Scenario: Continue with no name
	When I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Enter a name for your child"

Scenario: Continue with name
	When I enter the text "Child A" into "What name should we use for this child?"
	And I click on Continue
	Then the page header is "Has this child been born yet?"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Where do you live?"