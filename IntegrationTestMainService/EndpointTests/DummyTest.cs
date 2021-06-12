using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTestMainService.EndpointTests
{
    public class DummyTest : IClassFixture<WebApplicationFactory<MainService.Startup>>
    {
        private readonly WebApplicationFactory<MainService.Startup> _factory;

        public DummyTest(WebApplicationFactory<MainService.Startup> factory)
        {
            _factory = factory;
        }
        
        /// <summary>
        /// This passes all the InlineData attributes into the method for testing. 
        /// </summary>
        /// <param name="url"></param>
        [Theory]
        [InlineData("/api/dummy")]
        public async Task TestGetURLCheckStatusCode(string url) 
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }
    }
}