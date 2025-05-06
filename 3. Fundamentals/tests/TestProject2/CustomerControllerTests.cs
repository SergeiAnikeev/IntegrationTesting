using System.Net;

namespace Customers.Api.Tests.Integration
{
    public class CustomerControllerTests : IAsyncLifetime
    {
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("https://localhost:5001")
        };
        public CustomerControllerTests()
        {
            //setup code
        }
        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
        public async Task DisposeAsync()
        {
            await Task.Delay(1);
        }

        [Fact]
        public async Task Get_ReturnNorFound_WhenCustomerDoesNotExist()
        {
            //Arrange

            //Act
            var response = await _httpClient.GetAsync($"/customers/{Guid.NewGuid()}");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Get_ReturnNorFound_WhenCustomerDoesNotExist2()
        {
            //Arrange

            //Act
            var response = await _httpClient.GetAsync($"/customers/{Guid.NewGuid()}");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


    }
}