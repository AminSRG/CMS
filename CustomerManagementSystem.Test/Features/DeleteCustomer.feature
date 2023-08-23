Feature: Delete Customer

Scenario: Delete an existing customer
	Given I have an existing customer
	When I send a request to delete the customer
	Then the delete response should indicate success

