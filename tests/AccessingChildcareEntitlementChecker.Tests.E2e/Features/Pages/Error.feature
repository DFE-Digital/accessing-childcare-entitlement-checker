Feature: Error handling

Scenario: Not Found
    Given I visit a non-existent page
    Then the page header is "Page not found"
    And I am not redirected to another page
    And the HTTP status code is 404 Not Found

Scenario: Error
    Given I visit the development-only error test page
    Then the page header is "Sorry, there is a problem with the service"
    And I am not redirected to another page
    And the HTTP status code is 500 Internal Server Error
