using CustomerManagementSystem.Application.Customer.Command;
using CustomerManagementSystem.Application.Customer.Dtos;
using FluentResults;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Net;
using TechTalk.SpecFlow;

namespace CustomerManagementSystem.Test.Steps
{
    [Binding]
    public class CreateValidCustomerStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly HttpClient _httpClient;

        public CreateValidCustomerStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;

            // Set up HttpClient (replace with your actual API base URL)
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7044/"),
            };
        }

        [Given(@"I have valid customer data")]
        public void GivenIHaveValidCustomerData()
        {
            // Define a valid customer data object using your DTO structure.
            var command = new CreateCustomerCommand()
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
            };

            // Store the valid customer data in the ScenarioContext to be used in subsequent steps.
            _scenarioContext.Set(command, "ValidCustomer");
        }

        [When(@"I send a request to create a customer")]
        public async Task WhenISendARequestToCreateACustomerAsync()
        {
            // Retrieve the valid customer data from the ScenarioContext
            var validCustomer = _scenarioContext.Get<CreateCustomerCommand>("ValidCustomer");

            // Send a POST request to the CreateCustomer endpoint.
            var response = await _httpClient.PostAsJsonAsync("/api/customer/CreateCustomer", validCustomer);

            // Store the response in the ScenarioContext for later assertions.
            _scenarioContext.Set(response, "CreateCustomerResponse");
        }

        [Then(@"the response should indicate success")]
        public async Task ThenTheResponseShouldIndicateSuccessAsync()
        {
            // Retrieve the response from the ScenarioContext
            var response = _scenarioContext.Get<HttpResponseMessage>("CreateCustomerResponse");
            var resultw = await response.Content.ReadAsStringAsync();

            // Check if the response is successful (status code 200 OK).
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Optionally, you can further validate the response content if needed.
            var result = await response.Content.ReadAsAsync<Result<bool>>();
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
