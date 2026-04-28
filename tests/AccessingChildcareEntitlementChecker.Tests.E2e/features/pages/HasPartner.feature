Feature: Has Partner

Background:
    Given I am on the 'Do you live with a partner?' page

Scenario: Page load
    Then the page header is "Do you live with a partner?"
    And I should see 2 radio buttons with the following options:
    | Option               |
    | Yes                  |
    | No                   |

Scenario: Radio button selection
    When I select the "No" radio button
    And I select the "Yes" radio button
    Then the "Yes" radio button should be selected
    And all other options should be deselected 

Scenario: Continue without selection
    Given I have not selected an option
    When I click on Continue
    Then an error summary box should appear at the top of the page 
    And the error summary title should be "There is a problem" with an error message "Select do you live with a partner"
    And inline validation should display with the error message "Select do you live with a partner"

Scenario: Continue with selection
    Given I have selected the "Yes" radio button
    When I click on Continue
    Then I will be directed to the next page in the user journey "How old is your partner?"

Scenario: Back navigation
    When I click the back link
    Then I should be returned to the previous page in the user journey "Where do you live?"