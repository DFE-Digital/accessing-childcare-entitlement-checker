Feature: Check your answers

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I fill in my own details
	And I fill in my partner's details

Scenario: Page Load
	Then the page header is "Check your answers"
	And I should see 2 summary cards
	And I should see a summary card with the title "Aydin" and the following summary:
		| Question                                      | Answer   |
		| What is this child's due date?                | Tomorrow |
	And I should see a summary card with the title "Sara" and the following summary:
		| Question                                    | Answer                                |
		| What is Sara's date of birth?               | Yesterday                             |
		| Does Sara get any of the following support? | Education, health and care (EHC) plan |
	And I should see a summary list for "Your details" with the following summary:
		| Question                                                                                  | Answer                     |
		| Where do you live?                                                                        | England                    |
		| What is your age?                                                                         | Under 18                   |
		| What is your nationality?                                                                 | British or Irish citizen   |
		| Are you in paid work?                                                                     | Yes                        |
		| How would you describe your work status?                                                  | Self-employed              |
		| Have you been self-employed for less than 12 months?                                      | No                         |
		| On average, do you expect to earn £128 a week or more before tax?                         | Yes                        |
		| Do you expect your adjusted net income to be more than £100,000 for the current tax year? | No                         |
		| Does your household receive universal credit?                                             | Yes                        |
		| Do you get any of these benefits?                                                         | Carer's Allowance          |
		| Do you already get any of these to help pay for childcare?                                | Childcare vouchers         |
		| How do you receive your childcare vouchers?                                               | A workplace nursery scheme |
		| Do you live with a partner?                                                               | Yes                        |
	And I should see a summary list for "Your partners details" with the following summary:
		| Question                                                              | Answer                     |
		| What is your partner's age?                                           | 21 or over                 |
		| Is your partner in paid work?                                         | No, they are not in work   |
		| Does your partner get any of these benefits?                          | Carer's Allowance          |
		| Does your partner already get any of these to help pay for childcare? | Childcare vouchers         |
		| How does your partner receive childcare vouchers?                     | A workplace nursery scheme |

Scenario: Change Aydin's due date
	When I click the Change link in the "Aydin" card for "What is this child's due date"
	Then I will be directed to the next page in the user journey "What is this child's due date?"

Scenario: Continue with selection
	When I click on Continue
	Then the page header is "Childcare support you could get"

Scenario: Back navigation
	When I click the back link
	Then the page header is "How does your partner receive childcare vouchers?"

Scenario: Change whether Sara gets any support
	When I click the Change link in the "Sara" card for "Does Sara get any of the following support?"
	And I answer the questions as follows:
		| Question                                    | Answer                           |
		| Does Sara get any of the following support? | Certificate of visual impairment |
	Then the page header is "Check your answers"
	And I should see 2 summary cards
	And I should see a summary card with the title "Sara" and the following summary:
		| Question                                    | Answer                                                                  |
		| What is Sara's date of birth?               | Yesterday                                                               |
		| Does Sara get any of the following support? | Certificate of visual impairment, Education, health and care (EHC) plan |

Scenario: Change my age
	When I click the Change link in the "Your details" summary list for "What is your age?"
	And I answer the questions as follows:
		| Question                                                                                  | Answer   |
		| What is your age?                                                                         | 18 to 20 |
		| On average, do you expect to earn £173 a week or more before tax?                         | Yes      |
		| Do you expect your adjusted net income to be more than £100,000 for the current tax year? | No       |
	Then the page header is "Check your answers"
	And I should see a summary list for "Your details" with the following summary:
		| Question                                                                                  | Answer   |
		| What is your age?                                                                         | 18 to 20 |
		| On average, do you expect to earn £173 a week or more before tax?                         | Yes      |
		| Do you expect your adjusted net income to be more than £100,000 for the current tax year? | No       |

Scenario: Change my partner's employment status and age to trigger a different threshold
	When I click the Change link in the "Your partners details" summary list for "Is your partner in paid work?"
	And I answer the questions as follows:
		| Question                                                                                              | Answer          |
		| Is your partner in paid work?                                                                         | Yes             |
		| How would you describe your partner's work status?                                                    | Paid employment |
		| On average, does your partner expect to earn £208 a week or more before tax?                          | Yes             |
		| Does your partner expect their adjusted net income to be more than £100,000 for the current tax year? | No              |
	And I click the Change link in the "Your partners details" summary list for "What is your partner's age?"
	And I answer the questions as follows:
		| Question                                                                                              | Answer   |
		| What is your partner's age?                                                                           | 18 to 20 |
		| On average, does your partner expect to earn £173 a week or more before tax?                          | Yes      |
		| Does your partner expect their adjusted net income to be more than £100,000 for the current tax year? | No       |
	Then the page header is "Check your answers"
	And I should see a summary list for "Your partners details" with the following summary:
		| Question                                                                                              | Answer   |
		| What is your partner's age?                                                                           | 18 to 20 |
		| On average, does your partner expect to earn £173 a week or more before tax?                          | Yes      |
		| Does your partner expect their adjusted net income to be more than £100,000 for the current tax year? | No       |

Scenario: Change my weekly earnings
	When I click the Change link in the "Your details" summary list for "On average, do you expect to earn £128 a week or more before tax?"
	And I answer the questions as follows:
		| Question                                                          | Answer |
		| On average, do you expect to earn £128 a week or more before tax? | No     |
	Then the page header is "Check your answers"
	And I should see a summary list for "Your details" with the following summary:
		| Question                                                | Answer |
		| On average, do you expect to earn £128 a week or more before tax? | No     |
	And I do not see a summary row "Do you expect your adjusted net income to be more than £100,000 for the current tax year?"

Scenario: Partner details are not shown when I don't have a partner
	When I click the Change link in the "Your details" summary list for "Do you live with a partner?"
	And I select the "No" radio button
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary list for "Your partners details"

Scenario: Self employed details are not shown when I change my answer
	When I click the Change link in the "Your details" summary list for "How would you describe your work status?"
	And the page header is "How would you describe your work status?"
	And I select the "Paid employment" checkbox
	And I deselect the "Self-employed" checkbox
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "Have you been self-employed for less than 12 months"

Scenario: Back navigation to Does your partner already get any of these to help pay for childcare?
	When I click the Change link in the "Your partners details" summary list for "Does your partner already get any of these to help pay for childcare?"
	And I deselect the "Childcare vouchers" checkbox
	And I select the "No, they do not get any of these" checkbox
	And I click on Continue
	And the page header is "Check your answers"
	When I click the back link
	Then the page header is "Does your partner already get any of these to help pay for childcare?"

Scenario: Back navigation to Do you live with a partner?
	When I click the Change link in the "Your details" summary list for "Do you live with a partner?"
	And I select the "No" radio button
	And I click on Continue
	And the page header is "Check your answers"
	When I click the back link
	Then the page header is "Do you live with a partner?"
