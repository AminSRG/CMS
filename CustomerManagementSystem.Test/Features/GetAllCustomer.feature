Feature: GetAllCustomers

Scenario: Get all customers
	Given I have some existing customers
	When I send a request to get all customers
	Then the response should indicate success and contain a list of customers
