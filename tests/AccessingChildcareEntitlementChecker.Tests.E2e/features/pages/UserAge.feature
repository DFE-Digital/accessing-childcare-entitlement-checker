Feature: User Age

Background:
    Given I am on the childcare entitlement checker website
    And I start the journey, filling in Aydin's and Sara's details
    And I check my children's details and click on Continue

Scenario: Page load
    Given the page header is "What is your age?"
    Then I should see 3 radio buttons with the following options:
    | Option     |
    | Under 18   |
    | 18 to 20   |
    | 21 or over |

Scenario: Radio button selection
    When I select the "Under 18" radio button
    And I select the "18 to 20" radio button
    Then the "18 to 20" radio button should be selected
    And all other options should be deselected 

Scenario: Continue without selection
    Given I have not selected an option
    When I click on Continue
    Then an error summary box should appear at the top of the page 
    And the error summary title should be "There is a problem" with an error message "Select your age"
    And inline validation should display with the error message "Select your age"

Scenario: Continue with selection
    Given I have selected the "Under 18" radio button
    When I click on Continue
    Then I will be directed to the next page in the user journey "What is your nationality?"

Scenario: Back navigation
    When I click the back link
    Then I should be returned to the previous page in the user journey "Check your children's details"