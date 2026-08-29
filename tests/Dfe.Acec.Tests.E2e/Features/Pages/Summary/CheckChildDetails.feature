Feature: Check your children's details

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details

Scenario: Page Load
	Then the page header is "Check your children's details"
	And I should see 2 summary cards
	And I should see a summary card with the title "Aydin" and the following summary:
		| Question                                      | Answer   |
		| What is this child's due date?                | Tomorrow |
	And I should see a summary card with the title "Sara" and the following summary:
		| Question                                    | Answer                                |
		| What is Sara's date of birth?               | Yesterday                             |
		| Does Sara get any of the following support? | Education, health and care (EHC) plan |

Scenario: Change Aydin's due date
	When I click the Change link in the "Aydin" card for "What is this child's due date"
	Then I will be directed to the next page in the user journey "What is this child's due date?"

Scenario: Change whether Sara gets any support
	When I click the Change link in the "Sara" card for "Does Sara get any of the following support?"
	And I answer the questions as follows:
		| Question                                    | Answer                           |
		| Does Sara get any of the following support? | Certificate of visual impairment |
	Then the page header is "Check your children's details"
	And I should see 2 summary cards
	And I should see a summary card with the title "Sara" and the following summary:
		| Question                                    | Answer                                                                  |
		| What is Sara's date of birth?               | Yesterday                                                               |
		| Does Sara get any of the following support? | Certificate of visual impairment, Education, health and care (EHC) plan |
   
Scenario: Continue with selection
	When I click on Continue
	Then the page header is "What is your age?"

Scenario: Back navigation
	When I click the back link
	Then the page header is "Does Sara get any of the following support?"

Scenario: Back navigation to expected child
	And I click the Add another child button
	And I answer questions as follows:
		| Question                                      | Answer        |
		| Add details about your children               | Ringo         |
		| Has this child been born yet?                 | No            |
		| What is this child's due date?                | Tomorrow      |
	When I click the back link
	Then the page header is "What is this child's due date?"