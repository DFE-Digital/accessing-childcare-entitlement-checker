Feature: Has your partner been self-employed for less than 12 months?

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I answer questions as follows:
		| Question                                                 | Answer                                                      |
		| What is your age?                                        | Under 18                                                    |
		| What is your nationality?                                | British or Irish citizen                                    |
		| Are you in paid work?                                    | No                                                          |
		| Does your household receive universal credit?            | Yes                                                         |
		| Do you get any of these benefits?                        | Carer's Allowance                                           |
		| Do you already get any of this childcare support?        | A childcare bursary or grant (as part of education funding) |
		| Which of these best describes your partners nationality? | British or Irish citizen                                    |
		| Is your partner in paid work?                            | Yes                                                         |
		| How would you describe your partner's work status?       | Self-employed                                               |

Scenario: Page load
	When the page header is "Has your partner been self-employed for less than 12 months?"
	Then I should see 2 radio buttons with the following options:
		| Option |
		| Yes    |
		| No     |

Scenario: Radio button selection
	When I select the "Yes" radio button
	And I select the "No" radio button
	Then the "No" radio button should be selected
	And all other options should be deselected

Scenario: Continue without selection
	When I do not select a radio button
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select if your partner has been self-employed for less than 12 months"

Scenario: Continue with Yes
	When I select the "Yes" radio button
	And I click on Continue
	Then the page header is "Does your partner get any of these benefits?"

Scenario: Continue with No
	When I select the "No" radio button
	And I click on Continue
	Then the page header is "On average, does your partner earn £203 a week or more before tax?"

Scenario: Back navigation
	When I click the back link
	Then the page header is "How would you describe your partner's work status?"
