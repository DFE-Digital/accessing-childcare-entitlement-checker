Feature: User Nationality

Background:
    Given I am on the childcare entitlement checker website
    And I start the journey, filling in Aydin's and Sara's details
    And I check my children's details and click on Continue
    And I answer the questions as follows:
    | Question          | Answer   |
    | What is your age? | Under 18 |

Scenario: Page load
    Given the page header is "What is your nationality?"
    Then I should see 3 radio buttons with the following options:
    | Option                                               |
    | British or Irish citizen                             |
    | Citizen of an EU country, EEA country or Switzerland |
    | Citizen of a different country                       |

Scenario: Radio button selection
    When I select the "British or Irish citizen" radio button
    And I select the "Citizen of an EU country, EEA country or Switzerland" radio button
    Then the "Citizen of an EU country, EEA country or Switzerland" radio button should be selected
    And all other options should be deselected

Scenario: Continue without selection
    Given I have not selected an option
    When I click on Continue
    Then an error summary box should appear at the top of the page
    And the error summary title should be "There is a problem" with an error message "Select your nationality"
    And inline validation should display with the error message "Select your nationality"

Scenario: Continue with Citizen of an EU country, EEA country or Switzerland
    Given I have selected the "Citizen of an EU country, EEA country or Switzerland" radio button
    When I click on Continue
    Then I will be directed to the next page in the user journey "Do you have settled or pre-settled status under the EU Settlement Scheme?"

Scenario: Continue with British or Irish citizen
    Given I have selected the "British or Irish citizen" radio button
    When I click on Continue
    Then I will be directed to the next page in the user journey "Are you in paid work?"

Scenario: Back navigation
    When I click the back link
    Then I should be returned to the previous page in the user journey "What is your age?"
