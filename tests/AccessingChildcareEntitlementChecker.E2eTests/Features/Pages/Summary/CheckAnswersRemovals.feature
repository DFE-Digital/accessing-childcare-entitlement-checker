Feature: Check your answers - removal

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I fill in my own details
	And I fill in my partner's details

Scenario: Cancel a removal
	When I click the Remove link in the "Aydin" card
	And the page header is "Are you sure you want to remove Aydin?"
	And I should see 2 radio buttons with the following options:
		| Option |
		| Yes    |
		| No     |
	And I select the "No" radio button
	And I click on Continue
	Then the page header is "Check your answers"
	And I should see 2 summary cards

Scenario: Back navigation
	When I click the Remove link in the "Aydin" card
	And the page header is "Are you sure you want to remove Aydin?"
	And I click the back link
	Then the page header is "Check your answers"
	And I should see 2 summary cards

Scenario: Remove Aydin
	When I click the Remove link in the "Aydin" card
	And the page header is "Are you sure you want to remove Aydin?"
	And I should see 2 radio buttons with the following options:
		| Option |
		| Yes    |
		| No     |
	And I select the "Yes" radio button
	And I click on Continue
	Then the page header is "Check your answers"
	And I should see a success banner that says "Aydin has been removed"
	And I should see one summary card
	And I should see a summary card with the title "Sara" and the following summary:
		| Question                                    | Answer                                |
		| What is Sara's date of birth?               | Yesterday                             |
		| Does Sara get any of the following support? | Education, health and care (EHC) plan |

Scenario: Remove both children
	When I remove "Aydin"
	And the page header is "Check your answers"
	And I remove "Sara"
	And the page header is "Check your answers"
	Then I should see some text saying "You must add a child to continue."
	And I should not see a Continue button

Scenario: Remove both children and add another one
	Given I remove "Aydin"
	And I remove "Sara"
	When I click the Add another child button
	And I answer questions as follows:
		| Question                                      | Answer        |
		| Add details about your children               | Ringo         |
		| Has this child been born yet?                 | No            |
		| What is this child's due date?                | Tomorrow      |
	Then I should see one summary card
	And I should see a summary card with the title "Ringo" and the following summary:
		| Question                       | Answer   |
		| What is this child's due date? | Tomorrow |