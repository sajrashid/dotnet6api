using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;

using MyAPI.Models;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xunit;

namespace TestProject.IntergrationTests.Controllers
{
    public class VistorsControllerIntergrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;
        readonly TestData testData;
        public VistorsControllerIntergrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            testData = new TestData();
            // delete data & recreate new for every test run
            testData.DeleteVisitors();
            var token = TokenFixture.GetToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        // creates a visitor
        [Fact]
        public async Task Get_A_Visitor__WhoIs_Returns_OK()
        {
            var response = await _client.GetAsync("/api/Visitors/WhoIs");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Delete_A_Visitor__Returns_OK()
        {
            testData.CreateTestVisitor("Chrome", "1p-123");
            var response = await _client.DeleteAsync("/api/Visitors/1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        [Fact]
        public async Task Get_All_Visitor_Returns_ListOfVistors_OK()
        {
            // create a visitor
            await _client.GetAsync("/api/Visitors/WhoIs").ConfigureAwait(true);
            var response = await _client.GetAsync("/api/Visitors/");
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var visitorList = JsonSerializer.Deserialize<List<Visitor>>(res, options);
            visitorList.Should().HaveCount(1);
            visitorList.Should().BeOfType<List<Visitor>>();
        }

        [Fact]
        public async Task Update_Visitor_Returns_Single_Visitor_OK()
        {
            var token = TokenFixture.GetToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Arrange 
            // create a new visitor
            testData.CreateTestVisitor("userAgent-Chrome", "IP-123");

            //Act
            var visitor = new Visitor { Id = 1, Hash = "xyz", IP = "IP", UserAgent = "chrome", LastVisit = DateTime.Now, };
            var json = JsonSerializer.Serialize<Visitor>(visitor);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/Visitors/{1}", data).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var upatedvisitor = JsonSerializer.Deserialize<Visitor>(res, options);

            upatedvisitor.Should().NotBeNull();
            upatedvisitor.Should().BeOfType<Visitor>();
            upatedvisitor.Hash.Should().Be("xyz");
        }
    }
}