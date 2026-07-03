Feature: On average, do you earn x a week or more before tax?

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue

Scenario Outline: Page load for different age groups and employment statuses
	Given I answer questions as follows:
		| Question                                 | Answer                   |
		| What is your age?                        | <Age>                    |
		| What is your nationality?                | British or Irish citizen |
		| Are you in paid work?                    | Yes                      |
		| How would you describe your work status? | <Work Status>            |
	When the page header is "On average, do you earn £<Earnings> a week or more before tax?"
	Then I should see 2 radio buttons with the following options:
		| Option |
		| Yes    |
		| No     |
	And no radio buttons are selected

Examples:
	| Age        | Work Status     | Earnings |
	| Under 18   | Paid employment |      128 |
	| 18 to 20   | Paid employment |      174 |
	| 21 or over | Paid employment |      203 |
	| Under 18   | Apprentice      |      128 |
	| 18 to 20   | Apprentice      |      128 |
	| 21 or over | Apprentice      |      128 |

Scenario Outline: Page load for different age groups on parental leave
	Given I answer questions as follows:
		| Question                                 | Answer                          |
		| What is your age?                        | <Age>                           |
		| What is your nationality?                | British or Irish citizen        |
		| Are you in paid work?                    | Yes, but I am on parental leave |
		| Which child are you on leave for?        | Sara                            |
		| How would you describe your work status? | <Work Status>                   |
	When the page header is "On average, will you earn £<Earnings> a week or more before tax when your parental leave ends?"
	Then I should see 2 radio buttons with the following options:
		| Option |
		| Yes    |
		| No     |
	And no radio buttons are selected

Examples:
	| Age        | Work Status     | Earnings |
	| Under 18   | Paid employment |      128 |
	| 18 to 20   | Paid employment |      174 |
	| 21 or over | Paid employment |      203 |
	| Under 18   | Apprentice      |      128 |
	| 18 to 20   | Apprentice      |      128 |
	| 21 or over | Apprentice      |      128 |

Scenario Outline: Continue without selection for different age groups and employment statuses
	Given I answer questions as follows:
		| Question                                 | Answer                   |
		| What is your age?                        | <Age>                    |
		| What is your nationality?                | British or Irish citizen |
		| Are you in paid work?                    | Yes                      |
		| How would you describe your work status? | <Work Status>            |
	When I do not select a radio button
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select if you earn £<Earnings> a week or more before tax"

Examples:
	| Age        | Work Status     | Earnings |
	| Under 18   | Paid employment |      128 |
	| 18 to 20   | Paid employment |      174 |
	| 21 or over | Paid employment |      203 |
	| Under 18   | Apprentice      |      128 |
	| 18 to 20   | Apprentice      |      128 |
	| 21 or over | Apprentice      |      128 |

Scenario: Radio button selection
	Given I answer questions as follows:
		| Question                                 | Answer                   |
		| What is your age?                        | Under 18                 |
		| What is your nationality?                | British or Irish citizen |
		| Are you in paid work?                    | Yes                      |
		| How would you describe your work status? | Paid employment          |
	When I select the "Yes" radio button
	And I select the "No" radio button
	Then the "No" radio button should be selected
	And all other options should be deselected

Scenario: Continue with Yes
	Given I answer questions as follows:
		| Question                                 | Answer                   |
		| What is your age?                        | Under 18                 |
		| What is your nationality?                | British or Irish citizen |
		| Are you in paid work?                    | Yes                      |
		| How would you describe your work status? | Paid employment          |
	When I select the "Yes" radio button
	And I click on Continue
	Then the page header is "Is your adjusted net income more than £100,000 a year?"

Scenario: Continue with No
	Given I answer questions as follows:
		| Question                                 | Answer                   |
		| What is your age?                        | Under 18                 |
		| What is your nationality?                | British or Irish citizen |
		| Are you in paid work?                    | Yes                      |
		| How would you describe your work status? | Paid employment          |
	When I select the "No" radio button
	And I click on Continue
	Then the page header is "Does your household receive universal credit?"

Scenario: Back navigation
	Given I answer questions as follows:
		| Question                                 | Answer                   |
		| What is your age?                        | Under 18                 |
		| What is your nationality?                | British or Irish citizen |
		| Are you in paid work?                    | Yes                      |
		| How would you describe your work status? | Paid employment          |
	When I click the back link
	Then the page header is "How would you describe your work status?"

Scenario: Back navigation from Have you been self-employed for less than 12 months?
	Given I answer questions as follows:
		| Question                                             | Answer                   |
		| What is your age?                                    | Under 18                 |
		| What is your nationality?                            | British or Irish citizen |
		| Are you in paid work?                                | Yes                      |
		| How would you describe your work status?             | Self-employed            |
		| Have you been self-employed for less than 12 months? | No                       |
	When I click the back link
	Then the page header is "Have you been self-employed for less than 12 months?"