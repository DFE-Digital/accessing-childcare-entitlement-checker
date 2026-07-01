Feature: What is this child's due date?

Background:
	Given I am on the childcare entitlement checker website
	And I click the link to start the journey
	And I answer "Where do you live?" as "England"
	And I answer questions for "Aydin" as follows:
		| Question                        | Answer |
		| Add details about your children | Aydin  |
		| Has this child been born yet?   | No     |

Scenario: Page load
	When the page header is "What is this child's due date?"
	Then I should see the hint text "For example, 30 9 2026"
	And I should see a date entry input

# The GDS component covers date validity, we do a single test to validate
# we've integrated it correctly.
Scenario: Enter an invalid date
	When I enter the day "46" month "3" and year "2026"
	And I click on Continue
	Then the page header is "What is this child's due date?"
	And an error summary box should appear at the top of the page
	And the error summary and inline validation should be "The due date must be a real date"

Scenario: Enter a past date
	When I enter yesterdays date
	And I click on Continue
	Then the page header is "What is this child's due date?"
	And an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Enter a due date in the future"

Scenario: Continue without entering a date
	When I do not enter a date
	And I click on Continue
	Then the page header is "What is this child's due date?"
	And an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Enter this child's due date"

Scenario: Continue with a future date
	When I enter tomorrow's date
	And I click on Continue
	Then the page header is "Check your children's details"

Scenario: Continue with todays date
	When I enter todays date
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Enter a due date in the future"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Has this child been born yet?"