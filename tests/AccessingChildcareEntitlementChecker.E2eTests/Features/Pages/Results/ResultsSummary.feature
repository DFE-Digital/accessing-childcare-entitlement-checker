Feature: Results Summary

Scenario: Results summary does not show a public funds warning if i'm a British or Irish citizen
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I answer questions as follows:
		| Question                                          | Answer                                         |
		| What is your age?                                 | Under 18                                       |
		| What is your nationality?                         | British or Irish citizen                       |
		| Are you in paid work?                             | No, I am not in work                           |
		| Does your household receive universal credit?     | No                                             |
		| Do you get any of these benefits?                 | No, I do not get any of these benefits         |
		| Do you already get any of this childcare support? | No, I do not get any of this childcare support |
		| Do you live with a partner?                       | No                                             |
	When I click on Continue
	Then the page header is "Childcare support you could get"
	And I cannot see the public funds warning

Scenario: Results summary shows a public funds warning if i'm from a different country
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I answer questions as follows:
		| Question                                          | Answer                                         |
		| What is your age?                                 | Under 18                                       |
		| What is your nationality?                         | Citizen of a different country                 |
		| Are you in paid work?                             | No, I am not in work                           |
		| Does your household receive universal credit?     | No                                             |
		| Do you get any of these benefits?                 | No, I do not get any of these benefits         |
		| Do you already get any of this childcare support? | No, I do not get any of this childcare support |
		| Do you live with a partner?                       | No                                             |
	When I click on Continue
	Then the page header is "Childcare support you could get"
	And I can see the public funds warning

Scenario Outline: Results summary may show a public funds warning if i'm from the EU depending on my settled status
	Given I am on the childcare entitlement checker website
	And I start the journey, filling in Aydin's and Sara's details
	And I check my children's details and click on Continue
	And I answer questions as follows:
		| Question                                                                  | Answer                                               |
		| What is your age?                                                         | Under 18                                             |
		| What is your nationality?                                                 | Citizen of an EU country, EEA country or Switzerland |
		| Do you have settled or pre-settled status under the EU Settlement Scheme? | <Settled Status>                                     |
		| Are you in paid work?                                                     | No, I am not in work                                 |
		| Does your household receive universal credit?                             | No                                                   |
		| Do you get any of these benefits?                                         | No, I do not get any of these benefits               |
		| Do you already get any of this childcare support?                         | No, I do not get any of this childcare support       |
		| Do you live with a partner?                                               | No                                                   |
	When I click on Continue
	Then the page header is "Childcare support you could get"
	And I <Can Or Cannot> see the public funds warning

Examples:
	| Settled Status                                                   | Can Or Cannot |
	| Yes                                                              | cannot        |
	| No                                                               | can           |
	| I applied before 1 July 2021 and am still waiting for a decision | cannot        |
