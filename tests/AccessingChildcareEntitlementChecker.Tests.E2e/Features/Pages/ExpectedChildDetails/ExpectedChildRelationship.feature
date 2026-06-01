Feature: What will your relationship be to this child?

Background:
	Given I am on the childcare entitlement checker website
	And I click the Start now link
	And I answer "Where do you live?" as "England"
	And I answer questions for "Aydin" as follows:
		| Question                        | Answer   |
		| Add details about your children | Aydin    |
		| Has this child been born yet?   | No       |
		| What is this child's due date?  | Tomorrow |

Scenario: Page load
	When the page header is "What will your relationship be to this child?"
	Then I should see 3 radio buttons with the following options:
		| Option                               |
		| Parent                               |
		| Guardian or short-term respite carer |
		| Foster parent                        |

Scenario: Radio button selection
	When I select the "Parent" radio button
	And I select the "Guardian or short-term respite carer" radio button
	Then the "Guardian or short-term respite carer" radio button should be selected
	And all other options should be deselected

Scenario: Continue without selection
	When I do not select a radio button
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select what your relationship will be to this child"

Scenario: Continue with selection
	When I select the "Parent" radio button
	And I click on Continue
	Then the page header is "Check your children's details"

Scenario: Back navigation
	When I click the back link
	Then the page header is "What is this child's due date?"