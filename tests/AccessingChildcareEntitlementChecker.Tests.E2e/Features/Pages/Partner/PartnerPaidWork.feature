Feature: Is your partner in paid work?

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I fill in my own details
	And I answer questions as follows:
		| Question                                                 | Answer                   |
		| Do you live with a partner?                              | Yes                      |
		| What is your partner's age?                              | 21 or over               |
		| Which of these best describes your partners nationality? | British or Irish citizen |

Scenario: Page load
	When the page header is "Is your partner in paid work?"
	Then I should see 3 radio buttons with the following options:
		| Option                      |
		| Yes                         |
		| No                          |
		| They are on leave from work |

Scenario: Radio button selection
	When I select the "Yes" radio button
	And I select the "No" radio button
	Then the "No" radio button should be selected
	And all other options should be deselected

Scenario: Continue without selection
	When I do not select a radio button
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select if your partner is in paid work"

Scenario: Continue with Yes
	When I select the "Yes" radio button
	And I click on Continue
	Then the page header is "How would you describe your partner's work status?"

Scenario: Continue with They are on leave from work
	When I select the "They are on leave from work" radio button
	And I click on Continue
	Then the page header is "What type of leave is your partner on?"

Scenario: Continue with No
	When I select the "No" radio button
	And I click on Continue
	Then the page header is "Does your partner get any of these benefits?"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Which of these best describes your partners nationality?"

Scenario: Back navigation from Does your partner have settled or pre-settled status under the EU Settlement Scheme?
	Given I answer "Which of these best describes your partners nationality?" as "Citizen of an EU country, EEA country or Switzerland"
	And I answer "Does your partner have settled or pre-settled status under the EU Settlement Scheme?" as "Yes"
	When I click the back link
	Then the page header is "Does your partner have settled or pre-settled status under the EU Settlement Scheme?"
