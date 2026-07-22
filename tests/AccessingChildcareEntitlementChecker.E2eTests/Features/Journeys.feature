Feature: Check your answers - journies

Background:
	Given I am on the childcare entitlement checker website

Scenario: Forward navigation after adding another child
	Given I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I fill in my own details
	And I fill in my partner's details
	And I click the Add another child button
	And I answer questions as follows:
		| Question                                      | Answer        |
		| Add details about your children               | Ringo         |
		| Has this child been born yet?                 | No            |
		| What is this child's due date?                | Tomorrow      |
	Then the page header is "Check your answers"

Scenario: Back navigation after adding another child
	Given I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I fill in my own details
	And I fill in my partner's details
	And I click the Add another child button
	And I answer questions as follows:
		| Question                                      | Answer        |
		| Add details about your children               | Ringo         |
		| Has this child been born yet?                 | No            |
		| What is this child's due date?                | Tomorrow      |
	When I click the back link
	# Should potentially be "Then the page header is "What is this child's due date?"
	Then the page header is "How does your partner receive childcare vouchers?"

Scenario: Partner journey is shown when I change my answer to say I have a partner
	Given I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I fill in my own details
	And I answer "Do you live with a partner?" as "No"	
	And I do not see a summary list for "Your partners details"
	When I click the Change link in the "Your details" summary list for "Do you live with a partner?"
	And the page header is "Do you live with a partner?"
	And I select the "Yes" radio button
	And I click on Continue
	And I answer the questions as follows:
		| Question                                                              | Answer                     |
		| What is your partner's age?                                           | 21 or over                 |
		| Is your partner in paid work?                                         | No, they are not in work   |
		| Does your partner get any of these benefits?                          | Carer's Allowance          |
		| Does your partner already get any of these to help pay for childcare? | Childcare vouchers         |
		| How does your partner receive childcare vouchers?                     | A workplace nursery scheme |
	Then the page header is "Check your answers"
	And I should see a summary list for "Your partners details" with the following summary:
		| Question                                                              | Answer                     |
		| What is your partner's age?                                           | 21 or over                 |
		| Is your partner in paid work?                                         | No, they are not in work   |
		| Does your partner get any of these benefits?                          | Carer's Allowance          |
		| Does your partner already get any of these to help pay for childcare? | Childcare vouchers         |
		| How does your partner receive childcare vouchers?                     | A workplace nursery scheme |

@ignore
Scenario: Back during partner journey checks for valid journey
	Given I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I fill in my own details
	And I answer "Do you live with a partner?" as "No"	
	And I do not see a summary list for "Your partners details"
	When I click the Change link in the "Your details" summary list for "Do you live with a partner?"
	And the page header is "Do you live with a partner?"
	And I select the "Yes" radio button
	And I click on Continue
	And I answer the questions as follows:
		| Question                      | Answer                   |
		| What is your partner's age?   | 21 or over               |
		| Is your partner in paid work? | No, they are not in work |
	And I click the back link
	Then the page header is "Check your answers"
	And I should see a summary list for "Your partners details" with the following summary:
		| Question                      | Answer                   |
		| What is your partner's age?   | 21 or over               |
		| Is your partner in paid work? | No, they are not in work |
		# This is now in an invalid state - on the summary screen with no answers for the required partner qs