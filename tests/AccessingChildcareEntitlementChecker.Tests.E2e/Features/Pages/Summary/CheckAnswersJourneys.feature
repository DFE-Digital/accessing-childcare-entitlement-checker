Feature: Check your answers - Journeys

Background:
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue

Scenario: Nationality change to British or Irish citizen returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                                                  | Answer                                                      |
		| What is your age?                                                         | Under 18                                                    |
		| What is your nationality?                                                 | Citizen of an EU country, EEA country or Switzerland        |
		| Do you have settled or pre-settled status under the EU Settlement Scheme? | Yes                                                         |
		| Are you in paid work?                                                     | No                                                          |
		| Does your household receive universal credit?                             | Yes                                                         |
		| Do you get any of these benefits?                                         | Carer's Allowance                                           |
		| Do you already get any of this childcare support?                         | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                               | No                                                          |
	When I click the Change link in the "Your details" summary list for "What is your nationality?"
	And I select the "British or Irish citizen" radio button
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "Do you have settled or pre-settled status under the EU Settlement Scheme?"

Scenario: Nationality change to Citizen of a different country returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                                                  | Answer                                                      |
		| What is your age?                                                         | Under 18                                                    |
		| What is your nationality?                                                 | Citizen of an EU country, EEA country or Switzerland        |
		| Do you have settled or pre-settled status under the EU Settlement Scheme? | Yes                                                         |
		| Are you in paid work?                                                     | No                                                          |
		| Does your household receive universal credit?                             | Yes                                                         |
		| Do you get any of these benefits?                                         | Carer's Allowance                                           |
		| Do you already get any of this childcare support?                         | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                               | No                                                          |
	When I click the Change link in the "Your details" summary list for "What is your nationality?"
	And I select the "Citizen of a different country" radio button
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "Do you have settled or pre-settled status under the EU Settlement Scheme?"

Scenario: Nationality change to Citizen of an EU country, EEA country or Switzerland opens Do you have settled or pre-settled status under the EU Settlement Scheme?
	Given I answer questions as follows:
		| Question                                          | Answer                                                      |
		| What is your age?                                 | Under 18                                                    |
		| What is your nationality?                         | British or Irish citizen                                    |
		| Are you in paid work?                             | No                                                          |
		| Does your household receive universal credit?     | Yes                                                         |
		| Do you get any of these benefits?                 | Carer's Allowance                                           |
		| Do you already get any of this childcare support? | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                       | No                                                          |
	When I click the Change link in the "Your details" summary list for "What is your nationality?"
	And I select the "Citizen of an EU country, EEA country or Switzerland" radio button
	And I click on Continue
	Then the page header is "Do you have settled or pre-settled status under the EU Settlement Scheme?"

Scenario: PaidWork change to I am on leave from work opens TypeOfLeave
	Given I answer questions as follows:
		| Question                                          | Answer                                                      |
		| What is your age?                                 | Under 18                                                    |
		| What is your nationality?                         | British or Irish citizen                                    |
		| Are you in paid work?                             | No                                                          |
		| Does your household receive universal credit?     | Yes                                                         |
		| Do you get any of these benefits?                 | Carer's Allowance                                           |
		| Do you already get any of this childcare support? | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                       | No                                                          |
	When I click the Change link in the "Your details" summary list for "Are you in paid work?"
	And I select the "I am on leave from work" radio button
	And I click on Continue
	Then the page header is "TypeOfLeave"

Scenario: PaidWork change to No returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                             | Answer                                                      |
		| What is your age?                                    | Under 18                                                    |
		| What is your nationality?                            | British or Irish citizen                                    |
		| Are you in paid work?                                | Yes                                                         |
		| How would you describe your work status?             | Self-employed                                               |
		| Have you been self-employed for less than 12 months? | Yes                                                         |
		| Does your household receive universal credit?        | Yes                                                         |
		| Do you get any of these benefits?                    | Carer's Allowance                                           |
		| Do you already get any of this childcare support?    | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                          | No                                                          |
	When I click the Change link in the "Your details" summary list for "Are you in paid work?"
	And I select the "No" radio button
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "How would you describe your work status?"
	And I do not see a summary row "Have you been self-employed for less than 12 months?"

Scenario: PaidWork change to No opens Does your household receive universal credit?
	Given I answer questions as follows:
		| Question                                                | Answer                                                      |
		| What is your age?                                       | Under 18                                                    |
		| What is your nationality?                               | British or Irish citizen                                    |
		| Are you in paid work?                                   | Yes                                                         |
		| How would you describe your work status?                | Paid employment                                             |
		| On average, do you earn £203 a week or more before tax? | Yes                                                         |
		| Is your adjusted net income more than £100,000 a year?  | Yes                                                         |
		| Do you get any of these benefits?                       | Carer's Allowance                                           |
		| Do you already get any of this childcare support?       | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                             | No                                                          |
	When I click the Change link in the "Your details" summary list for "Are you in paid work?"
	And I select the "No" radio button
	And I click on Continue
	Then the page header is "Does your household receive universal credit?"

Scenario: PaidWork change to Yes opens How would you describe your work status?
	Given I answer questions as follows:
		| Question                                          | Answer                                                      |
		| What is your age?                                 | Under 18                                                    |
		| What is your nationality?                         | British or Irish citizen                                    |
		| Are you in paid work?                             | No                                                          |
		| Does your household receive universal credit?     | Yes                                                         |
		| Do you get any of these benefits?                 | Carer's Allowance                                           |
		| Do you already get any of this childcare support? | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                       | No                                                          |
	When I click the Change link in the "Your details" summary list for "Are you in paid work?"
	And I select the "Yes" radio button
	And I click on Continue
	Then the page header is "How would you describe your work status?"

@ignore
Scenario: WorkStatus change to Apprentice opens On average, do you earn £203 a week or more before tax?
	Given I answer questions as follows:
		| Question                                             | Answer                                                      |
		| What is your age?                                    | Under 18                                                    |
		| What is your nationality?                            | British or Irish citizen                                    |
		| Are you in paid work?                                | Yes                                                         |
		| How would you describe your work status?             | Self-employed                                               |
		| Have you been self-employed for less than 12 months? | Yes                                                         |
		| Does your household receive universal credit?        | Yes                                                         |
		| Do you get any of these benefits?                    | Carer's Allowance                                           |
		| Do you already get any of this childcare support?    | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                          | No                                                          |
	When I click the Change link in the "Your details" summary list for "How would you describe your work status?"
	And I unselect the "Self-employed" checkbox
	And I select the "Apprentice" checkbox
	And I click on Continue
	Then the page header is "On average, do you earn £203 a week or more before tax?"

@ignore
Scenario: WorkStatus change to Apprentice returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                                | Answer                                                      |
		| What is your age?                                       | Under 18                                                    |
		| What is your nationality?                               | British or Irish citizen                                    |
		| Are you in paid work?                                   | Yes                                                         |
		| How would you describe your work status?                | Self-employed                                               |
		| Have you been self-employed for less than 12 months?    | No                                                          |
		| On average, do you earn £203 a week or more before tax? | Yes                                                         |
		| Is your adjusted net income more than £100,000 a year?  | Yes                                                         |
		| Do you get any of these benefits?                       | Carer's Allowance                                           |
		| Do you already get any of this childcare support?       | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                             | No                                                          |
	When I click the Change link in the "Your details" summary list for "How would you describe your work status?"
	And I unselect the "Self-employed" checkbox
	And I select the "Apprentice" checkbox
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "Have you been self-employed for less than 12 months?"

@ignore
Scenario: WorkStatus change to Paid employment opens On average, do you earn £203 a week or more before tax?
	Given I answer questions as follows:
		| Question                                             | Answer                                                      |
		| What is your age?                                    | Under 18                                                    |
		| What is your nationality?                            | British or Irish citizen                                    |
		| Are you in paid work?                                | Yes                                                         |
		| How would you describe your work status?             | Self-employed                                               |
		| Have you been self-employed for less than 12 months? | Yes                                                         |
		| Does your household receive universal credit?        | Yes                                                         |
		| Do you get any of these benefits?                    | Carer's Allowance                                           |
		| Do you already get any of this childcare support?    | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                          | No                                                          |
	When I click the Change link in the "Your details" summary list for "How would you describe your work status?"
	And I unselect the "Self-employed" checkbox
	And I select the "Paid employment" checkbox
	And I click on Continue
	Then the page header is "On average, do you earn £203 a week or more before tax?"

@ignore
Scenario: WorkStatus change to Paid employment returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                                | Answer                                                      |
		| What is your age?                                       | Under 18                                                    |
		| What is your nationality?                               | British or Irish citizen                                    |
		| Are you in paid work?                                   | Yes                                                         |
		| How would you describe your work status?                | Self-employed                                               |
		| Have you been self-employed for less than 12 months?    | No                                                          |
		| On average, do you earn £203 a week or more before tax? | Yes                                                         |
		| Is your adjusted net income more than £100,000 a year?  | Yes                                                         |
		| Do you get any of these benefits?                       | Carer's Allowance                                           |
		| Do you already get any of this childcare support?       | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                             | No                                                          |
	When I click the Change link in the "Your details" summary list for "How would you describe your work status?"
	And I unselect the "Self-employed" checkbox
	And I select the "Paid employment" checkbox
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "Have you been self-employed for less than 12 months?"

@ignore
Scenario: WorkStatus change to Self-employed opens Have you been self-employed for less than 12 months?
	Given I answer questions as follows:
		| Question                                                | Answer                                                      |
		| What is your age?                                       | Under 18                                                    |
		| What is your nationality?                               | British or Irish citizen                                    |
		| Are you in paid work?                                   | Yes                                                         |
		| How would you describe your work status?                | Paid employment                                             |
		| On average, do you earn £203 a week or more before tax? | Yes                                                         |
		| Is your adjusted net income more than £100,000 a year?  | Yes                                                         |
		| Do you get any of these benefits?                       | Carer's Allowance                                           |
		| Do you already get any of this childcare support?       | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                             | No                                                          |
	When I click the Change link in the "Your details" summary list for "How would you describe your work status?"
	And I unselect the "Paid employment" checkbox
	And I select the "Self-employed" checkbox
	And I click on Continue
	Then the page header is "Have you been self-employed for less than 12 months?"

@ignore
Scenario: SelfEmployedDuration change to No opens On average, do you earn £203 a week or more before tax?
	Given I answer questions as follows:
		| Question                                             | Answer                                                      |
		| What is your age?                                    | Under 18                                                    |
		| What is your nationality?                            | British or Irish citizen                                    |
		| Are you in paid work?                                | Yes                                                         |
		| How would you describe your work status?             | Self-employed                                               |
		| Have you been self-employed for less than 12 months? | Yes                                                         |
		| Does your household receive universal credit?        | Yes                                                         |
		| Do you get any of these benefits?                    | Carer's Allowance                                           |
		| Do you already get any of this childcare support?    | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                          | No                                                          |
	When I click the Change link in the "Your details" summary list for "Have you been self-employed for less than 12 months?"
	And I select the "No" radio button
	And I click on Continue
	Then the page header is "On average, do you earn £203 a week or more before tax?"

@ignore
Scenario: SelfEmployedDuration change to Yes opens Does your household receive universal credit?
	Given I answer questions as follows:
		| Question                                                | Answer                                                      |
		| What is your age?                                       | Under 18                                                    |
		| What is your nationality?                               | British or Irish citizen                                    |
		| Are you in paid work?                                   | Yes                                                         |
		| How would you describe your work status?                | Self-employed                                               |
		| Have you been self-employed for less than 12 months?    | No                                                          |
		| On average, do you earn £203 a week or more before tax? | Yes                                                         |
		| Is your adjusted net income more than £100,000 a year?  | Yes                                                         |
		| Do you get any of these benefits?                       | Carer's Allowance                                           |
		| Do you already get any of this childcare support?       | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                             | No                                                          |
	When I click the Change link in the "Your details" summary list for "Have you been self-employed for less than 12 months?"
	And I select the "Yes" radio button
	And I click on Continue
	Then the page header is "Does your household receive universal credit?"

@ignore
Scenario: SelfEmployedDuration change to Yes returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                                | Answer                                                      |
		| What is your age?                                       | Under 18                                                    |
		| What is your nationality?                               | British or Irish citizen                                    |
		| Are you in paid work?                                   | Yes                                                         |
		| How would you describe your work status?                | Self-employed                                               |
		| Have you been self-employed for less than 12 months?    | No                                                          |
		| On average, do you earn £203 a week or more before tax? | No                                                          |
		| Does your household receive universal credit?           | Yes                                                         |
		| Do you get any of these benefits?                       | Carer's Allowance                                           |
		| Do you already get any of this childcare support?       | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                             | No                                                          |
	When I click the Change link in the "Your details" summary list for "Have you been self-employed for less than 12 months?"
	And I select the "Yes" radio button
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "On average, do you earn £203 a week or more before tax?"

@ignore
Scenario: WeeklyEarnings change to No opens Does your household receive universal credit?
	Given I answer questions as follows:
		| Question                                                | Answer                                                      |
		| What is your age?                                       | Under 18                                                    |
		| What is your nationality?                               | British or Irish citizen                                    |
		| Are you in paid work?                                   | Yes                                                         |
		| How would you describe your work status?                | Paid employment                                             |
		| On average, do you earn £203 a week or more before tax? | Yes                                                         |
		| Is your adjusted net income more than £100,000 a year?  | Yes                                                         |
		| Do you get any of these benefits?                       | Carer's Allowance                                           |
		| Do you already get any of this childcare support?       | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                             | No                                                          |
	When I click the Change link in the "Your details" summary list for "On average, do you earn £203 a week or more before tax?"
	And I select the "No" radio button
	And I click on Continue
	Then the page header is "Does your household receive universal credit?"

@ignore
Scenario: WeeklyEarnings change to No returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                                | Answer                                                      |
		| What is your age?                                       | Under 18                                                    |
		| What is your nationality?                               | British or Irish citizen                                    |
		| Are you in paid work?                                   | Yes                                                         |
		| How would you describe your work status?                | Paid employment                                             |
		| On average, do you earn £203 a week or more before tax? | Yes                                                         |
		| Is your adjusted net income more than £100,000 a year?  | No                                                          |
		| Does your household receive universal credit?           | Yes                                                         |
		| Do you get any of these benefits?                       | Carer's Allowance                                           |
		| Do you already get any of this childcare support?       | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                             | No                                                          |
	When I click the Change link in the "Your details" summary list for "On average, do you earn £203 a week or more before tax?"
	And I select the "No" radio button
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "Is your adjusted net income more than £100,000 a year?"

@ignore
Scenario: WeeklyEarnings change to Yes opens Is your adjusted net income more than £100,000 a year?
	Given I answer questions as follows:
		| Question                                                | Answer                                                      |
		| What is your age?                                       | Under 18                                                    |
		| What is your nationality?                               | British or Irish citizen                                    |
		| Are you in paid work?                                   | Yes                                                         |
		| How would you describe your work status?                | Paid employment                                             |
		| On average, do you earn £203 a week or more before tax? | No                                                          |
		| Does your household receive universal credit?           | Yes                                                         |
		| Do you get any of these benefits?                       | Carer's Allowance                                           |
		| Do you already get any of this childcare support?       | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                             | No                                                          |
	When I click the Change link in the "Your details" summary list for "On average, do you earn £203 a week or more before tax?"
	And I select the "Yes" radio button
	And I click on Continue
	Then the page header is "Is your adjusted net income more than £100,000 a year?"

@ignore
Scenario: YearlyEarnings change to No opens Does your household receive universal credit?
	Given I answer questions as follows:
		| Question                                                | Answer                                                      |
		| What is your age?                                       | Under 18                                                    |
		| What is your nationality?                               | British or Irish citizen                                    |
		| Are you in paid work?                                   | Yes                                                         |
		| How would you describe your work status?                | Paid employment                                             |
		| On average, do you earn £203 a week or more before tax? | Yes                                                         |
		| Is your adjusted net income more than £100,000 a year?  | Yes                                                         |
		| Do you get any of these benefits?                       | Carer's Allowance                                           |
		| Do you already get any of this childcare support?       | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                             | No                                                          |
	When I click the Change link in the "Your details" summary list for "Is your adjusted net income more than £100,000 a year?"
	And I select the "No" radio button
	And I click on Continue
	Then the page header is "Does your household receive universal credit?"

@ignore
Scenario: YearlyEarnings change to Yes returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                                | Answer                                                      |
		| What is your age?                                       | Under 18                                                    |
		| What is your nationality?                               | British or Irish citizen                                    |
		| Are you in paid work?                                   | Yes                                                         |
		| How would you describe your work status?                | Paid employment                                             |
		| On average, do you earn £203 a week or more before tax? | Yes                                                         |
		| Is your adjusted net income more than £100,000 a year?  | No                                                          |
		| Does your household receive universal credit?           | Yes                                                         |
		| Do you get any of these benefits?                       | Carer's Allowance                                           |
		| Do you already get any of this childcare support?       | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                             | No                                                          |
	When I click the Change link in the "Your details" summary list for "Is your adjusted net income more than £100,000 a year?"
	And I select the "Yes" radio button
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "Does your household receive universal credit?"

@ignore
Scenario: ChildcareSupport change to A childcare bursary or grant (as part of education funding) returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                          | Answer                     |
		| What is your age?                                 | Under 18                   |
		| What is your nationality?                         | British or Irish citizen   |
		| Are you in paid work?                             | No                         |
		| Does your household receive universal credit?     | Yes                        |
		| Do you get any of these benefits?                 | Carer's Allowance          |
		| Do you already get any of this childcare support? | Childcare vouchers         |
		| How do you receive your childcare vouchers?       | A workplace nursery scheme |
		| Do you live with a partner?                       | No                         |
	When I click the Change link in the "Your details" summary list for "Do you already get any of this childcare support?"
	And I unselect the "Childcare vouchers" checkbox
	And I select the "A childcare bursary or grant (as part of education funding)" checkbox
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "How do you receive your childcare vouchers?"

@ignore
Scenario: ChildcareSupport change to Childcare vouchers opens How do you receive your childcare vouchers?
	Given I answer questions as follows:
		| Question                                          | Answer                                                      |
		| What is your age?                                 | Under 18                                                    |
		| What is your nationality?                         | British or Irish citizen                                    |
		| Are you in paid work?                             | No                                                          |
		| Does your household receive universal credit?     | Yes                                                         |
		| Do you get any of these benefits?                 | Carer's Allowance                                           |
		| Do you already get any of this childcare support? | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                       | No                                                          |
	When I click the Change link in the "Your details" summary list for "Do you already get any of this childcare support?"
	And I unselect the "A childcare bursary or grant (as part of education funding)" checkbox
	And I select the "Childcare vouchers" checkbox
	And I click on Continue
	Then the page header is "How do you receive your childcare vouchers?"

@ignore
Scenario: ChildcareSupport change to No, I do not get any of this childcare support returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                          | Answer                     |
		| What is your age?                                 | Under 18                   |
		| What is your nationality?                         | British or Irish citizen   |
		| Are you in paid work?                             | No                         |
		| Does your household receive universal credit?     | Yes                        |
		| Do you get any of these benefits?                 | Carer's Allowance          |
		| Do you already get any of this childcare support? | Childcare vouchers         |
		| How do you receive your childcare vouchers?       | A workplace nursery scheme |
		| Do you live with a partner?                       | No                         |
	When I click the Change link in the "Your details" summary list for "Do you already get any of this childcare support?"
	And I unselect the "Childcare vouchers" checkbox
	And I select the "No, I do not get any of this childcare support" checkbox
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "How do you receive your childcare vouchers?"

@ignore
Scenario: HasPartner change to No returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                                     | Answer                                                      |
		| What is your age?                                            | Under 18                                                    |
		| What is your nationality?                                    | British or Irish citizen                                    |
		| Are you in paid work?                                        | No                                                          |
		| Does your household receive universal credit?                | Yes                                                         |
		| Do you get any of these benefits?                            | Carer's Allowance                                           |
		| Do you already get any of this childcare support?            | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                  | Yes                                                         |
		| What is your partner's age?                                  | Under 18                                                    |
		| Which of these best describes your partners nationality?     | British or Irish citizen                                    |
		| Is your partner in paid work?                                | No                                                          |
		| Does your partner get any of these benefits?                 | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support? | A childcare bursary or grant (as part of education funding) |
	When I click the Change link in the "Your details" summary list for "Do you live with a partner?"
	And I select the "No" radio button
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary list for "Your partners details"

@ignore
Scenario: HasPartner change to Yes opens What is your partner's age?
	Given I answer questions as follows:
		| Question                                          | Answer                                                      |
		| What is your age?                                 | Under 18                                                    |
		| What is your nationality?                         | British or Irish citizen                                    |
		| Are you in paid work?                             | No                                                          |
		| Does your household receive universal credit?     | Yes                                                         |
		| Do you get any of these benefits?                 | Carer's Allowance                                           |
		| Do you already get any of this childcare support? | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                       | No                                                          |
	When I click the Change link in the "Your details" summary list for "Do you live with a partner?"
	And I select the "Yes" radio button
	And I click on Continue
	Then the page header is "What is your partner's age?"

@ignore
Scenario: PartnerNationality change to British or Irish citizen returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                                                             | Answer                                                      |
		| What is your age?                                                                    | Under 18                                                    |
		| What is your nationality?                                                            | British or Irish citizen                                    |
		| Are you in paid work?                                                                | No                                                          |
		| Does your household receive universal credit?                                        | Yes                                                         |
		| Do you get any of these benefits?                                                    | Carer's Allowance                                           |
		| Do you already get any of this childcare support?                                    | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                                          | Yes                                                         |
		| What is your partner's age?                                                          | Under 18                                                    |
		| Which of these best describes your partners nationality?                             | Citizen of an EU country, EEA country or Switzerland        |
		| Does your partner have settled or pre-settled status under the EU Settlement Scheme? | Yes                                                         |
		| Is your partner in paid work?                                                        | No                                                          |
		| Does your partner get any of these benefits?                                         | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support?                         | A childcare bursary or grant (as part of education funding) |
	When I click the Change link in the "Your partners details" summary list for "Which of these best describes your partners nationality?"
	And I select the "British or Irish citizen" radio button
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "Does your partner have settled or pre-settled status under the EU Settlement Scheme?"

@ignore
Scenario: PartnerNationality change to Citizen of a different country returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                                                             | Answer                                                      |
		| What is your age?                                                                    | Under 18                                                    |
		| What is your nationality?                                                            | British or Irish citizen                                    |
		| Are you in paid work?                                                                | No                                                          |
		| Does your household receive universal credit?                                        | Yes                                                         |
		| Do you get any of these benefits?                                                    | Carer's Allowance                                           |
		| Do you already get any of this childcare support?                                    | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                                          | Yes                                                         |
		| What is your partner's age?                                                          | Under 18                                                    |
		| Which of these best describes your partners nationality?                             | Citizen of an EU country, EEA country or Switzerland        |
		| Does your partner have settled or pre-settled status under the EU Settlement Scheme? | Yes                                                         |
		| Is your partner in paid work?                                                        | No                                                          |
		| Does your partner get any of these benefits?                                         | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support?                         | A childcare bursary or grant (as part of education funding) |
	When I click the Change link in the "Your partners details" summary list for "Which of these best describes your partners nationality?"
	And I select the "Citizen of a different country" radio button
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "Does your partner have settled or pre-settled status under the EU Settlement Scheme?"

@ignore
Scenario: PartnerNationality change to Citizen of an EU country, EEA country or Switzerland opens Does your partner have settled or pre-settled status under the EU Settlement Scheme?
	Given I answer questions as follows:
		| Question                                                     | Answer                                                      |
		| What is your age?                                            | Under 18                                                    |
		| What is your nationality?                                    | British or Irish citizen                                    |
		| Are you in paid work?                                        | No                                                          |
		| Does your household receive universal credit?                | Yes                                                         |
		| Do you get any of these benefits?                            | Carer's Allowance                                           |
		| Do you already get any of this childcare support?            | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                  | Yes                                                         |
		| What is your partner's age?                                  | Under 18                                                    |
		| Which of these best describes your partners nationality?     | British or Irish citizen                                    |
		| Is your partner in paid work?                                | No                                                          |
		| Does your partner get any of these benefits?                 | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support? | A childcare bursary or grant (as part of education funding) |
	When I click the Change link in the "Your partners details" summary list for "Which of these best describes your partners nationality?"
	And I select the "Citizen of an EU country, EEA country or Switzerland" radio button
	And I click on Continue
	Then the page header is "Does your partner have settled or pre-settled status under the EU Settlement Scheme?"

@ignore
Scenario: PartnerPaidWork change to No returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                                     | Answer                                                      |
		| What is your age?                                            | Under 18                                                    |
		| What is your nationality?                                    | British or Irish citizen                                    |
		| Are you in paid work?                                        | No                                                          |
		| Does your household receive universal credit?                | Yes                                                         |
		| Do you get any of these benefits?                            | Carer's Allowance                                           |
		| Do you already get any of this childcare support?            | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                  | Yes                                                         |
		| What is your partner's age?                                  | Under 18                                                    |
		| Which of these best describes your partners nationality?     | British or Irish citizen                                    |
		| Is your partner in paid work?                                | Yes                                                         |
		| How would you describe your partner's work status?           | Self-employed                                               |
		| Has your partner been self-employed for less than 12 months? | Yes                                                         |
		| Does your partner get any of these benefits?                 | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support? | A childcare bursary or grant (as part of education funding) |
	When I click the Change link in the "Your partners details" summary list for "Is your partner in paid work?"
	And I select the "No" radio button
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "How would you describe your partner's work status?"
	And I do not see a summary row "Has your partner been self-employed for less than 12 months?"

@ignore
Scenario: PartnerPaidWork change to They are on leave from work opens PartnerTypeOfLeave
	Given I answer questions as follows:
		| Question                                                     | Answer                                                      |
		| What is your age?                                            | Under 18                                                    |
		| What is your nationality?                                    | British or Irish citizen                                    |
		| Are you in paid work?                                        | No                                                          |
		| Does your household receive universal credit?                | Yes                                                         |
		| Do you get any of these benefits?                            | Carer's Allowance                                           |
		| Do you already get any of this childcare support?            | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                  | Yes                                                         |
		| What is your partner's age?                                  | Under 18                                                    |
		| Which of these best describes your partners nationality?     | British or Irish citizen                                    |
		| Is your partner in paid work?                                | No                                                          |
		| Does your partner get any of these benefits?                 | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support? | A childcare bursary or grant (as part of education funding) |
	When I click the Change link in the "Your partners details" summary list for "Is your partner in paid work?"
	And I select the "They are on leave from work" radio button
	And I click on Continue
	Then the page header is "PartnerTypeOfLeave"

@ignore
Scenario: PartnerPaidWork change to Yes opens How would you describe your partner's work status?
	Given I answer questions as follows:
		| Question                                                     | Answer                                                      |
		| What is your age?                                            | Under 18                                                    |
		| What is your nationality?                                    | British or Irish citizen                                    |
		| Are you in paid work?                                        | No                                                          |
		| Does your household receive universal credit?                | Yes                                                         |
		| Do you get any of these benefits?                            | Carer's Allowance                                           |
		| Do you already get any of this childcare support?            | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                  | Yes                                                         |
		| What is your partner's age?                                  | Under 18                                                    |
		| Which of these best describes your partners nationality?     | British or Irish citizen                                    |
		| Is your partner in paid work?                                | No                                                          |
		| Does your partner get any of these benefits?                 | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support? | A childcare bursary or grant (as part of education funding) |
	When I click the Change link in the "Your partners details" summary list for "Is your partner in paid work?"
	And I select the "Yes" radio button
	And I click on Continue
	Then the page header is "How would you describe your partner's work status?"

@ignore
Scenario: PartnerWorkStatus change to Apprentice opens On average, does your partner earn £203 a week or more before tax?
	Given I answer questions as follows:
		| Question                                                     | Answer                                                      |
		| What is your age?                                            | Under 18                                                    |
		| What is your nationality?                                    | British or Irish citizen                                    |
		| Are you in paid work?                                        | No                                                          |
		| Does your household receive universal credit?                | Yes                                                         |
		| Do you get any of these benefits?                            | Carer's Allowance                                           |
		| Do you already get any of this childcare support?            | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                  | Yes                                                         |
		| What is your partner's age?                                  | Under 18                                                    |
		| Which of these best describes your partners nationality?     | British or Irish citizen                                    |
		| Is your partner in paid work?                                | Yes                                                         |
		| How would you describe your partner's work status?           | Self-employed                                               |
		| Has your partner been self-employed for less than 12 months? | Yes                                                         |
		| Does your partner get any of these benefits?                 | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support? | A childcare bursary or grant (as part of education funding) |
	When I click the Change link in the "Your partners details" summary list for "How would you describe your partner's work status?"
	And I unselect the "Self-employed" checkbox
	And I select the "Apprentice" checkbox
	And I click on Continue
	Then the page header is "On average, does your partner earn £203 a week or more before tax?"

@ignore
Scenario: PartnerWorkStatus change to Apprentice returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                                           | Answer                                                      |
		| What is your age?                                                  | Under 18                                                    |
		| What is your nationality?                                          | British or Irish citizen                                    |
		| Are you in paid work?                                              | No                                                          |
		| Does your household receive universal credit?                      | Yes                                                         |
		| Do you get any of these benefits?                                  | Carer's Allowance                                           |
		| Do you already get any of this childcare support?                  | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                        | Yes                                                         |
		| What is your partner's age?                                        | Under 18                                                    |
		| Which of these best describes your partners nationality?           | British or Irish citizen                                    |
		| Is your partner in paid work?                                      | Yes                                                         |
		| How would you describe your partner's work status?                 | Self-employed                                               |
		| Has your partner been self-employed for less than 12 months?       | No                                                          |
		| On average, does your partner earn £203 a week or more before tax? | No                                                          |
		| Does your partner get any of these benefits?                       | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support?       | A childcare bursary or grant (as part of education funding) |
	When I click the Change link in the "Your partners details" summary list for "How would you describe your partner's work status?"
	And I unselect the "Self-employed" checkbox
	And I select the "Apprentice" checkbox
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "Has your partner been self-employed for less than 12 months?"

@ignore
Scenario: PartnerWorkStatus change to Paid employment opens On average, does your partner earn £203 a week or more before tax?
	Given I answer questions as follows:
		| Question                                                     | Answer                                                      |
		| What is your age?                                            | Under 18                                                    |
		| What is your nationality?                                    | British or Irish citizen                                    |
		| Are you in paid work?                                        | No                                                          |
		| Does your household receive universal credit?                | Yes                                                         |
		| Do you get any of these benefits?                            | Carer's Allowance                                           |
		| Do you already get any of this childcare support?            | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                  | Yes                                                         |
		| What is your partner's age?                                  | Under 18                                                    |
		| Which of these best describes your partners nationality?     | British or Irish citizen                                    |
		| Is your partner in paid work?                                | Yes                                                         |
		| How would you describe your partner's work status?           | Self-employed                                               |
		| Has your partner been self-employed for less than 12 months? | Yes                                                         |
		| Does your partner get any of these benefits?                 | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support? | A childcare bursary or grant (as part of education funding) |
	When I click the Change link in the "Your partners details" summary list for "How would you describe your partner's work status?"
	And I unselect the "Self-employed" checkbox
	And I select the "Paid employment" checkbox
	And I click on Continue
	Then the page header is "On average, does your partner earn £203 a week or more before tax?"

@ignore
Scenario: PartnerWorkStatus change to Paid employment returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                                           | Answer                                                      |
		| What is your age?                                                  | Under 18                                                    |
		| What is your nationality?                                          | British or Irish citizen                                    |
		| Are you in paid work?                                              | No                                                          |
		| Does your household receive universal credit?                      | Yes                                                         |
		| Do you get any of these benefits?                                  | Carer's Allowance                                           |
		| Do you already get any of this childcare support?                  | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                        | Yes                                                         |
		| What is your partner's age?                                        | Under 18                                                    |
		| Which of these best describes your partners nationality?           | British or Irish citizen                                    |
		| Is your partner in paid work?                                      | Yes                                                         |
		| How would you describe your partner's work status?                 | Self-employed                                               |
		| Has your partner been self-employed for less than 12 months?       | No                                                          |
		| On average, does your partner earn £203 a week or more before tax? | No                                                          |
		| Does your partner get any of these benefits?                       | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support?       | A childcare bursary or grant (as part of education funding) |
	When I click the Change link in the "Your partners details" summary list for "How would you describe your partner's work status?"
	And I unselect the "Self-employed" checkbox
	And I select the "Paid employment" checkbox
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "Has your partner been self-employed for less than 12 months?"

@ignore
Scenario: PartnerWorkStatus change to Self-employed opens Has your partner been self-employed for less than 12 months?
	Given I answer questions as follows:
		| Question                                                           | Answer                                                      |
		| What is your age?                                                  | Under 18                                                    |
		| What is your nationality?                                          | British or Irish citizen                                    |
		| Are you in paid work?                                              | No                                                          |
		| Does your household receive universal credit?                      | Yes                                                         |
		| Do you get any of these benefits?                                  | Carer's Allowance                                           |
		| Do you already get any of this childcare support?                  | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                        | Yes                                                         |
		| What is your partner's age?                                        | Under 18                                                    |
		| Which of these best describes your partners nationality?           | British or Irish citizen                                    |
		| Is your partner in paid work?                                      | Yes                                                         |
		| How would you describe your partner's work status?                 | Paid employment                                             |
		| On average, does your partner earn £203 a week or more before tax? | No                                                          |
		| Does your partner get any of these benefits?                       | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support?       | A childcare bursary or grant (as part of education funding) |
	When I click the Change link in the "Your partners details" summary list for "How would you describe your partner's work status?"
	And I unselect the "Paid employment" checkbox
	And I select the "Self-employed" checkbox
	And I click on Continue
	Then the page header is "Has your partner been self-employed for less than 12 months?"

@ignore
Scenario: PartnerSelfEmployedDuration change to No opens On average, does your partner earn £203 a week or more before tax?
	Given I answer questions as follows:
		| Question                                                     | Answer                                                      |
		| What is your age?                                            | Under 18                                                    |
		| What is your nationality?                                    | British or Irish citizen                                    |
		| Are you in paid work?                                        | No                                                          |
		| Does your household receive universal credit?                | Yes                                                         |
		| Do you get any of these benefits?                            | Carer's Allowance                                           |
		| Do you already get any of this childcare support?            | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                  | Yes                                                         |
		| What is your partner's age?                                  | Under 18                                                    |
		| Which of these best describes your partners nationality?     | British or Irish citizen                                    |
		| Is your partner in paid work?                                | Yes                                                         |
		| How would you describe your partner's work status?           | Self-employed                                               |
		| Has your partner been self-employed for less than 12 months? | Yes                                                         |
		| Does your partner get any of these benefits?                 | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support? | A childcare bursary or grant (as part of education funding) |
	When I click the Change link in the "Your partners details" summary list for "Has your partner been self-employed for less than 12 months?"
	And I select the "No" radio button
	And I click on Continue
	Then the page header is "On average, does your partner earn £203 a week or more before tax?"

@ignore
Scenario: PartnerSelfEmployedDuration change to Yes returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                                           | Answer                                                      |
		| What is your age?                                                  | Under 18                                                    |
		| What is your nationality?                                          | British or Irish citizen                                    |
		| Are you in paid work?                                              | No                                                          |
		| Does your household receive universal credit?                      | Yes                                                         |
		| Do you get any of these benefits?                                  | Carer's Allowance                                           |
		| Do you already get any of this childcare support?                  | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                        | Yes                                                         |
		| What is your partner's age?                                        | Under 18                                                    |
		| Which of these best describes your partners nationality?           | British or Irish citizen                                    |
		| Is your partner in paid work?                                      | Yes                                                         |
		| How would you describe your partner's work status?                 | Self-employed                                               |
		| Has your partner been self-employed for less than 12 months?       | No                                                          |
		| On average, does your partner earn £203 a week or more before tax? | No                                                          |
		| Does your partner get any of these benefits?                       | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support?       | A childcare bursary or grant (as part of education funding) |
	When I click the Change link in the "Your partners details" summary list for "Has your partner been self-employed for less than 12 months?"
	And I select the "Yes" radio button
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "On average, does your partner earn £203 a week or more before tax?"

@ignore
Scenario: PartnerWeeklyEarnings change to No returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                                           | Answer                                                      |
		| What is your age?                                                  | Under 18                                                    |
		| What is your nationality?                                          | British or Irish citizen                                    |
		| Are you in paid work?                                              | No                                                          |
		| Does your household receive universal credit?                      | Yes                                                         |
		| Do you get any of these benefits?                                  | Carer's Allowance                                           |
		| Do you already get any of this childcare support?                  | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                        | Yes                                                         |
		| What is your partner's age?                                        | Under 18                                                    |
		| Which of these best describes your partners nationality?           | British or Irish citizen                                    |
		| Is your partner in paid work?                                      | Yes                                                         |
		| How would you describe your partner's work status?                 | Paid employment                                             |
		| On average, does your partner earn £203 a week or more before tax? | Yes                                                         |
		| Is your partner's adjusted net income more than £100,000 a year?   | Yes                                                         |
		| Does your partner get any of these benefits?                       | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support?       | A childcare bursary or grant (as part of education funding) |
	When I click the Change link in the "Your partners details" summary list for "On average, does your partner earn £203 a week or more before tax?"
	And I select the "No" radio button
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "Is your partner's adjusted net income more than £100,000 a year?"

@ignore
Scenario: PartnerWeeklyEarnings change to Yes opens Is your partner's adjusted net income more than £100,000 a year?
	Given I answer questions as follows:
		| Question                                                           | Answer                                                      |
		| What is your age?                                                  | Under 18                                                    |
		| What is your nationality?                                          | British or Irish citizen                                    |
		| Are you in paid work?                                              | No                                                          |
		| Does your household receive universal credit?                      | Yes                                                         |
		| Do you get any of these benefits?                                  | Carer's Allowance                                           |
		| Do you already get any of this childcare support?                  | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                        | Yes                                                         |
		| What is your partner's age?                                        | Under 18                                                    |
		| Which of these best describes your partners nationality?           | British or Irish citizen                                    |
		| Is your partner in paid work?                                      | Yes                                                         |
		| How would you describe your partner's work status?                 | Paid employment                                             |
		| On average, does your partner earn £203 a week or more before tax? | No                                                          |
		| Does your partner get any of these benefits?                       | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support?       | A childcare bursary or grant (as part of education funding) |
	When I click the Change link in the "Your partners details" summary list for "On average, does your partner earn £203 a week or more before tax?"
	And I select the "Yes" radio button
	And I click on Continue
	Then the page header is "Is your partner's adjusted net income more than £100,000 a year?"

@ignore
Scenario: PartnerChildcareSupport change to A childcare bursary or grant (as part of education funding) returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                                     | Answer                                                      |
		| What is your age?                                            | Under 18                                                    |
		| What is your nationality?                                    | British or Irish citizen                                    |
		| Are you in paid work?                                        | No                                                          |
		| Does your household receive universal credit?                | Yes                                                         |
		| Do you get any of these benefits?                            | Carer's Allowance                                           |
		| Do you already get any of this childcare support?            | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                  | Yes                                                         |
		| What is your partner's age?                                  | Under 18                                                    |
		| Which of these best describes your partners nationality?     | British or Irish citizen                                    |
		| Is your partner in paid work?                                | No                                                          |
		| Does your partner get any of these benefits?                 | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support? | Childcare vouchers                                          |
		| How does your partner receive childcare vouchers?            | A workplace nursery scheme                                  |
	When I click the Change link in the "Your partners details" summary list for "Does your partner already get any of this childcare support?"
	And I unselect the "Childcare vouchers" checkbox
	And I select the "A childcare bursary or grant (as part of education funding)" checkbox
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "How does your partner receive childcare vouchers?"

@ignore
Scenario: PartnerChildcareSupport change to Childcare vouchers opens How does your partner receive childcare vouchers?
	Given I answer questions as follows:
		| Question                                                     | Answer                                                      |
		| What is your age?                                            | Under 18                                                    |
		| What is your nationality?                                    | British or Irish citizen                                    |
		| Are you in paid work?                                        | No                                                          |
		| Does your household receive universal credit?                | Yes                                                         |
		| Do you get any of these benefits?                            | Carer's Allowance                                           |
		| Do you already get any of this childcare support?            | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                  | Yes                                                         |
		| What is your partner's age?                                  | Under 18                                                    |
		| Which of these best describes your partners nationality?     | British or Irish citizen                                    |
		| Is your partner in paid work?                                | No                                                          |
		| Does your partner get any of these benefits?                 | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support? | A childcare bursary or grant (as part of education funding) |
	When I click the Change link in the "Your partners details" summary list for "Does your partner already get any of this childcare support?"
	And I unselect the "A childcare bursary or grant (as part of education funding)" checkbox
	And I select the "Childcare vouchers" checkbox
	And I click on Continue
	Then the page header is "How does your partner receive childcare vouchers?"

@ignore
Scenario: PartnerChildcareSupport change to No, they do not get any of this childcare support returns to summary and closes stale answers
	Given I answer questions as follows:
		| Question                                                     | Answer                                                      |
		| What is your age?                                            | Under 18                                                    |
		| What is your nationality?                                    | British or Irish citizen                                    |
		| Are you in paid work?                                        | No                                                          |
		| Does your household receive universal credit?                | Yes                                                         |
		| Do you get any of these benefits?                            | Carer's Allowance                                           |
		| Do you already get any of this childcare support?            | A childcare bursary or grant (as part of education funding) |
		| Do you live with a partner?                                  | Yes                                                         |
		| What is your partner's age?                                  | Under 18                                                    |
		| Which of these best describes your partners nationality?     | British or Irish citizen                                    |
		| Is your partner in paid work?                                | No                                                          |
		| Does your partner get any of these benefits?                 | Carer's Allowance                                           |
		| Does your partner already get any of this childcare support? | Childcare vouchers                                          |
		| How does your partner receive childcare vouchers?            | A workplace nursery scheme                                  |
	When I click the Change link in the "Your partners details" summary list for "Does your partner already get any of this childcare support?"
	And I unselect the "Childcare vouchers" checkbox
	And I select the "No, they do not get any of this childcare support" checkbox
	And I click on Continue
	Then the page header is "Check your answers"
	And I do not see a summary row "How does your partner receive childcare vouchers?"
