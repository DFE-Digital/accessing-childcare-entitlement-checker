Feature: Check your children's details (First)

Background:
    Given I am on the childcare entitlement checker website
    And I start the journey and answer the questions as follows:
    | Question                                      | Answer   |
    | Where do you live?                            | England  |
    | Add details of a child                        | Mia      |
    | Has this child been born yet?                 | No       |
    | What is this child's due date?                | Tomorrow |
    | What will your relationship be to this child? | Parent   |
    And I click the Add another child button and answer the questions as follows:
    | Question                                    | Answer                                |
    | Add details of a child                      | Jack                                  |
    | Has this child been born yet?               | Yes                                   |
    | What is Jack's date of birth?               | Yesterday                             |
    | What is your relationship to Jack?          | Parent                                |
    | Does Jack get any of the following support? | Education, health and care (EHC) plan |


Scenario: Page Load
    Then the page header is "Check your children's details"
    And I should see 2 summary panels
    And I should see a summary panel with the title "Mia" and the following summary:
    | Question                                      | Answer   |
    | What is this child's due date?                | Tomorrow |
    | What will your relationship be to this child? | Parent   |
    And I should see a summary panel with the title "Jack" and the following summary:
    | Question                                    | Answer                                |
    | What is Jack's date of birth?               | Yesterday                             |
    | What is your relationship to Jack?          | Parent                                |
    | Does Jack get any of the following support? | Education, health and care (EHC) plan |

Scenario: Change Mia's birth date
    When I click the Change link in the "Mia" panel for "What is this child's due date"
    Then I will be directed to the next page in the user journey "What is this child's due date?"

Scenario: Change my relationship to Jack
    When I click the Change link in the "Jack" panel for "What is your relationship to Jack?"
    And I answer the questions as follows:
    | Question                           | Answer |
    | What is your relationship to Jack? | Parent |
    Then the page header is "Check your children's details"
    And I should see 2 summary panels
    And I should see a summary panel with the title "Jack" and the following summary:
    | Question                                    | Answer                                |
    | What is Jack's date of birth?               | Yesterday                             |
    | What is your relationship to Jack?          | Parent                                |
    | Does Jack get any of the following support? | Education, health and care (EHC) plan |
   
Scenario: Continue with selection
    When I click on Continue
    Then I will be directed to the next page in the user journey "How old are you?"

Scenario: Back navigation
    When I click the back link
    Then I should be returned to the previous page in the user journey "Does Jack get any of the following support?"

Scenario: Back navigation to expected child
    # Easiest to just go round again first?
    And I click the Add another child button and answer the questions as follows:
    | Question                                      | Answer        |
    | Add details of a child                        | George        |
    | Has this child been born yet?                 | No            |
    | What is this child's due date?                | Tomorrow      |
    | What will your relationship be to this child? | Foster parent |
    When I click the back link
    Then I should be returned to the previous page in the user journey "What will your relationship be to this child?"