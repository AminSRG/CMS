using CustomerManagementSystem.Application;
using CustomerManagementSystem.Application.Customer.Command;
using CustomerManagementSystem.Application.Customer.Dtos;
using Microsoft.AspNetCore.Http;
using System.Net;
using TechTalk.SpecFlow;

namespace CustomerManagementSystem.Test.Steps
{
    [Binding]
    public class CreateCustomerStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly HttpClient _httpClient;

        public CreateCustomerStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;

            // Set up HttpClient (replace with your actual API base URL)
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7044/"),
            };
        }

        [Given(@"I have invalid customer data")]
        public void GivenIHaveInvalidCustomerData()
        {
            // Define a valid customer data object using your DTO structure.
            var command = new CreateCustomerCommand()
            {
                CustomerDto = new CustomerDto
                {
                    FirstName = "",
                    LastName = "Customer",
                    DateOfBirth = new DateTime(1980, 3, 10),
                    PhoneNumber = "123",
                    Email = "example.com",
                    BankAccountNumber = "99996666"
                }
            };

            // Store the valid customer data in the ScenarioContext to be used in subsequent steps.
            _scenarioContext.Set(command, "ValidCustomer");
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

        [Given(@"I have a valid customer with the following details:")]
        public void GivenIHaveAValidCustomerWithTheFollowingDetails(Table table)
        {
            throw new PendingStepException();
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
        
        [Then(@"the response should indicate a validation error with status (.*)")]
        public async Task ThenTheResponseShouldIndicateAValidationErrorWithStatusAsync(int p0)
        {
            // Retrieve the response from the ScenarioContext
            var response = _scenarioContext.Get<HttpResponseMessage>("CreateCustomerResponse");

            // Check if the response has a status code of 400 Bad Request.
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            // Optionally, you can further validate the response content if it contains validation error messages.
            var result = await response.Content.ReadAsAsync<FluentResultVM<bool>>();
            Assert.NotNull(result.Errors); // Replace with actual validation error message
        }

        [Then(@"the response should indicate success")]
        public async Task ThenTheResponseShouldIndicateSuccessAsync()
        {
            // Retrieve the response from the ScenarioContext
            var response = _scenarioContext.Get<HttpResponseMessage>("CreateCustomerResponse");

            // Check if the response is successful (status code 200 OK).
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Optionally, you can further validate the response content if needed.
            var result = await response.Content.ReadAsAsync<FluentResultVM<bool>>();
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
