Feature: How would you describe your partner's work status?

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
	When the page header is "How would you describe your partner's work status?"
	Then I should see 3 checkboxes with the following options:
		| Checkbox        |
		| Paid employment |
		| Self-employed   |
		| Apprentice      |
	And no checkboxes are selected

Scenario: Checkbox selection
	When I select the "Paid employment" checkbox
	And I select the "Self-employed" checkbox
	Then the following checkboxes should be selected:
		| Checkbox        |
		| Paid employment |
		| Self-employed   |

Scenario: Continue without selection
	When I do not select a checkbox
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select how you would describe your partner's work status"

Scenario: Continue with Self-employed
	When I select the "Self-employed" checkbox
	And I click on Continue
	Then the page header is "Has your partner been self-employed for less than 12 months?"

Scenario: Continue with Paid employment
	When I select the "Paid employment" checkbox
	And I click on Continue
	Then the page header is "On average, does your partner earn £203 a week or more before tax?"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Is your partner in paid work?"
