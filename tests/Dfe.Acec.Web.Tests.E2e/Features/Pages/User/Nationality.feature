Feature: User Nationality

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I answer questions as follows:
		| Question          | Answer   |
		| What is your age? | Under 18 |

Scenario: Page load
	When the page header is "What is your nationality?"
	Then I should see the following checkboxes:
		| Option                                               |
		| British or Irish citizen                             |
		| Citizen of an EU country, EEA country or Switzerland |
		| Citizen of a different country                       |
	And no checkboxes are selected

Scenario: Checkbox selection
	When I select the "British or Irish citizen" checkbox
	And I select the "Citizen of an EU country, EEA country or Switzerland" checkbox
	Then the following checkboxes should be selected:
		| Checkbox                                             |
		| British or Irish citizen                             |
		| Citizen of an EU country, EEA country or Switzerland |

Scenario: Continue without selection
	When I do not select a checkbox
	And I click on Continue
	Then an error summary box should appear at the top of the page
	And the error summary and inline validation should be "Select your nationality"

Scenario: Continue with Citizen of an EU country, EEA country or Switzerland
	When I select the "Citizen of an EU country, EEA country or Switzerland" checkbox
	And I click on Continue
	Then the page header is "Do you have settled or pre-settled status under the EU Settlement Scheme?"

Scenario: Continue with British or Irish citizen
	When I select the "British or Irish citizen" checkbox
	And I click on Continue
	Then the page header is "Are you in paid work?"

Scenario: Back navigation
	When I click the back link
	Then the page header is "What is your age?"
