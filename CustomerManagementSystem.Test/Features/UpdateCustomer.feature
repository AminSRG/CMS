Feature: Update Customer
  As a system user
  I want to update a customer's information
  So that I can keep the customer data up-to-date

Scenario: Update Customer's Name
	Given I have valid customer data
	And I create a customer with the valid data
	When I update the customer's email to "NewEmail.update@test.com"
	And I send a request to update the customer
	Then the update response should indicate success