Feature: User PaidWork

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I answer questions as follows:
		| Question                  | Answer                   |
		| What is your age?         | Under 18                 |
		| What is your nationality? | British or Irish citizen |

Scenario: Page load
	When the page header is "Are you in paid work?"
	Then I should see 3 radio buttons with the following options:
		| Option                  |
		| Yes                     |
		| No                      |
		| I am on leave from work |

Scenario: Radio button selection
	When I select the "Yes" radio button
	And I select the "No" radio button
	Then the "No" radio button should be selected
	And all other options should be deselected

Scenario: Continue without selection
	When I do not select a radio button
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select if you are in paid work"

Scenario: Continue with Yes
	When I select the "Yes" radio button
	And I click on Continue
	Then the page header is "How would you describe your work status?"

Scenario: Continue with I am on leave from work
	When I select the "I am on leave from work" radio button
	And I click on Continue
	Then the page header is "TypeOfLeave"

Scenario: Continue with No
	When I select the "No" radio button
	And I click on Continue
	Then the page header is "Does your household receive universal credit?"

Scenario: Back navigation from What is your nationality?
	When I click the back link
	Then the page header is "What is your nationality?"

Scenario: Back navigation from Do you have settled or pre-settled status under the EU Settlement Scheme?
	Given I answer "What is your nationality?" as "Citizen of an EU country, EEA country or Switzerland"
	And I answer "Do you have settled or pre-settled status under the EU Settlement Scheme?" as "Yes"
	When I click the back link
	Then the page header is "Do you have settled or pre-settled status under the EU Settlement Scheme?"
