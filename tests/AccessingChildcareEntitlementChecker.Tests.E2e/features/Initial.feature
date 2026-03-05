Feature: Initial

Stub feature just to get the E2E tests set up.

Background:
	Given I am on the childcare entitlement checker website

Scenario: I can open the website
<<<<<<< HEAD
	Then the page header is "Check your childcare entitlement"

Scenario: I can click the start now button and still see the title
	When I click on Start Now
	Then the page header is "Check your childcare entitlement"
=======
	Then the page header is "Where do you live?"

Scenario: If I don't select a country I get an error
	When I click on Continue
	Then the country error is "Please select where you live"
>>>>>>> main
