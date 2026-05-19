Feature: What is your relationship to child?

Background:
    Given I am on the childcare entitlement checker website
    And I start the journey and answer the questions as follows:
    | Question                      | Answer    |
    | Where do you live?            | England   |
    | Add details of a child        | Jack      |
    | Has this child been born yet? | Yes       |
    | What is Jack's date of birth? | Yesterday |

Scenario: Page load
    Given the page header is "What is your relationship to Jack?"
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
    And the error summary title should be "There is a problem" with an error message "Select your relationship to Jack"
    And inline validation should display with the error message "Select your relationship to Jack"

Scenario: Continue with selection
    Given I have selected the "Parent" radio button
    When I click on Continue
    Then I will be directed to the next page in the user journey "Does Jack get any of the following support?"

Scenario: Back navigation
    When I click the back link
    Then I should be returned to the previous page in the user journey "What is Jack's date of birth?"