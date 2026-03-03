Feature: Initial

Stub feature just to get the E2E tests set up.

Background:
	Given I am on the childcare entitlement checker website

Scenario: I can open the website
	Then the page header is "Where do you live?"

Scenario: If I don't select a country I get an error
	When I click on Continue
	Then the country error is "Please select where you live"