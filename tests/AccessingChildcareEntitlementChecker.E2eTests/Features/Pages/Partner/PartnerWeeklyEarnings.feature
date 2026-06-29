Feature: On average, does your partner earn £203 a week or more before tax?

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I fill in my own details
	And I answer questions as follows:
		| Question                                                 | Answer                   |
		| Do you live with a partner?                              | Yes                      |
		| What is your partner's age?                              | 21 or over               |
		| Is your partner in paid work?                            | Yes                      |

Scenario: Page load
	Given I answer "How would you describe your partner's work status?" as "Paid employment"
	When the page header is "On average, does your partner earn £203 a week or more before tax?"
	Then I should see 2 radio buttons with the following options:
		| Option |
		| Yes    |
		| No     |

Scenario: Radio button selection
	Given I answer "How would you describe your partner's work status?" as "Paid employment"
	When I select the "Yes" radio button
	And I select the "No" radio button
	Then the "No" radio button should be selected
	And all other options should be deselected

Scenario: Continue without selection
	Given I answer "How would you describe your partner's work status?" as "Paid employment"
	When I do not select a radio button
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select if your partner earns £203 a week or more before tax"

Scenario: Continue with Yes
	Given I answer "How would you describe your partner's work status?" as "Paid employment"
	When I select the "Yes" radio button
	And I click on Continue
	Then the page header is "Is your partner's adjusted net income more than £100,000 a year?"

Scenario: Continue with No
	Given I answer "How would you describe your partner's work status?" as "Paid employment"
	When I select the "No" radio button
	And I click on Continue
	Then the page header is "Does your partner get any of these benefits?"

Scenario: Back navigation
	Given I answer "How would you describe your partner's work status?" as "Paid employment"
	When I click the back link
	Then the page header is "How would you describe your partner's work status?"

Scenario: Back navigation from Has your partner been self-employed for less than 12 months?
	Given I answer "How would you describe your partner's work status?" as "Self-employed"
	And I answer "Has your partner been self-employed for less than 12 months?" as "No"
	When I click the back link
	Then the page header is "Has your partner been self-employed for less than 12 months?"
