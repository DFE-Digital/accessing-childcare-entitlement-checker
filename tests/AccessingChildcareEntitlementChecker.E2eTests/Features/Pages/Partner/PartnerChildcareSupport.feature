Feature: Does your partner already get any of these to help pay for childcare?

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I fill in my own details
	And I answer questions as follows:
		| Question                                     | Answer                   |
		| Do you live with a partner?                  | Yes                      |
		| What is your partner's age?                  | 21 or over               |
		| Is your partner in paid work?                | No, they are not in work |
		| Does your partner get any of these benefits? | Carer's Allowance        |

Scenario: Page load
	When the page header is "Does your partner already get any of these to help pay for childcare?"
	Then I should see the following checkboxes:
		| Name                             | Hint                                                                                                                 |
		| Childcare vouchers               | A scheme that lets you pay for childcare from your salary before tax, which closed to new applicants in October 2018 |
		| A childcare bursary or grant     | Money to help pay for childcare while you study, for example through a college or university                         |
		| No, they do not get any of these |                                                                                                                      |
	And no checkboxes are selected

Scenario: Checkbox selection
	When I select the "Childcare vouchers" checkbox
	And I select the "A childcare bursary or grant" checkbox
	Then the following checkboxes should be selected:
		| Checkbox                     |
		| Childcare vouchers           |
		| A childcare bursary or grant |

@withJavascript
Scenario: None selection is exclusive with JavaScript enabled
	When I select the "Childcare vouchers" checkbox
	And I select the "A childcare bursary or grant" checkbox
	And I select the "No, I do not get any of these" checkbox
	Then the following checkboxes should be selected:
		| Checkbox                      |
		| No, I do not get any of these |

Scenario: None selection is validated without Javascript
	When I select the "Childcare vouchers" checkbox
	And I select the "A childcare bursary or grant" checkbox
	And I select the "No, I do not get any of these" checkbox
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select any of this childcare support you already get, or select 'No, I do not get any of these'"

Scenario: Continue without selection
	When I do not select a checkbox
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select any of this childcare support your partner already gets, or select 'No, they do not get any of these'"

Scenario: Continue with Childcare vouchers
	When I select the "Childcare vouchers" checkbox
	And I click on Continue
	Then the page header is "How does your partner receive childcare vouchers?"

Scenario: Continue with A childcare bursary or grant
	When I select the "A childcare bursary or grant" checkbox
	And I click on Continue
	Then the page header is "Check your answers"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Does your partner get any of these benefits?"
