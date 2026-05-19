Feature: What is this child's due date?

Background:
    Given I am on the childcare entitlement checker website
    And I start the journey and answer the questions as follows:
    | Question                      | Answer            |
    | Where do you live?            | England           |
    | Add details of a child        | Jack              |
    | Has this child been born yet? | No                |

Scenario: Page load
    Given the page header is "What is this child's due date?"
    Then I should see the hint text "For example, 30 9 2026"
    And I should see a date entry input

# The GDS component covers date validity, we do a single test to validate
# we've integrated it correctly.
Scenario: Enter an invalid date
    Given I enter the day "46" month "3" and year "2026"
    When I click on Continue
    Then an error summary box should appear at the top of the page
    # Update this error message with the GDS default
    And the error summary title should be "There is a problem" with an error message "The due date must be a real date"
    And inline validation should display with the error message "The due date must be a real date"

Scenario: Enter a past date
    Given I have entered yesterdays date
    When I click on Continue
    Then an error summary box should appear at the top of the page
    And the error summary title should be "There is a problem" with an error message "Enter a due date in the future"
    And inline validation should display with the error message "Enter a due date in the future"

Scenario: Continue without entering a date
    Given I have not entered a date
    When I click on Continue
    Then an error summary box should appear at the top of the page
    And the error summary title should be "There is a problem" with an error message "Enter this child's due date"
    And inline validation should display with the error message "Enter this child's due date"

Scenario: Continue with a future date
    Given I enter tomorrow's date
    When I click on Continue
    Then I will be directed to the next page in the user journey "What will your relationship be to this child?"

Scenario: Continue with todays date
    Given I have entered todays date
    When I click on Continue
    Then an error summary box should appear at the top of the page
    And the error summary title should be "There is a problem" with an error message "Enter a due date in the future"
    And inline validation should display with the error message "Enter a due date in the future"

Scenario: Back navigation
    When I click the back link
    Then I should be returned to the previous page in the user journey "Has this child been born yet?"