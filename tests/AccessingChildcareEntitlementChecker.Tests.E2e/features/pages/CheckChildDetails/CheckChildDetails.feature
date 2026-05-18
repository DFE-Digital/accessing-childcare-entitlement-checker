Feature: CheckChildDetails CheckChildDetails

Background:
    Given I am on the childcare entitlement checker website

Scenario: Page load
    Then the page header is "Check your children's details"

Scenario: Continue with children
    When I click on Continue
    Then I will be directed to the next page in the user journey "What is your age?"

Scenario: Delete a child
    When I click the Delete link in the "Child A" panel
    Then the page header is "Are you sure you want to delete Child A?"

Scenario: Deleted all children
    Then I should see some text saying "You must add a child to continue."
