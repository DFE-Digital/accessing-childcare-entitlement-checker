Feature: Initial

Stub feature just to get the E2E tests set up.

Background:
	Given I am on the childcare entitlement checker website

Scenario: I can open the website
	Then the page header is "Check your childcare entitlement"

Scenario: I can click the start now button and still see the title
	When I click on Start Now
	Then the page header is "Check your childcare entitlement"