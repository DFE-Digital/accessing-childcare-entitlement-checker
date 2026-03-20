Feature: Initial

Background:
	Given I am on the childcare entitlement checker website

Scenario: I can start the journey
	Then the page header is "Check what help you could get with childcare costs"
	When I click the start button
	Then the page header is "Where do you live?"

Scenario: If I don't select a country I get an error
	When I click the start button
	And I click on Continue
	Then the country error is "Select where you live"