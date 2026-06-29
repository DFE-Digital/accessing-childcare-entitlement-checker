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
		| What will your relationship be to this child? | Parent   |
	And I should see a summary card with the title "Sara" and the following summary:
		| Question                                    | Answer                                |
		| What is Sara's date of birth?               | Yesterday                             |
		| What is your relationship to Sara?          | Parent                                |
		| Does Sara get any of the following support? | Education, health and care (EHC) plan |
	And I should see a summary list for "Your details" with the following summary:
		| Question                                                | Answer                     |
		| Where do you live?                                      | England                    |
		| What is your age?                                       | Under 18                   |
		| What is your nationality?                               | British or Irish citizen   |
		| Are you in paid work?                                   | Yes                        |
		| How would you describe your work status?                | Self-employed              |
		| Have you been self-employed for less than 12 months?    | No                         |
		| On average, do you earn £203 a week or more before tax? | Yes                        |
		| Is your adjusted net income more than £100,000 a year?  | No                         |
		| Does your household receive universal credit?           | Yes                        |
		| Do you get any of these benefits?                       | Carer's Allowance          |
		| Do you already get any of this childcare support?       | Childcare vouchers         |
		| How do you receive your childcare vouchers?             | A workplace nursery scheme |
		| Do you live with a partner?                             | Yes                        |
	And I should see a summary list for "Your partners details" with the following summary:
		| Question                                                     | Answer                     |
		| What is your partner's age?                                  | 21 or over                 |
		| Is your partner in paid work?                                | No, they are not in work   |
		| Does your partner get any of these benefits?                 | Carer's Allowance          |
		| Does your partner already get any of this childcare support? | Childcare vouchers         |
		| How does your partner receive childcare vouchers?            | A workplace nursery scheme |

Scenario: Change Aydin's due date
	When I click the Change link in the "Aydin" card for "What is this child's due date"
	Then I will be directed to the next page in the user journey "What is this child's due date?"

Scenario: Continue with selection
	When I click on Continue
	Then the page header is "Childcare support you could get"

Scenario: Back navigation
	When I click the back link
	Then the page header is "How does your partner receive childcare vouchers?"

Scenario: Change my relationship to Sara
	When I click the Change link in the "Sara" card for "What is your relationship to Sara?"
	And I answer the questions as follows:
		| Question                           | Answer |
		| What is your relationship to Sara? | Parent |
	Then the page header is "Check your answers"
	And I should see 2 summary cards
	And I should see a summary card with the title "Sara" and the following summary:
		| Question                                    | Answer                                |
		| What is Sara's date of birth?               | Yesterday                             |
		| What is your relationship to Sara?          | Parent                                |
		| Does Sara get any of the following support? | Education, health and care (EHC) plan |

Scenario: Partner details are not shown when I don't have a partner
	Given I answer "Do you live with a partner?" as "No"
	Then I do not see a summary list for "Your partners details"

Scenario: Self employed details are not shown when I am not self employed
	Given I answer "How would you describe your work status?" as "Paid employment"
	Then I do not see a summary row "Have you been self-employed for less than 12 months"

Scenario: Self employed details are not shown when I change my answer
	When I click the Change link in the "Your details" summary list for "How would you describe your work status?"
	And the page header is "How would you describe your work status?"
	And I select the "Paid employment" checkbox
	And I deselect the "Self-employed" checkbox
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "Have you been self-employed for less than 12 months"

Scenario: Back navigation to Does your partner already get any of this childcare support?
	Given I answer "Does your partner already get any of this childcare support?" as "No, they do not get any of this childcare support"
	When I click the back link
	Then the page header is "Does your partner already get any of this childcare support?"

Scenario: Back navigation to Do you live with a partner?
	Given I answer "Do you live with a partner?" as "No"
	When I click the back link
	Then the page header is "Do you live with a partner?"
