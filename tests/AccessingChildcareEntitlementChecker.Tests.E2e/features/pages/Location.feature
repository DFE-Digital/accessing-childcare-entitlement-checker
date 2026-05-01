Feature: Location

Background:
    Given I am on the 'Where do you live?' page

Scenario: Page load
    Given the page header is "Where do you live?"
    Then I should see 4 radio buttons with the following options:
    | Option               |
    | England              |
    | Scotland             |
    | Wales                |
    | Northern Ireland     |

Scenario: Radio button selection
    When I select the "Scotland" radio button
    And I select the "England" radio button
    Then the "England" radio button should be selected
    And all other options should be deselected 

Scenario: Continue without selection
    Given I have not selected an option
    When I click on Continue
    Then an error summary box should appear at the top of the page 
    And the error summary title should be "There is a problem" with an error message "Select where you live"
    And inline validation should display with the error message "Select where you live"

Scenario: Continue with selection
    Given I have selected the "England" radio button
    When I click on Continue
    Then I will be directed to the next page in the user journey "Add details of a child"

Scenario: Back navigation
    When I click the back link
    Then I should be returned to the previous page in the user journey "Check what help you could get with childcare costs"