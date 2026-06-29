Feature: Does your partner have settled or pre-settled status under the EU Settlement Scheme?

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I answer questions as follows:
		| Question                                          | Answer                                         |
		| What is your age?                                 | Under 18                                       |
		| What is your nationality?                         | Citizen of a different country                 |
		| Are you in paid work?                             | No, I am not in work                           |
		| Does your household receive universal credit?     | Yes                                            |
		| Do you get any of these benefits?                 | Carer's Allowance                              |
		| Do you already get any of this childcare support? | No, I do not get any of this childcare support |
	And I answer questions as follows:
		| Question                                                 | Answer                                               |
		| Do you live with a partner?                              | Yes                                                  |
		| What is your partner's age?                              | 21 or over                                           |
		| Which of these best describes your partners nationality? | Citizen of an EU country, EEA country or Switzerland |

Scenario: Page load
	When the page header is "Does your partner have settled or pre-settled status under the EU Settlement Scheme?"
	Then I should see 3 radio buttons with the following options:
		| Option                                                           |
		| Yes                                                              |
		| No                                                               |
		| I applied before 1 July 2021 and am still waiting for a decision |

Scenario: Radio button selection
	When I select the "Yes" radio button
	And I select the "No" radio button
	Then the "No" radio button should be selected
	And all other options should be deselected

Scenario: Continue without selection
	When I do not select a radio button
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select if your partner has settled or pre-settled status"

Scenario: Continue with Yes
	When I select the "Yes" radio button
	And I click on Continue
	Then the page header is "Is your partner in paid work?"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Which of these best describes your partners nationality?"
