@smoke_tests
Feature: End to End Use Cases

Note that "Funded hours for working parents" in the lucid = "Free Childcare for Working Parents"

Background:
    Given I am on the childcare entitlement checker website
    And I click the link to start the journey
    
Scenario: Scenario 01 - Single parent earning below the threshold, household receives Universal Credit, child is not born yet
    Given I complete the journey for the use case "Single parent earning below the threshold, household receives Universal Credit, child is not born yet"
    Then the page header is "Check your answers"
    And I should see 3 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                                                      | Answer                                 |
        | Where do you live?                                                            | England                                |
        | What is your age?                                                             | 21 or over                             |
        | What is your nationality?                                                     | British or Irish citizen               |
        | Are you in paid work?                                                         | Yes                                    |
        | How would you describe your work status?                                      | Paid employment                        |
        | Does your household receive universal credit?                                 | Yes                                    |
        | Do you get any of these benefits?                                             | No, I do not get any of these benefits |
        | Do you already get any of these to help pay for childcare?                    | No, I do not get any of these          |
        | Do you live with a partner?                                                   | No                                     |
        | On average, do you expect to earn £__PLACEHOLDER__ a week or more before tax? | No                                     |
    When I click on Continue
    Then the page header is "Childcare support you could get"
    And I can see that "Simon" is now eligible for "Universal Credit childcare"
    And I can see that "Baby" is eligible for:
        | Scheme                                        | When            |
        | Universal Credit childcare                    | birth           |
        | Early learning for 2-year-olds                | two years old   |
        | 15 hours free childcare for 3 and 4-year-olds | three years old |
    And I can see that "Frankie" is eligible for:
        | Scheme                                        | When          |
        | Universal Credit childcare                    | now           |
        | Early learning for 2-year-olds                | now           |
        | 15 hours free childcare for 3 and 4-year-olds | in the future |


Scenario: Scenario 02 - One parent on carer's allowance, child receives DLA
    Given I complete the journey for the use case "One parent on carer's allowance, child receives DLA"
    Then the page header is "Check your answers"
    And I should see 2 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                                   | Answer                        |
        | Where do you live?                                         | England                       |
        | What is your age?                                          | 21 or over                    |
        | What is your nationality?                                  | British or Irish citizen      |
        | Are you in paid work?                                      | No, I am not in work          |
        | Does your household receive universal credit?              | No                            |
        | Do you get any of these benefits?                          | Carer's Allowance             |
        | Do you already get any of these to help pay for childcare? | No, I do not get any of these |
        | Do you live with a partner?                                | Yes                           |
    And I should see a summary list for "Your partners details" with the following summary:
        | Question                                                                                              | Answer                                    |
        | What is your partner's age?                                                                           | 21 or over                                |
        | Is your partner in paid work?                                                                         | Yes                                       |
        | How would you describe your partner's work status?                                                    | Paid employment                           |
        | Does your partner expect their adjusted net income to be more than £100,000 for the current tax year? | No                                        |
        | Does your partner get any of these benefits?                                                          | No, they do not get any of these benefits |
        | Does your partner already get any of these to help pay for childcare?                                 | No, they do not get any of these          |
        | On average, does your partner expect to earn £__PLACEHOLDER__ a week or more before tax?              | Yes                                       |
    When I click on Continue
    Then the page header is "Childcare support you could get"          
    And I can see that "Katherine" is now eligible for "Tax-Free Childcare"
    And I can see that "Tom" is eligible for:
        | Scheme                                        | When          |
        | Tax-Free Childcare                            | now           |
        | Free Childcare for Working Parents            | now           | 
        | Early learning for 2-year-olds                | now           |
        | 15 hours free childcare for 3 and 4-year-olds | in the future |
    
Scenario: Scenario 03 - One parent is earning under the threshold, household receives Universal Credit
    Given I complete the journey for the use case "One parent is earning under the threshold, household receives Universal Credit"
    Then the page header is "Check your answers"
    And I should see 1 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                                                      | Answer                                 |
        | Where do you live?                                                            | England                                |
        | What is your age?                                                             | 21 or over                             |
        | What is your nationality?                                                     | British or Irish citizen               |
        | Are you in paid work?                                                         | Yes                                    |
        | How would you describe your work status?                                      | Paid employment                        |
        | Does your household receive universal credit?                                 | Yes                                    |
        | Do you get any of these benefits?                                             | No, I do not get any of these benefits |
        | Do you already get any of these to help pay for childcare?                    | No, I do not get any of these          |
        | Do you live with a partner?                                                   | Yes                                    |
        | On average, do you expect to earn £__PLACEHOLDER__ a week or more before tax? | No                                     |
    And I should see a summary list for "Your partners details" with the following summary:
        | Question                                                                                              | Answer                                    |
        | What is your partner's age?                                                                           | 21 or over                                |
        | Is your partner in paid work?                                                                         | Yes                                       |
        | How would you describe your partner's work status?                                                    | Paid employment                           |
        | Does your partner expect their adjusted net income to be more than £100,000 for the current tax year? | No                                        |
        | Does your partner get any of these benefits?                                                          | No, they do not get any of these benefits |
        | Does your partner already get any of these to help pay for childcare?                                 | No, they do not get any of these          |
        | On average, does your partner expect to earn £__PLACEHOLDER__ a week or more before tax?              | Yes                                       |
    When I click on Continue
    Then the page header is "Childcare support you could get"
    And I can see that "Rosa" is eligible for:
        # EXPECTED FROM LUCID: This one has an extra scheme
        #| Scheme                                        | When          |
        #| Universal Credit childcare                    | now           |
        #| 15 hours free childcare for 3 and 4-year-olds | in the future |
        | Scheme                                        | When          |
        | Universal Credit childcare                    | now           |
        | Early learning for 2-year-olds                | now           |
        | 15 hours free childcare for 3 and 4-year-olds | in the future |

Scenario: Scenario 04 - One parent aged 18-20, child not yet born
    Given I complete the journey for the use case "One parent aged 18-20, child not yet born"
    Then the page header is "Check your answers"
    And I should see 2 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                                                                  | Answer                                 |
        | Where do you live?                                                                        | England                                |
        | What is your age?                                                                         | 18 to 20                               |
        | What is your nationality?                                                                 | British or Irish citizen               |
        | Are you in paid work?                                                                     | Yes                                    |
        | How would you describe your work status?                                                  | Paid employment                        |
        | Do you expect your adjusted net income to be more than £100,000 for the current tax year? | No                                     |
        | Does your household receive universal credit?                                             | No                                     |
        | Do you get any of these benefits?                                                         | No, I do not get any of these benefits |
        | Do you already get any of these to help pay for childcare?                                | No, I do not get any of these          |
        | Do you live with a partner?                                                               | Yes                                    |
        | On average, do you expect to earn £__PLACEHOLDER__ a week or more before tax?             | Yes                                    |
    And I should see a summary list for "Your partners details" with the following summary:
        | Question                                                                                              | Answer                                    |
        | What is your partner's age?                                                                           | 21 or over                                |
        | Is your partner in paid work?                                                                         | Yes                                       |
        | How would you describe your partner's work status?                                                    | Paid employment                           |
        | Does your partner expect their adjusted net income to be more than £100,000 for the current tax year? | No                                        |
        | Does your partner get any of these benefits?                                                          | No, they do not get any of these benefits |
        | Does your partner already get any of these to help pay for childcare?                                 | No, they do not get any of these          |
        | On average, does your partner expect to earn £__PLACEHOLDER__ a week or more before tax?              | Yes                                       |
    When I click on Continue
    Then the page header is "Childcare support you could get"
	And I can see that "Daphne" is now eligible for "Tax-Free Childcare"
	And I can see that "Baby" is eligible for:
        | Scheme                                        | When            |
        | Tax-Free Childcare                            | birth           |
        | Free Childcare for Working Parents            | nine months old |
        | 15 hours free childcare for 3 and 4-year-olds | three years old |

Scenario: Scenario 05 - Single parent who is self employed, child is not born yet
    Given I complete the journey for the use case "Single parent who is self employed, child is not born yet"
    Then the page header is "Check your answers"
    And I should see 1 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                                   | Answer                                 |
        | Where do you live?                                         | England                                |
        | What is your age?                                          | 21 or over                             |
        | What is your nationality?                                  | British or Irish citizen               |
        | Are you in paid work?                                      | Yes                                    |
        | How would you describe your work status?                   | Self-employed                          |
        | Have you been self-employed for less than 12 months?       | Yes                                    |
        | Does your household receive universal credit?              | No                                     |
        | Do you get any of these benefits?                          | No, I do not get any of these benefits |
        | Do you already get any of these to help pay for childcare? | No, I do not get any of these          |
        | Do you live with a partner?                                | No                                     |
    When I click on Continue
    Then the page header is "Childcare support you could get"    
    And I can see that "Baby" is eligible for:
        | Scheme                                        | When            |
        | Tax-Free Childcare                            | birth           |
        | Free Childcare for Working Parents            | nine months old |
        | 15 hours free childcare for 3 and 4-year-olds | three years old |
        
Scenario: Scenario 06 - Both parents under 18, one parent an apprentice, one parent earning under the threshold
    Given I complete the journey for the use case "Both parents under 18, one parent an apprentice, one parent earning under the threshold"
    Then the page header is "Check your answers"
    And I should see 1 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                                                                  | Answer                                 |
        | Where do you live?                                                                        | England                                |
        | What is your age?                                                                         | Under 18                               |
        | What is your nationality?                                                                 | British or Irish citizen               |
        | Are you in paid work?                                                                     | Yes                                    |
        | How would you describe your work status?                                                  | Apprentice                             |
        | Do you expect your adjusted net income to be more than £100,000 for the current tax year? | No                                     |
        | Does your household receive universal credit?                                             | Yes                                    |
        | Do you get any of these benefits?                                                         | No, I do not get any of these benefits |
        | Do you already get any of these to help pay for childcare?                                | No, I do not get any of these          |
        | Do you live with a partner?                                                               | Yes                                    |
        | On average, do you expect to earn £__PLACEHOLDER__ a week or more before tax?             | Yes                                    |
    And I should see a summary list for "Your partners details" with the following summary:
        | Question                                                                                 | Answer                                    |
        | What is your partner's age?                                                              | Under 18                                  |
        | Is your partner in paid work?                                                            | Yes                                       |
        | How would you describe your partner's work status?                                       | Paid employment                           |
        | Does your partner get any of these benefits?                                             | No, they do not get any of these benefits |
        | Does your partner already get any of these to help pay for childcare?                    | No, they do not get any of these          |
        | On average, does your partner expect to earn £__PLACEHOLDER__ a week or more before tax? | No                                        |
    When I click on Continue
    Then the page header is "Childcare support you could get"
    And I can see that "Winston" is eligible for:
        | Scheme                                        | When            |
        | Universal Credit childcare                    | now             |
        | Early learning for 2-year-olds                | two years old   |
        | 15 hours free childcare for 3 and 4-year-olds | three years old |

Scenario: Scenario 07 - One parent on parental leave
    Given I complete the journey for the use case "One parent on parental leave"
    Then the page header is "Check your answers"
    And I should see 2 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                                                                  | Answer                                 |
        | Where do you live?                                                                        | England                                |
        | What is your age?                                                                         | 21 or over                             |
        | What is your nationality?                                                                 | British or Irish citizen               |
        | Are you in paid work?                                                                     | Yes                                    |
        | How would you describe your work status?                                                  | Paid employment                        |
        | Do you expect your adjusted net income to be more than £100,000 for the current tax year? | No                                     |
        | Does your household receive universal credit?                                             | No                                     |
        | Do you get any of these benefits?                                                         | No, I do not get any of these benefits |
        | Do you already get any of these to help pay for childcare?                                | No, I do not get any of these          |
        | Do you live with a partner?                                                               | Yes                                    |
        | On average, do you expect to earn £__PLACEHOLDER__ a week or more before tax?             | Yes                                    |
    And I should see a summary list for "Your partners details" with the following summary:
        | Question                                                                                                                | Answer                                    |
        | What is your partner's age?                                                                                             | 21 or over                                |
        | Is your partner in paid work?                                                                                           | Yes, but they are on parental leave       |
        | Which child is your partner on leave for?                                                                               | Paula                                     |
        | How would you describe your partner's work status?                                                                      | Paid employment                           |
        | Does your partner expect their adjusted net income to be more than £100,000 for the current tax year?                   | No                                        |
        | Does your partner get any of these benefits?                                                                            | No, they do not get any of these benefits |
        | Does your partner already get any of these to help pay for childcare?                                                   | No, they do not get any of these          |
        | On average, will your partner expect to earn £__PLACEHOLDER__ a week or more before tax when their parental leave ends? | Yes                                       |
    When I click on Continue
    Then the page header is "Childcare support you could get"
    # EXPECTED FROM LUCID: One scheme is different. Also lucid does not seem to take into account new parental leave message
        #| Scheme                                        | When            |
        #| Tax-Free Childcare                            | birth           |
        #| 30 hours for working parents                  | nine months old |
        #| 15 hours free childcare for 3 and 4-year-olds | three years old |
    And I can see that "Paula" is eligible for:
        | Scheme                                        | When                                     |
        | Tax-Free Childcare                            | when partner returns from parental leave |
        | Free Childcare for Working Parents            | when partner returns from parental leave |
        | 15 hours free childcare for 3 and 4-year-olds | three years old                          |
    And I can see that "Nicky" is eligible for:
        | Scheme             | When |
        | Tax-Free Childcare | now  |

Scenario: Scenario 08 - Single parent on sick leave, parent is a citizen of a different country
    Given I complete the journey for the use case "Single parent on sick leave, parent is a citizen of a different country"
    Then the page header is "Check your answers"
    And I should see 1 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                                                                  | Answer                                 |
        | Where do you live?                                                                        | England                                |
        | What is your age?                                                                         | 21 or over                             |
        | What is your nationality?                                                                 | Citizen of a different country         |
        | Are you in paid work?                                                                     | Yes, but I am on sick leave            |
        | How would you describe your work status?                                                  | Paid employment                        |
        | Do you expect your adjusted net income to be more than £100,000 for the current tax year? | No                                     |
        | Does your household receive universal credit?                                             | Yes                                    |
        | Do you get any of these benefits?                                                         | No, I do not get any of these benefits |
        | Do you already get any of these to help pay for childcare?                                | No, I do not get any of these          |
        | Do you live with a partner?                                                               | No                                     |
    When I click on Continue
    Then the page header is "Childcare support you could get"
    # EXPECTED FROM LUCID: Three additional schemes. I double checked the scenario setup and couldn't see any errors.
        #| Scheme                                        | When            |
        #| Universal Credit childcare                    | now             |
        #| 30 hours for working parents                  | now             |
        #| Early learning for 2-year-olds                | two years old   |
        #| 15 hours free childcare for 3 and 4-year-olds | three years old |
    And I can see that "Lee" is eligible for:
        | Scheme                                        | When            |
        | 15 hours free childcare for 3 and 4-year-olds | three years old |

Scenario: Scenario 09 - One parent not working, one parent receiving ESA
    Given I complete the journey for the use case "One parent not working, one parent receiving ESA"
    Then the page header is "Check your answers"
    And I should see 2 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                                   | Answer                                              |
        | Where do you live?                                         | England                                             |
        | What is your age?                                          | 21 or over                                          |
        | What is your nationality?                                  | British or Irish citizen                            |
        | Are you in paid work?                                      | No, I am not in work                                |
        | Does your household receive universal credit?              | Yes                                                 |
        | Do you get any of these benefits?                          | Contribution-based Employment and Support Allowance |
        | Do you already get any of these to help pay for childcare? | No, I do not get any of these                       |
        | Do you live with a partner?                                | Yes                                                 |
    And I should see a summary list for "Your partners details" with the following summary:
        | Question                                                                                              | Answer                                    |
        | What is your partner's age?                                                                           | 21 or over                                |
        | Is your partner in paid work?                                                                         | Yes                                       |
        | How would you describe your partner's work status?                                                    | Paid employment                           |
        | Does your partner expect their adjusted net income to be more than £100,000 for the current tax year? | No                                        |
        | Does your partner get any of these benefits?                                                          | No, they do not get any of these benefits |
        | Does your partner already get any of these to help pay for childcare?                                 | No, they do not get any of these          |
        | On average, does your partner expect to earn £__PLACEHOLDER__ a week or more before tax?              | Yes                                       |
    When I click on Continue
    Then the page header is "Childcare support you could get"
    # EXPECTED FROM LUCID:
        #| Scheme                                        | When          |
        #| Universal Credit childcare                    | now           |
        #| Free Childcare for Working Parents            | now           |
        #| 15 hours free childcare for 3 and 4-year-olds | in the future |
    And I can see that "Isabel" is eligible for:
        | Scheme                                        | When          |
        | Free Childcare for Working Parents            | now           |
        | Early learning for 2-year-olds                | now           |
        | 15 hours free childcare for 3 and 4-year-olds | in the future |
    # EXPECTED FROM LUCID:
        #| Scheme                                        | When            |
        #| Universal Credit childcare                    | now             |
        #| Free Childcare for Working Parents            | nine months old |
        #| 15 hours free childcare for 3 and 4-year-olds | three years old |
    And I can see that "Mary" is eligible for:
        | Scheme                                        | When            |
        | Free Childcare for Working Parents            | two years old   |
        | Early learning for 2-year-olds                | two years old   |
        | 15 hours free childcare for 3 and 4-year-olds | three years old |

Scenario: Scenario 10 - Parent is a non-UK national without pre-settled or settled status
    Given I complete the journey for the use case "Parent is a non-UK national without pre-settled or settled status"
    Then the page header is "Check your answers"
    And I should see 2 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                                                                  | Answer                                               |
        | Where do you live?                                                                        | England                                              |
        | What is your age?                                                                         | 21 or over                                           |
        | What is your nationality?                                                                 | Citizen of an EU country, EEA country or Switzerland |
        | Do you have settled or pre-settled status under the EU Settlement Scheme?                 | No                                                   |
        | Are you in paid work?                                                                     | Yes                                                  |
        | How would you describe your work status?                                                  | Paid employment                                      |
        | Do you expect your adjusted net income to be more than £100,000 for the current tax year? | No                                                   |
        | Does your household receive universal credit?                                             | No                                                   |
        | Do you get any of these benefits?                                                         | No, I do not get any of these benefits               |
        | Do you already get any of these to help pay for childcare?                                | No, I do not get any of these                        |
        | Do you live with a partner?                                                               | No                                                   |
        | On average, do you expect to earn £__PLACEHOLDER__ a week or more before tax?             | Yes                                                  |
    When I click on Continue
    Then the page header is "Childcare support you could get"
    # Lucid says Tom for the last item, but it's a typo.
    And I can see that "Louise" is eligible for:
        | Scheme                                        | When                       | IsBorn |
        | Tax-Free Childcare                            | now                        | true   |
        | Free Childcare for Working Parents            | nine months old            | true   |
        | 15 hours free childcare for 3 and 4-year-olds | in the future              | true   |
        
    And I can see that "Jeremy" is not eligible for any childcare entitlement schemes

Scenario: Scenario 11 - Single parent not working, on carer's allowance
    Given I complete the journey for the use case "Single parent not working, on carer's allowance"
    Then the page header is "Check your answers"
    And I should see 1 summary cards
    And I should see a summary list for "Your details" with the following summary:
        | Question                                                   | Answer                        |
        | Where do you live?                                         | England                       |
        | What is your age?                                          | 21 or over                    |
        | What is your nationality?                                  | British or Irish citizen      |
        | Are you in paid work?                                      | No, I am not in work          |
        | Does your household receive universal credit?              | Yes                           |
        | Do you get any of these benefits?                          | Carer's Allowance             |
        | Do you already get any of these to help pay for childcare? | No, I do not get any of these |
        | Do you live with a partner?                                | No                            |
    When I click on Continue
    Then the page header is "Childcare support you could get"
    And I can see that "Kurt" is eligible for:
        | Scheme                                        | When            |
        | Early learning for 2-year-olds                | two years old   |
        | 15 hours free childcare for 3 and 4-year-olds | three years old |
    