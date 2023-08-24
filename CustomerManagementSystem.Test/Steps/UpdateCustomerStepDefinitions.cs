using CustomerManagementSystem.Application;
using CustomerManagementSystem.Application.Customer.Command;
using CustomerManagementSystem.Application.Customer.Dtos;
using System;
using System.Net;
using System.Text;
using System.Text.Json;
using TechTalk.SpecFlow;

namespace CustomerManagementSystem.Test.Steps
{
    [Binding]
    public class UpdateCustomerStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly HttpClient _httpClient;

        public UpdateCustomerStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _httpClient = new HttpClient();
            // Set up HttpClient (replace with your actual API base URL)
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7044/"),
            };
        }

        [Given("I create a customer with the valid data")]
        public async Task GivenICreateACustomerWithTheValidData()
        {
            // Retrieve the valid customer from ScenarioContext
            var validCustomer = _scenarioContext.Get<CreateCustomerCommand>("ValidCustomer");

            // Serialize the customer object to JSON
            var jsonContent = new StringContent(JsonSerializer.Serialize(validCustomer), Encoding.UTF8, "application/json");

            // Send a POST request to create the customer
            var createResponse = await _httpClient.PostAsync("/api/customer/CreateCustomer", jsonContent);
            createResponse.EnsureSuccessStatusCode(); // Ensure the creation was successful

            var result = await createResponse.Content.ReadAsAsync<FluentResultVM<string>>();
            _scenarioContext.Set(result.value, "CreatedCustomerID");

        }

        [When("I update the customer's email to \"(.*)\"")]
        public void WhenIUpdateTheCustomerSFirstNameTo(string updatedEmail)
        {
            // Retrieve the valid customer from ScenarioContext
            var validCustomer = _scenarioContext.Get<CreateCustomerCommand>("ValidCustomer");
            var createdCustomerID = _scenarioContext.Get<string>("CreatedCustomerID");

            // Update the Email
            var updateCustomer = new UpdateCustomerCommand()
            {
                CustomerId = createdCustomerID,
                CustomerDto = validCustomer.CustomerDto
            };
            updateCustomer.CustomerDto.Email = updatedEmail;

            _scenarioContext.Set(updateCustomer, "UpdatedCustomer");
        }

        [When("I send a request to update the customer")]
        public async Task WhenISendARequestToUpdateTheCustomer()
        {
            // Retrieve the updated customer from ScenarioContext
            var updatedCustomer = _scenarioContext.Get<UpdateCustomerCommand>("UpdatedCustomer");

            // Serialize the updated customer object to JSON
            var jsonContent = new StringContent(JsonSerializer.Serialize(updatedCustomer), Encoding.UTF8, "application/json");

            // Send a PUT request to update the customer
            var updateResponse = await _httpClient.PutAsync("/api/customer/UpdateCustomer", jsonContent);
            _scenarioContext.Set(updateResponse, "updateResponse");
        }

        [Then("the update response should indicate success")]
        public async Task ThenTheResponseShouldIndicateSuccess()
        {
            // Retrieve the response from the ScenarioContext
            var response = _scenarioContext.Get<HttpResponseMessage>("updateResponse");

            // Check if the response is successful (status code 200 OK).
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Optionally, you can further validate the response content if needed.
            var result = await response.Content.ReadAsAsync<FluentResultVM<bool>>();
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}
