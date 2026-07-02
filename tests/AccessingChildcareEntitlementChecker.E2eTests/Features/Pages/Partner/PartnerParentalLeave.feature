Feature: Which child is your partner on leave for?

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I answer questions as follows:
		| Question                                                   | Answer                                 |
		| What is your age?                                          | Under 18                               |
		| What is your nationality?                                  | British or Irish citizen               |
		| Are you in paid work?                                      | Yes, but I am on parental leave        |
		| Which child are you on leave for?                          | Sara                                   |
		| How would you describe your work status?                   | Paid employment                        |
		| On average, do you earn £203 a week or more before tax?    | No                                     |
		| Does your household receive universal credit?              | No                                     |
		| Do you get any of these benefits?                          | No, I do not get any of these benefits |
		| Do you already get any of these to help pay for childcare? | No, I do not get any of these          |
		| Do you live with a partner?                                | Yes                                    |
		| What is your partner's age?                                | 21 or over                             |
		| Is your partner in paid work?                              | Yes, but they are on parental leave    |

Scenario: Page load
	When the page header is "Which child is your partner on leave for?"
	Then I should see 3 checkboxes with the following options:
		| Option                 |
		| Sara                   |
		| Aydin                  |
		| None of these children |
	And no checkboxes are selected

Scenario: Checkbox selection
	When I select the "Sara" checkbox
	And I select the "Aydin" checkbox
	Then the following checkboxes should be selected:
		| Checkbox |
		| Sara     |
		| Aydin    |

Scenario: Continue without selection
	When I do not select a checkbox
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select which child your partner is on leave for, or 'None of these children'"

Scenario: Continue
	When I select the "Sara" checkbox
	And I click on Continue
	Then the page header is "How would you describe your partner's work status?"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Is your partner in paid work?"