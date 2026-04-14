Feature: Age

Background:
    Given I am on the 'How old are you?' page

Scenario: Page load
    Given the page header is "How old are you?"
    Then I should see three radio buttons with the following options:
    | Option               |
    | Under 18 years old   |
    | 18 to 20 years old   |
    | 21 years old or over |

Scenario: Radio button selection
    When I select the "Under 18 years old" radio button
    And I select the "18 to 20 years old" radio button
    Then the "18 to 20 years old" radio button should be selected
    And all other options should be deselected 

Scenario: Continue without selection
    Given I have not selected an option
    When I click on Continue
    Then an error summary box should appear at the top of the page 
    And the error summary title should be "There is a problem" with an error message "Select your age"
    And inline validation should display with the error message "Select your age"

@ignore
Scenario: Continue with selection
    Given I have selected the "Under 18 years old" radio button
    When I click on Continue
    Then I will be directed to the next page in the user journey "How old is your partner"

@ignore
Scenario: Back navigation
    When I click the back link
    Then I should be returned to the previous page in the user journey "Children's Details"