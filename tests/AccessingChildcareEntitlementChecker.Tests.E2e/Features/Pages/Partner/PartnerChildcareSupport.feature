Feature: Does your partner already get any of this childcare support?

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
	When the page header is "Does your partner already get any of this childcare support?"
	Then I should see 3 checkboxes with the following options:
		| Checkbox                                                    |
		| Childcare vouchers                                          |
		| A childcare bursary or grant (as part of education funding) |
		| No, they do not get any of this childcare support           |

Scenario: Checkbox selection
	When I select the "Childcare vouchers" checkbox
	And I select the "A childcare bursary or grant (as part of education funding)" checkbox
	Then the following checkboxes should be selected:
		| Checkbox                                                    |
		| Childcare vouchers                                          |
		| A childcare bursary or grant (as part of education funding) |

Scenario: Continue without selection
	When I do not select a checkbox
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select any of this childcare support your partner already gets, or select 'No, they do not get any of this childcare support'"

Scenario: Continue with Childcare vouchers
	When I select the "Childcare vouchers" checkbox
	And I click on Continue
	Then the page header is "How does your partner receive childcare vouchers?"

Scenario: Continue with A childcare bursary or grant (as part of education funding)
	When I select the "A childcare bursary or grant (as part of education funding)" checkbox
	And I click on Continue
	Then the page header is "Check your answers"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Does your partner get any of these benefits?"
