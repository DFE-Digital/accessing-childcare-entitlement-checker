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
	Then the "Country" error is "Select where you live"
	
Scenario: Partner page displays correctly
	Given I am on the has partner page
	Then the page header is "Do you live with a partner?"
	And I see "Yes"
	And I see "No"
    
 Scenario: If I don't select a partner response I get an error
    Given I am on the childcare entitlement checker website
    And I am on the has partner page
    When I click on Continue
    Then the "HasPartner" error is "Select do you live with a partner"
    

Scenario: I can select an option and continue
	Given I am on the has partner page
	When I select "Yes" for "HasPartner"
	And I click on Continue
	Then I see the text "Next step placeholder"
    
Scenario: I can navigate from the has partner page back to the where do you live page
	Given I am on the has partner page
	When I click the back link
	Then the page header is "Where do you live?"
	

    