Feature: Age

Scenario: Page load
    Given I have navigated to the page 
    When the page loads
    Then I should see the heading "How old are you", and three radio buttons with the following options: "Under 18yrs old", "18-20yrs old" and "21years old or over"

Scenario: Radio button selection
    Given the page has loaded 
    When I select an age range option
    Then only that option should be selected and any previously selected option should be deselected 

Scenario: Continue without selection
    Given I am on the "How old are you" page
    And I have not selected an option
    When I click continue
    Then I should remain on the same page
    And an error summary box should appear at the top of the page 
    And the error summary title should be “There is a problem”with an error message “Select your age”
    And inline validation should display with the error message “Select your age”

Scenario: Continue with selection
    Given I have selected an option
    When I click the continue button 
    Then I will be directed to the next page in the user journey (How old is your partner)

Scenario: Back navigation
    Given I am on the "How old are you" page
    When I click the back link
    Then I should be returned to the previous page in the user journey (children's details)