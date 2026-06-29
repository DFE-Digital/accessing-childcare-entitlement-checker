Feature: Does your household receive universal credit?

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I answer questions as follows:
		| Question                  | Answer                   |
		| What is your age?         | Under 18                 |
		| What is your nationality? | British or Irish citizen |

Scenario: Page load
	Given I answer "Are you in paid work?" as "No, I am not in work"
	When the page header is "Does your household receive universal credit?"
	Then I should see 2 radio buttons with the following options:
		| Option |
		| Yes    |
		| No     |

Scenario: Radio button selection
	Given I answer "Are you in paid work?" as "No, I am not in work"
	When I select the "Yes" radio button
	And I select the "No" radio button
	Then the "No" radio button should be selected
	And all other options should be deselected

Scenario: Continue without selection
	Given I answer "Are you in paid work?" as "No, I am not in work"
	When I do not select a radio button
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select if your household receives universal credit"

Scenario: Continue with Yes
	Given I answer "Are you in paid work?" as "No, I am not in work"
	When I select the "Yes" radio button
	And I click on Continue
	Then the page header is "Do you get any of these benefits?"

Scenario: Back navigation
	Given I answer "Are you in paid work?" as "No, I am not in work"
	When I click the back link
	Then the page header is "Are you in paid work?"

Scenario: Back navigation from Have you been self-employed for less than 12 months?
	Given I answer "Are you in paid work?" as "Yes"
	And I answer "How would you describe your work status?" as "Self-employed"
	And I answer "Have you been self-employed for less than 12 months?" as "Yes"
	When I click the back link
	Then the page header is "Have you been self-employed for less than 12 months?"

Scenario: Back navigation from On average, do you earn £203 a week or more before tax?
	Given I answer "Are you in paid work?" as "Yes"
	And I answer "How would you describe your work status?" as "Paid employment"
	And I answer "On average, do you earn £203 a week or more before tax?" as "No"
	When I click the back link
	Then the page header is "On average, do you earn £203 a week or more before tax?"

Scenario: Back navigation from Is your adjusted net income more than £100,000 a year?
	Given I answer "Are you in paid work?" as "Yes"
	And I answer "How would you describe your work status?" as "Paid employment"
	And I answer "On average, do you earn £203 a week or more before tax?" as "Yes"
	And I answer "Is your adjusted net income more than £100,000 a year?" as "No"
	When I click the back link
	Then the page header is "Is your adjusted net income more than £100,000 a year?"
