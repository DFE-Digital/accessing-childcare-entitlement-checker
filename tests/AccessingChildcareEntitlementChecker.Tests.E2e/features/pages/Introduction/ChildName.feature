Feature: Add details of a child

Background:
    Given I am on the childcare entitlement checker website
    And I start the journey and answer the questions as follows:
    | Question           | Answer            |
    | Where do you live? | England           |

Scenario: Page load
    Given the page header is "Add details of a child"
    Then I should see a text box with the label "What name should we use for this child?"

# Assumes the field is required - i.e. we don't generate a name like `Child A` when the text box is empty.
Scenario: Continue with no name
    When I click on Continue
    Then an error summary box should appear at the top of the page
    And the error summary title should be "There is a problem" with an error message "Enter a name for the child"
    And inline validation should display with the error message "Enter a name for the child"

Scenario: Continue with name
    Given I have entered the text "Child A" into "What name should we use for this child?"
    When I click on Continue
    Then I will be directed to the next page in the user journey "Has this child been born yet?"

Scenario: Back navigation
    When I click the back link
    Then I should be returned to the previous page in the user journey "Where do you live?"