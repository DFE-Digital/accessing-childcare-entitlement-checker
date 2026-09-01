Feature: Start

Background:
	Given I am on the childcare entitlement checker website

Scenario: Page load
	When the page header is "Before you continue"
	Then I should see a navigation bar with the service name "Check if you are eligible for childcare funding"
	And the navigation bar service name should link to the start page
	And I should see a beta banner with the text "This is a new service. Help us improve it and give your feedback (opens in new tab)."
	And the beta banner should have a link to the qualtrix survey