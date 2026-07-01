Feature: What is your relationship to child?

Background:
	Given I am on the childcare entitlement checker website
	And I click the link to start the journey
	And I answer "Where do you live?" as "England"
	And I answer questions for "Sara" as follows:
		| Question                        | Answer    |
		| Add details about your children | Sara      |
		| Has this child been born yet?   | Yes       |
		| What is Sara's date of birth?   | Yesterday |

Scenario: Page load
	When the page header is "What is your relationship to Sara?"
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
	Then the page header is "What is your relationship to Sara?"
	And an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select your relationship to Sara"

Scenario: Continue with selection
	When I select the "Parent" radio button
	And I click on Continue
	Then the page header is "Does Sara get any of the following support?"

Scenario: Back navigation
	When I click the back link
	Then the page header is "What is Sara's date of birth?"