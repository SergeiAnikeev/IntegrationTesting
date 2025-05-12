using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;

namespace Customers.Api.Tests.Integration
{
    public class CustomerControllerTests : IClassFixture<WebApplicationFactory<IApiMarker>>
    {
        private readonly HttpClient _httpClient;
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


    }
}