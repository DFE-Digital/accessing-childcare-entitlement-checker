@smoke_tests
Feature: End to End Use Cases

Background:
    Given I am on the childcare entitlement checker website
    And I click the link to start the journey

    
Scenario: Scenario 1 - ingle parent earning below the threshold, household receives Universal Credit, child is not born yet
    Given I complete the journey for the use case "Single parent earning below the threshold, household receives Universal Credit, child is not born yet"
    Then the page header is "Check your answers"
    And I should see 3 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                                            | Answer                                         |
        | Where do you live?                                                  | England                                        |
        | What is your age?                                                   | 21 or over                                     |
        | What is your nationality?                                           | British or Irish citizen                       |
        | Are you in paid work?                                               | Yes                                            |
        | How would you describe your work status?                            | Paid employment                                |
        | Does your household receive universal credit?                       | Yes                                            |
        | Do you get any of these benefits?                                   | No, I do not get any of these benefits         |
        | Do you already get any of this childcare support?                   | No, I do not get any of this childcare support |
        | Do you live with a partner?                                         | No                                             |
        | On average, do you earn £__PLACEHOLDER__ a week or more before tax? | No                                             |
    When I click on Continue
    Then the page header is "Childcare support you could get"    

Scenario: Scenario 2 - One parent on carer's allowance, child receives DLA
    Given I complete the journey for the use case "One parent on carer's allowance, child receives DLA"
    Then the page header is "Check your answers"
    And I should see 2 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                          | Answer                                         |
        | Where do you live?                                | England                                        |
        | What is your age?                                 | 21 or over                                     |
        | What is your nationality?                         | British or Irish citizen                       |
        | Are you in paid work?                             | No, I am not in work                           |
        | Does your household receive universal credit?     | No                                             |
        | Do you get any of these benefits?                 | Carer's Allowance                              |
        | Do you already get any of this childcare support? | No, I do not get any of this childcare support |
        | Do you live with a partner?                       | Yes                                            |
    And I should see a summary list for "Your partners details" with the following summary:
        | Question                                                                       | Answer                                            |
        | What is your partner's age?                                                    | 21 or over                                        |
        | Is your partner in paid work?                                                  | Yes                                               |
        | How would you describe your partner's work status?                             | Paid employment                                   |
        | Is your partner's adjusted net income more than £100,000 a year?               | No                                                |
        | Does your partner get any of these benefits?                                   | No, they do not get any of these benefits         |
        | Does your partner already get any of this childcare support?                   | No, they do not get any of this childcare support |
        | On average, does your partner earn £__PLACEHOLDER__ a week or more before tax? | Yes                                               |
    When I click on Continue
    Then the page header is "Childcare support you could get"          
    
Scenario: Scenario 3 - One parent is earning under the threshold, household receives Universal Credit
    Given I complete the journey for the use case "One parent is earning under the threshold, household receives Universal Credit"
    Then the page header is "Check your answers"
    And I should see 1 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                                            | Answer                                         |
        | Where do you live?                                                  | England                                        |
        | What is your age?                                                   | 21 or over                                     |
        | What is your nationality?                                           | British or Irish citizen                       |
        | Are you in paid work?                                               | Yes                                            |
        | How would you describe your work status?                            | Paid employment                                |
        | Does your household receive universal credit?                       | Yes                                            |
        | Do you get any of these benefits?                                   | No, I do not get any of these benefits         |
        | Do you already get any of this childcare support?                   | No, I do not get any of this childcare support |
        | Do you live with a partner?                                         | Yes                                            |
        | On average, do you earn £__PLACEHOLDER__ a week or more before tax? | No                                             |
    And I should see a summary list for "Your partners details" with the following summary:
        | Question                                                                       | Answer                                            |
        | What is your partner's age?                                                    | 21 or over                                        |
        | Is your partner in paid work?                                                  | Yes                                               |
        | How would you describe your partner's work status?                             | Paid employment                                   |
        | Is your partner's adjusted net income more than £100,000 a year?               | No                                                |
        | Does your partner get any of these benefits?                                   | No, they do not get any of these benefits         |
        | Does your partner already get any of this childcare support?                   | No, they do not get any of this childcare support |
        | On average, does your partner earn £__PLACEHOLDER__ a week or more before tax? | Yes                                               |
    When I click on Continue
    Then the page header is "Childcare support you could get"     

Scenario: Scenario 4 - One parent aged 18-20, child not yet born
    Given I complete the journey for the use case "One parent aged 18-20, child not yet born"
    Then the page header is "Check your answers"
    And I should see 2 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                                            | Answer                                         |
        | Where do you live?                                                  | England                                        |
        | What is your age?                                                   | 18 to 20                                       |
        | What is your nationality?                                           | British or Irish citizen                       |
        | Are you in paid work?                                               | Yes                                            |
        | How would you describe your work status?                            | Paid employment                                |
        | Is your adjusted net income more than £100,000 a year?              | No                                             |
        | Does your household receive universal credit?                       | No                                             |
        | Do you get any of these benefits?                                   | No, I do not get any of these benefits         |
        | Do you already get any of this childcare support?                   | No, I do not get any of this childcare support |
        | Do you live with a partner?                                         | Yes                                            |
        | On average, do you earn £__PLACEHOLDER__ a week or more before tax? | Yes                                            |
    And I should see a summary list for "Your partners details" with the following summary:
        | Question                                                                       | Answer                                            |
        | What is your partner's age?                                                    | 21 or over                                        |
        | Is your partner in paid work?                                                  | Yes                                               |
        | How would you describe your partner's work status?                             | Paid employment                                   |
        | Is your partner's adjusted net income more than £100,000 a year?               | No                                                |
        | Does your partner get any of these benefits?                                   | No, they do not get any of these benefits         |
        | Does your partner already get any of this childcare support?                   | No, they do not get any of this childcare support |
        | On average, does your partner earn £__PLACEHOLDER__ a week or more before tax? | Yes                                               |
    When I click on Continue
    Then the page header is "Childcare support you could get"    

Scenario: Scenario 5 - Single parent who is self employed, child is not born yet
    Given I complete the journey for the use case "Single parent who is self employed, child is not born yet"
    Then the page header is "Check your answers"
    And I should see 1 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                             | Answer                                         |
        | Where do you live?                                   | England                                        |
        | What is your age?                                    | 21 or over                                     |
        | What is your nationality?                            | British or Irish citizen                       |
        | Are you in paid work?                                | Yes                                            |
        | How would you describe your work status?             | Self-employed                                  |
        | Have you been self-employed for less than 12 months? | Yes                                            |
        | Does your household receive universal credit?        | No                                             |
        | Do you get any of these benefits?                    | No, I do not get any of these benefits         |
        | Do you already get any of this childcare support?    | No, I do not get any of this childcare support |
        | Do you live with a partner?                          | No                                             |
    When I click on Continue
    Then the page header is "Childcare support you could get"    
        
Scenario: Scenario 6 - Both parents under 18, one parent an apprentice, one parent earning under the threshold
    Given I complete the journey for the use case "Both parents under 18, one parent an apprentice, one parent earning under the threshold"
    Then the page header is "Check your answers"
    And I should see 1 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                                            | Answer                                         |
        | Where do you live?                                                  | England                                        |
        | What is your age?                                                   | Under 18                                       |
        | What is your nationality?                                           | British or Irish citizen                       |
        | Are you in paid work?                                               | Yes                                            |
        | How would you describe your work status?                            | Apprentice                                     |
        | Is your adjusted net income more than £100,000 a year?              | No                                             |
        | Does your household receive universal credit?                       | No                                             |
        | Do you get any of these benefits?                                   | No, I do not get any of these benefits         |
        | Do you already get any of this childcare support?                   | No, I do not get any of this childcare support |
        | Do you live with a partner?                                         | Yes                                            |
        | On average, do you earn £__PLACEHOLDER__ a week or more before tax? | Yes                                            |
    And I should see a summary list for "Your partners details" with the following summary:
        | Question                                                                       | Answer                                            |
        | What is your partner's age?                                                    | Under 18                                          |
        | Is your partner in paid work?                                                  | Yes                                               |
        | How would you describe your partner's work status?                             | Paid employment                                   |
        | Does your partner get any of these benefits?                                   | No, they do not get any of these benefits         |
        | Does your partner already get any of this childcare support?                   | No, they do not get any of this childcare support |
        | On average, does your partner earn £__PLACEHOLDER__ a week or more before tax? | No                                                |
    When I click on Continue
    Then the page header is "Childcare support you could get"

Scenario: Scenario 7 - One parent on parental leave
    Given I complete the journey for the use case "One parent on parental leave"
    Then the page header is "Check your answers"
    And I should see 2 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                                            | Answer                                         |
        | Where do you live?                                                  | England                                        |
        | What is your age?                                                   | 21 or over                                     |
        | What is your nationality?                                           | British or Irish citizen                       |
        | Are you in paid work?                                               | Yes                                            |
        | How would you describe your work status?                            | Paid employment                                |
        | Is your adjusted net income more than £100,000 a year?              | No                                             |
        | Does your household receive universal credit?                       | No                                             |
        | Do you get any of these benefits?                                   | No, I do not get any of these benefits         |
        | Do you already get any of this childcare support?                   | No, I do not get any of this childcare support |
        | Do you live with a partner?                                         | Yes                                            |
        | On average, do you earn £__PLACEHOLDER__ a week or more before tax? | Yes                                            |
    And I should see a summary list for "Your partners details" with the following summary:
        | Question                                                                                                      | Answer                                            |
        | What is your partner's age?                                                                                   | 21 or over                                        |
        | Is your partner in paid work?                                                                                 | Yes, but they are on parental leave               |
        | Which child is your partner on leave for?                                                                     | Paula                                             |
        | How would you describe your partner's work status?                                                            | Paid employment                                   |
        | Is your partner's adjusted net income more than £100,000 a year?                                              | No                                                |
        | Does your partner get any of these benefits?                                                                  | No, they do not get any of these benefits         |
        | Does your partner already get any of this childcare support?                                                  | No, they do not get any of this childcare support |
        | On average, will your partner earn £__PLACEHOLDER__ a week or more before tax when their parental leave ends? | Yes                                               |
    When I click on Continue
    #TODO: Pending leave results page updates
    #Then the page header is "Childcare support you could get"

Scenario: Scenario 8 - Single parent on sick leave, parent is a citizen of a different country
    Given I complete the journey for the use case "Single parent on sick leave, parent is a citizen of a different country"
    Then the page header is "Check your answers"
    And I should see 1 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                               | Answer                                         |
        | Where do you live?                                     | England                                        |
        | What is your age?                                      | 21 or over                                     |
        | What is your nationality?                              | Citizen of a different country                 |
        | Are you in paid work?                                  | Yes, but I am on sick leave                    |
        | How would you describe your work status?               | Paid employment                                |
        | Is your adjusted net income more than £100,000 a year? | No                                             |
        | Does your household receive universal credit?          | Yes                                            |
        | Do you get any of these benefits?                      | No, I do not get any of these benefits         |
        | Do you already get any of this childcare support?      | No, I do not get any of this childcare support |
        | Do you live with a partner?                            | No                                             |
    When I click on Continue
    #TODO: Pending leave results page updates
    #Then the page header is "Childcare support you could get"

Scenario: Scenario 9 - One parent not working, one parent receiving ESA
    Given I complete the journey for the use case "One parent not working, one parent receiving ESA"
    Then the page header is "Check your answers"
    And I should see 2 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                          | Answer                                              |
        | Where do you live?                                | England                                             |
        | What is your age?                                 | 21 or over                                          |
        | What is your nationality?                         | British or Irish citizen                            |
        | Are you in paid work?                             | No, I am not in work                                |
        | Does your household receive universal credit?     | Yes                                                 |
        | Do you get any of these benefits?                 | Contribution-based Employment and Support Allowance |
        | Do you already get any of this childcare support? | No, I do not get any of this childcare support      |
        | Do you live with a partner?                       | Yes                                                 |
    And I should see a summary list for "Your partners details" with the following summary:
        | Question                                                                       | Answer                                            |
        | What is your partner's age?                                                    | 21 or over                                        |
        | Is your partner in paid work?                                                  | Yes                                               |
        | How would you describe your partner's work status?                             | Paid employment                                   |
        | Is your partner's adjusted net income more than £100,000 a year?               | No                                                |
        | Does your partner get any of these benefits?                                   | No, they do not get any of these benefits         |
        | Does your partner already get any of this childcare support?                   | No, they do not get any of this childcare support |
        | On average, does your partner earn £__PLACEHOLDER__ a week or more before tax? | Yes                                               |
    When I click on Continue
    Then the page header is "Childcare support you could get"

Scenario: Scenario 10 - Parent is a non-UK national without pre-settled or settled status
    Given I complete the journey for the use case "Parent is a non-UK national without pre-settled or settled status"
    Then the page header is "Check your answers"
    And I should see 2 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                                                  | Answer                                               |
        | Where do you live?                                                        | England                                              |
        | What is your age?                                                         | 21 or over                                           |
        | What is your nationality?                                                 | Citizen of an EU country, EEA country or Switzerland |
        | Do you have settled or pre-settled status under the EU Settlement Scheme? | No                                                   |
        | Are you in paid work?                                                     | Yes                                                  |
        | How would you describe your work status?                                  | Paid employment                                      |
        | Is your adjusted net income more than £100,000 a year?                    | No                                                   |
        | Does your household receive universal credit?                             | No                                                   |
        | Do you get any of these benefits?                                         | No, I do not get any of these benefits               |
        | Do you already get any of this childcare support?                         | No, I do not get any of this childcare support       |
        | Do you live with a partner?                                               | No                                                   |
        | On average, do you earn £__PLACEHOLDER__ a week or more before tax?       | Yes                                                  |
    When I click on Continue
    Then the page header is "Childcare support you could get"

Scenario: Scenario 11 - Single parent not working, on carer's allowance
    Given I complete the journey for the use case "Single parent not working, on carer's allowance"
    Then the page header is "Check your answers"
    And I should see 1 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                          | Answer                                         |
        | Where do you live?                                | England                                        |
        | What is your age?                                 | 21 or over                                     |
        | What is your nationality?                         | British or Irish citizen                       |
        | Are you in paid work?                             | No, I am not in work                           |
        | Does your household receive universal credit?     | Yes                                            |
        | Do you get any of these benefits?                 | Carer's Allowance                              |
        | Do you already get any of this childcare support? | No, I do not get any of this childcare support |
        | Do you live with a partner?                       | No                                             |
    When I click on Continue
    Then the page header is "Childcare support you could get"
    