Feature: Check your children's details - removal

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details

Scenario: Cancel a removal
	When I click the Remove link in the "Aydin" panel
	And the page header is "Are you sure you want to remove Aydin?"
	And I should see 2 radio buttons with the following options:
		| Option |
		| Yes    |
		| No     |
	And I select the "No" radio button
	And I click on Continue
	Then the page header is "Check your children's details"
	And I should see 2 summary panels

Scenario: Back navigation
	When I click the Remove link in the "Aydin" panel
	And the page header is "Are you sure you want to remove Aydin?"
	And I click the back link
	Then the page header is "Check your children's details"
	And I should see 2 summary panels

Scenario: Remove Aydin
	When I click the Remove link in the "Aydin" panel
	And the page header is "Are you sure you want to remove Aydin?"
	And I should see 2 radio buttons with the following options:
		| Option |
		| Yes    |
		| No     |
	And I select the "Yes" radio button
	And I click on Continue
	Then the page header is "Check your children's details"
	And I should see a success banner that says "Aydin has been removed"
	And I should see one summary panel
	And I should see a summary panel with the title "Sara" and the following summary:
		| Question                                    | Answer                                |
		| What is Sara's date of birth?               | Yesterday                             |
		| What is your relationship to Sara?          | Parent                                |
		| Does Sara get any of the following support? | Education, health and care (EHC) plan |

Scenario: Remove both children
	When I remove "Aydin"
	And I remove "Sara"
	Then I should see some text saying "You must add a child to continue."
	And I should not see a Continue button