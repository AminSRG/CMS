using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using CustomerManagementSystem.Application;
using CustomerManagementSystem.Application.Customer.Command;
using CustomerManagementSystem.Application.Customer.Dtos;
using CustomerManagementSystem.Application.Customer.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;

namespace CustomerManagementSystem.Test.Steps
{
    [Binding]
    public class GetCustomerByIdStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly HttpClient _httpClient;

        public GetCustomerByIdStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;

            // Set up HttpClient (replace with your actual API base URL)
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7044/")
            };
        }

        [When("I send a request to get the customer by ID")]
        public async Task WhenISendARequestToGetTheCustomerById()
        {
            // Retrieve the customer ID from ScenarioContext
            var customerId = _scenarioContext.Get<string>("CreatedCustomerID");

            var existingCustomer = new GetCustomerByIdQuery
            {
                CustomerId = customerId
            };

            // Serialize the customer object to JSON
            var jsonContent = new StringContent(JsonSerializer.Serialize(existingCustomer), Encoding.UTF8, "application/json");

            // Assuming you want to send a DELETE request without a request body (JSON content)
            var getCustomerRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/customer/GetCustomerById");
            getCustomerRequest.Content = jsonContent;

            // Send a GET request to get the customer by ID
            var getResponse = await _httpClient.SendAsync(getCustomerRequest);
            _scenarioContext.Set(getResponse, "GetResponse");
        }

        [Then("the response should indicate success & return customer")]
        public async Task ThenTheResponseShouldIndicateSuccess()
        {
            // Retrieve the response from ScenarioContext
            var response = _scenarioContext.Get<HttpResponseMessage>("GetResponse");
            var createdCustomer = _scenarioContext.Get<CustomerDto>("CreatedCustomer");

            // Check if the response is successful (status code 200 OK).
            response.EnsureSuccessStatusCode();

            // Optionally, you can further validate the response content if needed.
            var result = await response.Content.ReadFromJsonAsync<FluentResultVM<CustomerDto>>();
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(result.value.Email , createdCustomer.Email);
            Assert.True(result.IsSuccess);
        }
    }
}
