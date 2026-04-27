Feature: Child Date Of Birth

Background:
    Given I am on the 'What is Jack''s date of birth?' page

Scenario: Page load
    Then the page header is "What is Jack's date of birth?"
    And I see "For example, 31 3 2025"
    And I see "Day"
    And I see "Month"
    And I see "Year"
    And I see "Why we ask for your child's date of birth?"

Scenario: Continue without entering a date
    When I click on Continue
    Then an error summary box should appear at the top of the page
    And the error summary title should be "There is a problem" with an error message "Enter Jack's date of birth"
    And inline validation should display with the error message "Enter Jack's date of birth"
