Feature: What is child's date of birth?

Background:
	Given I am on the childcare entitlement checker website
	And I click the link to start the journey
	And I answer "Where do you live?" as "England"
	And I answer questions for "Sara" as follows:
		| Question                        | Answer |
		| Add details about your children | Sara   |
		| Has this child been born yet?   | Yes    |

Scenario: Page load
	When the page header is "What is Sara's date of birth?"
	Then I should see the hint text "For example, 31 3 2022"
	And I should see a date entry input
	And the date entry input is empty

# The GDS component covers date validity, we do a single test to validate
# we've integrated it correctly.
Scenario: Enter an invalid date
	When I enter the day "46" month "3" and year "2026"
	And I click on Continue
	Then the page header is "What is Sara's date of birth?"
	And an error summary box should appear at the top of the page
	And the error summary and inline validation should be "The date of birth must be a real date"

Scenario: Enter a a future date
	When I enter tomorrow's date
	And I click on Continue
	Then the page header is "What is Sara's date of birth?"
	And an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Enter a date of birth in the past"

Scenario: Continue without entering a date
	When I do not enter a date
	And I click on Continue
	Then the page header is "What is Sara's date of birth?"
	And an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Enter this child's date of birth"

Scenario: Continue with a past date
	When I enter yesterdays date
	And I click on Continue
	Then the page header is "Does Sara get any of the following support?"

Scenario: Continue with todays date
	When I enter todays date
	And I click on Continue
	Then the page header is "Does Sara get any of the following support?"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Has this child been born yet?"