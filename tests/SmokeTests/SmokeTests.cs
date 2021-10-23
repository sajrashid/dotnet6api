using Microsoft.AspNetCore.Mvc.Testing;

using System.Threading.Tasks;

using Xunit;

namespace TestProject.SmokeTests
{
    public class SmokeTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public SmokeTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
        [Theory]
        [InlineData("/swagger/index.html")]
        public async Task EnsureSwaggerIndexHTMLReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/swagger/v1/swagger.json")]
        public async Task EnsureSwaggerJsonReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
    }
}