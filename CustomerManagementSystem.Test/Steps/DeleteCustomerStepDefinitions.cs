using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CustomerManagementSystem.Application;
using CustomerManagementSystem.Application.Customer.Command;
using CustomerManagementSystem.Application.Customer.Dtos;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TechTalk.SpecFlow;
using Xunit;

namespace CustomerManagementSystem.Test.Steps
{
    [Binding]
    public class DeleteCustomerStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly HttpClient _httpClient;

        public DeleteCustomerStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7044/") // Update with your API base URL
            };
        }

        [Given("I have an existing customer")]
        public async Task GivenIHaveAnExistingCustomer()
        {
            // Retrieve an existing customer from your application's database or create one.
            // You can use a valid customer as a starting point if needed.
            var existingCustomer = new CreateCustomerCommand
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

            // Serialize the customer object to JSON
            var jsonContent = new StringContent(JsonSerializer.Serialize(existingCustomer), Encoding.UTF8, "application/json");

            // Send a POST request to create the customer (if not already existing)
            var createResponse = await _httpClient.PostAsync("/api/customer/CreateCustomer", jsonContent);
            createResponse.EnsureSuccessStatusCode(); // Ensure the creation was successful

            var result = await createResponse.Content.ReadAsAsync<FluentResultVM<string>>();
            _scenarioContext.Set(existingCustomer.CustomerDto, "CreatedCustomer");
            _scenarioContext.Set(result.value, "CreatedCustomerID");
        }

        [When("I send a request to delete the customer")]
        public async Task WhenISendARequestToDeleteTheCustomer()
        {
            // Retrieve the existing customer from ScenarioContext
            var createdCustomerID = _scenarioContext.Get<string>("CreatedCustomerID");

            // Create a delete request using the customer's data (assuming your API accepts a delete request with customer data)
            var deleteRequestCommand = new DeleteCustomerCommand
            {
                CustomerId = createdCustomerID
            };

            // Serialize the delete request object to JSON
            var jsonContent = new StringContent(JsonSerializer.Serialize(deleteRequestCommand), Encoding.UTF8, "application/json");

            // Assuming you want to send a DELETE request without a request body (JSON content)
            var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"/api/customer/DeleteCustomer");
            deleteRequest.Content = jsonContent;

            // Send the DELETE request to delete the customer
            var deleteResponse = await _httpClient.SendAsync(deleteRequest);
            _scenarioContext.Set(deleteResponse, "DeleteResponse");
        }

        [Then("the delete response should indicate success")]
        public async Task ThenTheDeleteResponseShouldIndicateSuccess()
        {
            // Retrieve the delete response from the ScenarioContext
            var deleteResponse = _scenarioContext.Get<HttpResponseMessage>("DeleteResponse");

            // Check if the response is successful (status code 200 OK).
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

            // Optionally, you can further validate the response content if needed.
            var result = await deleteResponse.Content.ReadAsAsync<FluentResultVM<bool>>();
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
        }
    }
}

