using FluentAssertions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace TestProject
{
    public class UnAuthTest
     : IClassFixture<WebApplicationFactory<API.Startup>>
    {
        private readonly WebApplicationFactory<API.Startup> _factory;

        public UnAuthTest(WebApplicationFactory<API.Startup> factory)
        {
            _factory = factory;
        }


        [Theory]
        [InlineData("/Authenticate/Get")]
        public async Task Get_EndpointsReturnUnauth(string url)
        {
            // Arrange
            var client = _factory.CreateClient();
            // Act
            var response = await client.GetAsync(url);
            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);

        }


    }
}
