Feature: Has this child been born yet?

Background:
    Given I am on the childcare entitlement checker website
    And I start the journey and answer the questions as follows:
    | Question               | Answer            |
    | Where do you live?     | England           |
    | Add details of a child | Jack              |

Scenario: Page load
    Given the page header is "Has this child been born yet?"
    Then I should see 2 radio buttons with the following options:
    | Option   |
    | Yes      |
    | No       |

Scenario: Radio button selection
    When I select the "Yes" radio button
    And I select the "No" radio button
    Then the "No" radio button should be selected
    And all other options should be deselected 

Scenario: Continue without selection
    Given I have not selected an option
    When I click on Continue
    Then an error summary box should appear at the top of the page
    And the error summary title should be "There is a problem" with an error message "Select whether the child has been born"
    And inline validation should display with the error message "Select whether the child has been born"

Scenario: Continue with yes
    Given I have selected the "Yes" radio button
    When I click on Continue
    Then I will be directed to the next page in the user journey "What is Jack's date of birth?"

Scenario: Continue with no
    Given I have selected the "No" radio button
    When I click on Continue
    Then I will be directed to the next page in the user journey "What is this child's due date?"

Scenario: Back navigation
    When I click the back link
    Then I should be returned to the previous page in the user journey "Add details of a child"