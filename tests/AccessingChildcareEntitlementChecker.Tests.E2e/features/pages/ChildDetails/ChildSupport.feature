Feature: Does child get any of the following support?

Background:
    Given I am on the childcare entitlement checker website
    And I start the journey and answer the questions as follows:
    | Question                           | Answer    |
    | Where do you live?                 | England   |
    | Add details of a child             | Jack      |
	| Has this child been born yet?      | Yes       |
    | What is Jack's date of birth?      | Yesterday |
    | What is your relationship to Jack? | Parent    |

Scenario: Page load
    Given the page header is "Does Jack get any of the following support?"
    Then I should see 6 checkboxes with the following options:
    | Checkbox                              |
    | Armed Forces Independence Payment     |
    | Certificate of Visual Impairment      |
    | Disability Living Allowance (DLA)     |
    | Education, health and care (EHC) plan |
    | Personal Independence Payment (PIP)   |
    | No, none of these apply               |

Scenario: Checkbox selection
    When I select the "Armed Forces Independence Payment" checkbox
    And I select the "Certificate of Visual Impairment" checkbox
    Then the following checkboxes should be selected:
    | Checkbox                              |
    | Armed Forces Independence Payment     |
    | Certificate of Visual Impairment      |

Scenario: Continuing with none applicable while others are selected results in an error
    When I select the "Armed Forces Independence Payment" checkbox
    And I select the "Certificate of Visual Impairment" checkbox
    And I select the "No, none of these apply" checkbox
    When I click on Continue
    Then an error summary box should appear at the top of the page
    And the error summary title should be "There is a problem" with an error message "You may not select 'no, none of these apply' with other options"
    And inline validation should display with the error message "You may not select 'no, none of these apply' with other options"

Scenario: Continue without selection
    Given I have not selected a checkbox
    When I click on Continue
    Then an error summary box should appear at the top of the page
    And the error summary title should be "There is a problem" with an error message "Select any support Jack gets, or select 'No, none of these apply'"
    And inline validation should display with the error message "Select any support Jack gets, or select 'No, none of these apply'"

Scenario: Continue with selection
    Given I have selected the "Armed Forces Independence Payment" checkbox
    And I have selected the "Certificate of Visual Impairment" checkbox
    When I click on Continue
    Then I will be directed to the next page in the user journey "Check your children's details"

Scenario: Continue with none applicable
    Given I have selected the "No, none of these apply" checkbox
    When I click on Continue
    Then I will be directed to the next page in the user journey "Check your children's details"

Scenario: Back navigation
    When I click the back link
    Then I should be returned to the previous page in the user journey "What is your relationship to Jack?"