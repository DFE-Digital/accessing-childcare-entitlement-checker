Feature: ExpectedChildDetails ChildDueDate

Background:
    Given I am on the childcare entitlement checker website

Scenario: Page load
    Given the page header is "What is this child's due date?"

Scenario: Continue
    When I click on Continue
    Then I will be directed to the next page in the user journey "What will your relationship be to this child?"

Scenario: Back navigation
    Given I answered "Has this child been born yet?" with "No"
    When I click the back link
    Then I should be returned to the previous page in the user journey "Has this child been born yet?"
