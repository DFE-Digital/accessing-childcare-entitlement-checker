Feature: Cookies

Background:
	Given I am on the childcare entitlement checker website
	And I click the link to start the journey

Scenario: Banner is shown
	Then the cookie banner is shown

Scenario: Banner is shown then hidden after accept
	When I click the Accept button
	Then the cookie banner is not shown

Scenario: Banner is shown then hidden after reject
	When I click the Reject button
	Then the cookie banner is not shown

Scenario: Banner is hidden on the next page
	When I click the Accept button
	And I answer "Where do you live?" as "England"
	Then the page header is "Add details about your children"
	And the cookie banner is not shown

Scenario: Cookies screen lets me change my preferences
	When I click the link to change my cookie preferences
	And I select the "Yes" radio button
	And I click on Continue
	Then the page header is "Cookies"
	And I should see a success banner that says "You've set your cookie preferences."
	And the "Yes" radio button should be selected
