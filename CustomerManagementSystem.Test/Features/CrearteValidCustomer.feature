Feature: CrearteValidCustomer
  As a system user
  I want to create a valid customer
  So that I can add them to the system

@tag1
Scenario: Create a Valid Customer
	Given I have valid customer data
	When I send a request to create a customer
	Then the response should indicate success

@tag2
Scenario: Create an Invalid Customer
	Given I have invalid customer data
	When I send a request to create a customer
	Then the response should indicate a validation error with status 400

Examples:
	| FirstName | LastName | DateOfBirth | PhoneNumber  | Email        | BankAccountNumber   |
	|           | Doe      | 1990-01-01  | 123-456-7890 | john.doe.com | 12343456 |
