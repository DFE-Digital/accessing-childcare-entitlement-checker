Feature: Session timeout

Background:
	Given I am on the childcare entitlement checker website
	And I click the link to start the journey
	And the browser clock is controlled

@javascript-enabled
Scenario: Dialog is not shown while the session is not close to expiry
	When I wait until the session timeout warning is almost due
	Then the session timeout dialog is not shown

@javascript-enabled
Scenario: Dialog is shown when the session is close to expiry
	When I wait until the session timeout warning is due
	Then the session timeout dialog is shown
	And the session timeout dialog says the answers will be deleted soon

@javascript-enabled
Scenario: Continuing closes the dialog and extends the session
	When I wait until the session timeout warning is due
	And I click Continue in the session timeout dialog
	Then the session timeout dialog is not shown
	When I wait until the session timeout warning is almost due
	Then the session timeout dialog is not shown

@javascript-enabled
Scenario: Pressing escape continues the session
	When I wait until the session timeout warning is due
	And I press the escape key
	Then the session timeout dialog is not shown

@javascript-enabled
Scenario: I am taken to the session expired page when the session times out
	When I wait until the session has timed out
	Then I am on the session expired page
