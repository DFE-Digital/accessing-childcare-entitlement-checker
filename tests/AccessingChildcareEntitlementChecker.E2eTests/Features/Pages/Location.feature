@RequiresLocationPage
Feature: Location

Background:
	Given I am on the childcare entitlement checker website
	And I click the link to start the journey

Scenario: Page load
	When the page header is "Where do you live?"
	Then I should see 4 radio buttons with the following options:
		| Option           |
		| England          |
		| Scotland         |
		| Wales            |
		| Northern Ireland |
	And no radio buttons are selected

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
	Then the page header is "Add details about your children"

Scenario: Continue to check child details when I already have children
	Given I answer "Where do you live?" as "England"
	And I answer questions as follows:
		| Question                        | Answer   |
		| Add details about your children | Aydin    |
		| Has this child been born yet?   | No       |
		| What is this child's due date?  | Tomorrow |
	And I check my children's details and click on Continue
	And I answer questions as follows:
		| Question                                                   | Answer                        |
		| What is your age?                                          | Under 18                      |
		| What is your nationality?                                  | British or Irish citizen      |
		| Are you in paid work?                                      | No, I am not in work          |
		| Does your household receive universal credit?              | Yes                           |
		| Do you get any of these benefits?                          | Carer's Allowance             |
		| Do you already get any of these to help pay for childcare? | No, I do not get any of these |
		| Do you live with a partner?                                | No                            |
	When I click the Change link in the "Your details" summary list for "Where do you live?"
	And I answer "Where do you live?" as "England"
	Then the page header is "Check your children's details"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Before you continue"