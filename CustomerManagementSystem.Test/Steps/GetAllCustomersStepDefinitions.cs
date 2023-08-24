using System.Net.Http.Json;
using CustomerManagementSystem.Application;
using CustomerManagementSystem.Application.Customer.Command;
using CustomerManagementSystem.Application.Customer.Dtos;
using Microsoft.AspNetCore.Http;
using TechTalk.SpecFlow;

namespace CustomerManagementSystem.Test.Steps
{
    [Binding]
    public class GetAllCustomersStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly HttpClient _httpClient;

        public GetAllCustomersStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _httpClient = new HttpClient();
            // Set up HttpClient (replace with your actual API base URL)
            _httpClient.BaseAddress = new Uri("https://localhost:7044/");
        }

        [Given("I have some existing customers")]
        public async Task GivenIHaveSomeExistingCustomers()
        {
            // Create multiple customers (you may need to adjust this based on your API)
            var createCustomerCommands = new List<CreateCustomerCommand>
            {
                new CreateCustomerCommand
                {
                CustomerDto = new CustomerDto
                {
                    FirstName = "ToCreate2",
                    LastName = "Customer2",
                    DateOfBirth = new DateTime(1980, 3, 10),
                    PhoneNumber = "+981123456789",
                    Email = "Create2.customer2@example.com",
                    BankAccountNumber = "2222-8888-7777-6666"
                }
                },
                new CreateCustomerCommand
                {
                CustomerDto = new CustomerDto
                {
                    FirstName = "ToCreate",
                    LastName = "Customer",
                    DateOfBirth = new DateTime(1980, 3, 10),
                    PhoneNumber = "+989123456789",
                    Email = "Create.customer@example.com",
                    BankAccountNumber = "9999-8888-7777-6666"
                }
                }
            };

            foreach (var createCustomerCommand in createCustomerCommands)
            {
                // Send a POST request to create each customer
                var createResponse = await _httpClient.PostAsJsonAsync("/api/customer/CreateCustomer", createCustomerCommand);
                var result = await createResponse.Content.ReadAsAsync<FluentResultVM<string>>();
                createResponse.EnsureSuccessStatusCode();
            }
            _scenarioContext.Set(createCustomerCommands, "CreatedCustomers");
        }

        [When("I send a request to get all customers")]
        public async Task WhenISendARequestToGetAllCustomers()
        {
            // Send a GET request to get all customers
            var getAllResponse = await _httpClient.GetAsync("/api/customer/GetAllCustomers");
            _scenarioContext.Set(getAllResponse, "GetAllResponse");
        }

        [Then("the response should indicate success and contain a list of customers")]
        public async Task ThenTheResponseShouldIndicateSuccessAndContainAListOfCustomers()
        {
            // Retrieve the response from ScenarioContext
            var response = _scenarioContext.Get<HttpResponseMessage>("GetAllResponse");
            var existingCustomer = _scenarioContext.Get<List<CreateCustomerCommand>>("CreatedCustomers");

            // Check if the response is successful (status code 200 OK).
            response.EnsureSuccessStatusCode();

            // Optionally, you can further validate the response content if needed.
            var result = await response.Content.ReadFromJsonAsync<FluentResultVM<List<CustomerDto>>>();
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(result.value.Count, existingCustomer.Count);
            Assert.True(result.IsSuccess);
        }
    }
}
