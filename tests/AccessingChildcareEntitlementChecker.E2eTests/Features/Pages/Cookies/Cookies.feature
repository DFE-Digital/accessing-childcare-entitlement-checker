Feature: Cookies

Background:
	Given I am on the childcare entitlement checker website
	And I click the link to start the journey

Scenario: Banner is not shown with no javascript
	Then the page header is "Where do you live?"
	And the cookie banner is not shown

@javascript-enabled
Scenario: Banner is shown with javascript enabled
	Then the page header is "Where do you live?"
	And the unselected cookie banner is shown

@javascript-enabled
Scenario: Banner is shown then hidden after accept, and I stay on the same page
	When I click the Accept button
	And the accepted cookie banner is shown
	And I click the Hide cookie message button
	Then the page header is "Where do you live?"
	And the cookie banner is not shown

@javascript-enabled
Scenario: Banner is shown then hidden after reject, and I stay on the same page
	When I click the Reject button
	And the rejected cookie banner is shown
	And I click the Hide cookie message button
	Then the page header is "Where do you live?"
	And the cookie banner is not shown

@javascript-enabled
Scenario: Banner is hidden on the next page
	When I click the Accept button
	And I click the Hide cookie message button
	And I answer "Where do you live?" as "England"
	Then the page header is "Add details about your children"
	And the cookie banner is not shown

@javascript-enabled
Scenario: Banner has a link to the cookies screen
	When I click the link to change my cookie preferences
	Then the page header is "Cookies"

Scenario: Cookies screen lets me change my preferences
	When I click the Cookies link in the footer
	And I select the "Yes" radio button
	And I click on Continue
	Then the page header is "Cookies"
	And I should see a success banner that says "You've set your cookie preferences."
	And the "Yes" radio button should be selected
