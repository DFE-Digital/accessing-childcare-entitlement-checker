Feature: Does child get any of the following support?

Background:
	Given I am on the childcare entitlement checker website
	And I click the link to start the journey
	And I answer "Where do you live?" as "England"
	And I answer questions for "Sara" as follows:
		| Question                           | Answer    |
		| Add details about your children    | Sara      |
		| Has this child been born yet?      | Yes       |
		| What is Sara's date of birth?      | Yesterday |

Scenario: Page load
	When the page header is "Does Sara get any of the following support?"
	Then I should see 6 checkboxes with the following options:
		| Checkbox                              |
		| Armed Forces Independence Payment     |
		| Certificate of visual impairment      |
		| Disability Living Allowance (DLA)     |
		| Education, health and care (EHC) plan |
		| Personal Independence Payment (PIP)   |
		| No, none of these apply               |
	And no checkboxes are selected

Scenario: Checkbox selection
	When I select the "Armed Forces Independence Payment" checkbox
	And I select the "Certificate of visual impairment" checkbox
	Then the following checkboxes should be selected:
		| Checkbox                          |
		| Armed Forces Independence Payment |
		| Certificate of visual impairment  |

Scenario: Continuing with none applicable while others are selected results in an error
	When I select the "Armed Forces Independence Payment" checkbox
	And I select the "Certificate of visual impairment" checkbox
	And I select the "No, none of these apply" checkbox
	And I click on Continue
	Then the page header is "Does Sara get any of the following support?"
	And an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select any support Sara gets, or select 'No, none of these apply'"

Scenario: Continue without selection
	When I do not select a checkbox
	And I click on Continue
	Then the page header is "Does Sara get any of the following support?"
	And an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select any support Sara gets, or select 'No, none of these apply'"

Scenario: Continue with selection
	When I select the "Armed Forces Independence Payment" checkbox
	And I select the "Certificate of visual impairment" checkbox
	And I click on Continue
	Then the page header is "Check your children's details"

Scenario: Continue with none applicable
	When I select the "No, none of these apply" checkbox
	And I click on Continue
	Then the page header is "Check your children's details"

Scenario: Back navigation
	When I click the back link
	Then the page header is "What is Sara's date of birth?"