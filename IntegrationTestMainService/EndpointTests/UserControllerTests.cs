using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTestMainService.EndpointTests
{
    public class UserControllerTests : IClassFixture<WebApplicationFactory<MainService.Startup>>
    {
        private readonly WebApplicationFactory<MainService.Startup> _factory;

        public UserControllerTests(WebApplicationFactory<MainService.Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetDickbuttFromUserController()
        {
            var client = _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddAuthentication("Test")
                            .AddScheme<AuthenticationSchemeOptions, AuthHandler>(
                                "Test", options => {});
                    });
                })
                .CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                });

            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Test");
            
            //Act
            var response = await client.GetAsync("/api/users/dickbutt");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
 

    }
}