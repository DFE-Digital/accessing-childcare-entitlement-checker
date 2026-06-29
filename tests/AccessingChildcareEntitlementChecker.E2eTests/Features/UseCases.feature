Feature: End to End Use Cases

Background:
	Given I am on the childcare entitlement checker website
	And I click the Start now link

Scenario: Eligible single parent with 2 children
	Given I complete the journey for the use case "Single parent with 2 children (Eligible)"
	Then the page header is "Check your answers"
	And I should see 2 summary cards
	And I should see a summary list for "Your details" with the following summary:
		| Question                                                | Answer                   |
		| Where do you live?                                      | England                  |
		| What is your age?                                       | 21 or over               |
		| What is your nationality?                               | British or Irish citizen |
		| Do you live with a partner?                             | No                       |
		| Are you in paid work?                                   | Yes                      |
		| How would you describe your work status?                | Paid employment          |
		| On average, do you earn £203 a week or more before tax? | Yes                      |
		| Is your adjusted net income more than £100,000 a year?  | No                       |
