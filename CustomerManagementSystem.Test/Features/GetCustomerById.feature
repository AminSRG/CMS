Feature: GetCustomerById

Scenario: Get an existing customer by ID
	Given I have an existing customer
	When I send a request to get the customer by ID
	Then the response should indicate success & return customer
