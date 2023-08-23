Feature: CrearteValidCustomer
  As a system user
  I want to create a valid customer
  So that I can add them to the system

  @tag1
  Scenario: Create a Valid Customer
    Given I have valid customer data
    When I send a request to create a customer
    Then the response should indicate success
