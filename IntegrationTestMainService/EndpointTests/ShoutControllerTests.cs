using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTestMainService.EndpointTests
{
    public class ShoutControllerTests : IClassFixture<WebApplicationFactory<MainService.Startup>>
    {
        private readonly WebApplicationFactory<MainService.Startup> _factory;

        public ShoutControllerTests(WebApplicationFactory<MainService.Startup> factory)
        {
            _factory = factory;
        }
        // I didn't delete what's below because it's helpful for integration testing with auth
        //
        // [Fact]
        // public async Task GetShoutsForUser()
        // {
        //     var client = GetAuthClient(); 
        //     //Act
        //     var response = await client.GetAsync("/api/shout");
        //
        //     var content = await response.Content.ReadAsStringAsync();
        //
        //     var shouts = JsonConvert.DeserializeObject<List<ShoutDeliverable>>(content);
        //     
        //     // Assert
        //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        //     Assert.NotEmpty(shouts);
        //     Assert.NotNull(shouts[0]);
        //     Assert.NotNull(shouts[0].Customer);
        //     Assert.NotNull(shouts[0].Customer.UserName);
        //     Assert.NotNull(shouts[0].Url);
        // }
        //
        // [Fact]
        // public async Task LikeShout()
        // {
        //     var client = GetAuthClient(); 
        //     
        //     var response = await client.GetAsync("/api/shout");
        //
        //     var content = await response.Content.ReadAsStringAsync();
        //
        //     var shouts = JsonConvert.DeserializeObject<List<ShoutDeliverable>>(content);
        //
        //     var shout = new Shout
        //     {
        //         Id = shouts[0].Id,
        //         Url = shouts[0].Url,
        //         Customer = shouts[0].Customer,
        //         DateAdded = DateTime.Now,
        //     };
        //     
        //     var values = new Dictionary<string, string>
        //     {
        //         { "Shout", shout.ToString() },
        //     };
        //     var postContent = new FormUrlEncodedContent(values);
        //
        //     // Act
        //     var likeResponse = await client.PostAsync("/api/shout/recordlike", postContent);
        //
        //     var likeResponseContent = await likeResponse.Content.ReadAsStringAsync();
        //     
        //     // Assert
        //     Assert.Equal(HttpStatusCode.OK, likeResponse.StatusCode);
        //     Assert.NotNull(likeResponseContent);
        //     
        //     Assert.NotEqual("", likeResponseContent);
        //
        // }

        private HttpClient GetAuthClient()
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

            return client;
        }

    }
}