Feature: Check your answers - journies

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I fill in my own details
	And I fill in my partner's details

# still fails because returnto is not passed backwards from the check your answers page.
@ignore
Scenario: Forward navigation after adding another child
	Given I click the Add another child button on the 'Check your answers' page
	And I answer questions for "Ringo" as follows:
		| Question                                      | Answer        |
		| Add details about your children               | Ringo         |
		| Has this child been born yet?                 | No            |
		| What is this child's due date?                | Tomorrow      |
		| What will your relationship be to this child? | Foster parent |
	When I click on Continue
	Then the page header is "Check your answers"

Scenario: Back navigation after adding another child
	Given I click the Add another child button on the 'Check your answers' page
	And I answer questions for "Ringo" as follows:
		| Question                                      | Answer        |
		| Add details about your children               | Ringo         |
		| Has this child been born yet?                 | No            |
		| What is this child's due date?                | Tomorrow      |
		| What will your relationship be to this child? | Foster parent |
	When I click the back link
	Then the page header is "What will your relationship be to this child?"

# broken waiting for partner journey to be refined per AC-810
@ignore 
Scenario: Partner journey is shown when I change my answer to say I have a partner
	Given I answer "Do you live with a partner?" as "No"
	And I do not see a summary list for "Your partners details"
	When I click the Change link in the "Your details" summary list for "Do you live with a partner?"
	And the page header is "Do you live with a partner?"
	And I select the "Yes" radio button
	And I click on Continue
	And I answer the questions as follows:
		| Question                                                     | Answer                     |
		| What is your partner's age?                                  | 21 or over                 |
		| Which of these best describes your partners nationality?     | British or Irish citizen   |
		| Is your partner in paid work?                                | No                         |
		| Does your partner get any of these benefits?                 | Carer's Allowance          |
		| Does your partner already get any of this childcare support? | Childcare vouchers         |
		| How does your partner receive childcare vouchers?            | A workplace nursery scheme |
	Then the page header is "Check your answers"
	And I should see a summary list for "Your partners details" with the following summary:
		| Question                                                     | Answer                     |
		| What is your partner's age?                                  | 21 or over                 |
		| Which of these best describes your partners nationality?     | British or Irish citizen   |
		| Is your partner in paid work?                                | No                         |
		| Does your partner get any of these benefits?                 | Carer's Allowance          |
		| Does your partner already get any of this childcare support? | Childcare vouchers         |
		| How does your partner receive childcare vouchers?            | A workplace nursery scheme |
