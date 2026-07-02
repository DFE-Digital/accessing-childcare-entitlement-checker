Feature: Is your partner in paid work?

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue

Scenario: Page load
	Given I fill in my own details
	And I answer questions as follows:
		| Question                                                 | Answer                   |
		| Do you live with a partner?                              | Yes                      |
		| What is your partner's age?                              | 21 or over               |
	When the page header is "Is your partner in paid work?"
	Then I should see 4 radio buttons with the following options:
		| Option                              |
		| Yes                                 |
		| Yes, but they are on parental leave |
		| Yes, but they are on sick leave     |
		| No, they are not in work            |
	And no radio buttons are selected

Scenario: Radio button selection
	Given I fill in my own details
	And I answer questions as follows:
		| Question                                                 | Answer                   |
		| Do you live with a partner?                              | Yes                      |
		| What is your partner's age?                              | 21 or over               |
	When I select the "Yes" radio button
	And I select the "No, they are not in work" radio button
	Then the "No, they are not in work" radio button should be selected
	And all other options should be deselected

Scenario: Continue without selection
	Given I fill in my own details
	And I answer questions as follows:
		| Question                                                 | Answer                   |
		| Do you live with a partner?                              | Yes                      |
		| What is your partner's age?                              | 21 or over               |
	When I do not select a radio button
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select if your partner is in paid work"

Scenario: Continue with Yes
	Given I fill in my own details
	And I answer questions as follows:
		| Question                                                 | Answer                   |
		| Do you live with a partner?                              | Yes                      |
		| What is your partner's age?                              | 21 or over               |
	When I select the "Yes" radio button
	And I click on Continue
	Then the page header is "How would you describe your partner's work status?"

Scenario: Continue with They are on leave from work
	Given I fill in my own details
	And I answer questions as follows:
		| Question                                                 | Answer                   |
		| Do you live with a partner?                              | Yes                      |
		| What is your partner's age?                              | 21 or over               |
	When I select the "Yes, but they are on parental leave" radio button
	And I click on Continue
	Then the page header is "Which child is your partner on leave for?"

Scenario: Continue with No
	Given I fill in my own details
	And I answer questions as follows:
		| Question                                                 | Answer                   |
		| Do you live with a partner?                              | Yes                      |
		| What is your partner's age?                              | 21 or over               |
	When I select the "No, they are not in work" radio button
	And I click on Continue
	Then the page header is "Does your partner get any of these benefits?"

Scenario: Back navigation
	Given I fill in my own details
	And I answer questions as follows:
		| Question                                                 | Answer                   |
		| Do you live with a partner?                              | Yes                      |
		| What is your partner's age?                              | 21 or over               |
	When I click the back link
	Then the page header is "What is your partner's age?"

Scenario: Back navigation from Does your partner have settled or pre-settled status under the EU Settlement Scheme?
	Given I answer the following pages:
		| Page                                                                                 | Answer                                               |
		| What is your age?                                                                    | Under 18                                             |
		| What is your nationality?                                                            | Citizen of a different country                       |
		| Are you in paid work?                                                                | No, I am not in work                                 |
		| Does your household receive universal credit?                                        | Yes                                                  |
		| Do you get any of these benefits?                                                    | Carer's Allowance                                    |
		| Do you already get any of these to help pay for childcare?                           | Childcare vouchers                                   |
		| How do you receive your childcare vouchers?                                          | A workplace nursery scheme                           |
		| Do you live with a partner?                                                          | Yes                                                  |
		| What is your partner's age?                                                          | 21 or over                                           |
		| Which of these best describes your partners nationality?                             | Citizen of an EU country, EEA country or Switzerland |
		| Does your partner have settled or pre-settled status under the EU Settlement Scheme? | Yes                                                  |
	When I click the back link
	Then the page header is "Does your partner have settled or pre-settled status under the EU Settlement Scheme?"
