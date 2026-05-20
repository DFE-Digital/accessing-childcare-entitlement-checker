Feature: User SettledStatus

Background:
    Given I am on the childcare entitlement checker website
    And I start the journey, filling in Aydin's and Sara's details
    And I check my children's details and click on Continue
    And I answer the questions as follows:
    | Question                  | Answer                                               |
    | What is your age?         | Under 18                                             |
    | What is your nationality? | Citizen of an EU country, EEA country or Switzerland |

Scenario: Page load
    Given the page header is "Do you have settled or pre-settled status under the EU Settlement Scheme?"
    Then I should see 3 radio buttons with the following options:
    | Option                                                           |
    | Yes                                                              |
    | No                                                               |
    | I applied before 1 July 2021 and am still waiting for a decision |

Scenario: Radio button selection
    When I select the "Yes" radio button
    And I select the "No" radio button
    Then the "No" radio button should be selected
    And all other options should be deselected

Scenario: Continue without selection
    Given I have not selected an option
    When I click on Continue
    Then an error summary box should appear at the top of the page
    And the error summary title should be "There is a problem" with an error message "Select your status"
    And inline validation should display with the error message "Select your status"

Scenario: Continue with Yes
    Given I have selected the "Yes" radio button
    When I click on Continue
    Then I will be directed to the next page in the user journey "Are you in paid work?"

Scenario: Back navigation from What is your nationality?
    When I click the back link
    Then I should be returned to the previous page in the user journey "What is your nationality?"
