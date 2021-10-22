
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

using Xunit;
using FluentAssertions;
using FakeItEasy;
using MyAPI.Repository;
using MyAPI.Models;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace TestProject.IntergrationTests
{
    public class ApiTests
    {
        public ApiTests(APIAppFixture fixture)
        {
            Fixture = fixture;

        }
        private APIAppFixture Fixture { get; }

        [Fact]
        public async Task Can_Read_Products_From_Api()
        {
            // var client = await CreateAuthenticatedClientAsync();

            var client = Fixture.CreateClient();
            var response = await client.GetFromJsonAsync<List<Product>>("url");
            response.Should().NotBeNull();
            response.Should().NotBeEmpty();
            response.Count.Should().BeGreaterThan(999);
            response.Should().BeOfType<List<Product>>();
        }
        private async Task<HttpClient> CreateAuthenticatedClientAsync()
        {
            var options = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                BaseAddress = Fixture.ClientOptions.BaseAddress,
                HandleCookies = true
            };

            var client = Fixture.CreateClient(options);

            var parameters = Array.Empty<KeyValuePair<string?, string?>>();
            using var content = new FormUrlEncodedContent(parameters);

            // Go through the sign-in flow, which will set
            // the authentication cookie on the HttpClient.
            using var response = await client.PostAsync("/sign-in", content);

            Assert.True(response.IsSuccessStatusCode);

            return client;
        }
    }
}
