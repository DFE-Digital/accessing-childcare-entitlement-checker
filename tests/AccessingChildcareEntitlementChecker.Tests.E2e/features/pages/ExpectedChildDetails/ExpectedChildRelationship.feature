Feature: What will your relationship be to this child?

Background:
    Given I am on the childcare entitlement checker website
    And I start the journey and answer the questions as follows:
    | Question                      | Answer            |
    | Where do you live?            | England           |
    | Add details of a child        | Jack              |
    | Has this child been born yet? | No                |
    | What is this child's due date? | Tomorrow          |

Scenario: Page load
    Given the page header is "What will your relationship be to this child?"
    Then I should see 3 radio buttons with the following options:
    | Option                               |
    | Parent                               |
    | Guardian or short-term respite carer |
    | Foster parent                        |

Scenario: Radio button selection
    When I select the "Parent" radio button
    And I select the "Guardian or short-term respite carer" radio button
    Then the "Guardian or short-term respite carer" radio button should be selected
    And all other options should be deselected 

Scenario: Continue without selection
    Given I have not selected an option
    When I click on Continue
    Then an error summary box should appear at the top of the page
    And the error summary title should be "There is a problem" with an error message "Select what your relationship will be to this child"
    And inline validation should display with the error message "Select what your relationship will be to this child"

Scenario: Continue with selection
    Given I have selected the "Parent" radio button
    When I click on Continue
    Then I will be directed to the next page in the user journey "Check your children's details"

Scenario: Back navigation
    When I click the back link
    Then I should be returned to the previous page in the user journey "What is this child's due date?"