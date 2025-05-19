using Bogus;
using Customers.Api.Contracts.Requests;
using Customers.Api.Contracts.Responses;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace Customers.Api.Tests.Integration
{
    public class CustomerControllerTests : IClassFixture<WebApplicationFactory<IApiMarker>>, IAsyncLifetime
    {
        private readonly HttpClient _httpClient;
        private readonly Faker<CustomerRequest> _customerGenerator = new Faker<CustomerRequest>()
            .RuleFor(x => x.FullName, faker => faker.Person.FullName)
            .RuleFor(x => x.Email, faker => faker.Person.Email)
            .RuleFor(x => x.GitHubUsername, "sergeianikeev")
            .RuleFor(x => x.DateOfBirth, faker => faker.Person.DateOfBirth.Date)
            ;
        private readonly List<Guid> _createdIds = new();

        public CustomerControllerTests(WebApplicationFactory<IApiMarker> appFactory)
        {
            _httpClient = appFactory.CreateClient();
        }

        [Fact]
        public async Task Get_ReturnNorFound_WhenCustomerDoesNotExist()
        {
            //Act
            var response = await _httpClient.GetAsync($"/customers/{Guid.NewGuid()}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            //var text = await response.Content.ReadAsStringAsync();
            var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            problem!.Title.Should().Be("Not Found");
            problem.Status.Should().Be(404);

        }

        [Fact]
        public async Task Create_ReturnsCreated_WhenCustomerIsCreated()
        {
            //Arrange
            var customer = _customerGenerator.Generate();

            //Act
            var response = await _httpClient.PostAsJsonAsync("customers", customer);

            //Assert
            var customerResponse = await response.Content.ReadFromJsonAsync<CustomerResponse>();
            customerResponse.Should().BeEquivalentTo(customer);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            _createdIds.Add(customerResponse!.Id);

        }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            foreach(var createdId in _createdIds) 
            {
                await _httpClient.DeleteAsync($"/customers/{createdId}");
            }
        }
    }
}