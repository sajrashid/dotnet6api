
using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;

using MyAPI.Models;

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xunit;

namespace TestProject.IntergrationTests.Controllers
{
    public class RolesControllerIntergrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly TestData testData;
        public RolesControllerIntergrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            testData = new TestData();
            // delete data & recreate new for every test run
            testData.DeleteUserAndRoles();

        }

        // Tests will execute in sequence as they are in the same test class 

        [Fact]
        public async Task Get_All_Roles_Returns_ListOfRoles_OK()
        {
            testData.CreateUserAndRoles();
            var response = await _client.GetAsync("/api/Roles/");
            response.EnsureSuccessStatusCode();
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var rolesList = JsonSerializer.Deserialize<List<Roles>>(res);
            rolesList.Should().NotBeNull();
            rolesList.Should().HaveCount(2);
            rolesList.Should().BeOfType<List<Roles>>();
        }

        [Fact]
        public async Task Get_Roles_By_Id_Returns_Roles_OK()
        {
            testData.CreateUserAndRoles();
            var response = await _client.GetAsync("/api/Roles/1");

            response.EnsureSuccessStatusCode();
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var roles = JsonSerializer.Deserialize<Roles>(res);
            roles.Should().NotBeNull();
            roles.Should().BeOfType<Roles>();
        }
        [Fact]
        public async Task Get_Roles_By_Id_Returns_NotFound()
        {
            var response = await _client.GetAsync("/api/Roles/1010");
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_New_Roles_Returns_Roles_OK()
        {
            //Arrange 
            // create a new user
            testData.CreateUserAndRoles();
            // Create_New_Roles
            var role = new Roles { Role = "Admin", UserId = 1 };
            var json = JsonSerializer.Serialize<Roles>(role);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            // insert role
            var response = await _client.PostAsync("/api/Roles", data).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var newRoles = JsonSerializer.Deserialize<Roles>(res, options);

            // Assert
            newRoles.Should().NotBeNull();
            newRoles.Should().BeOfType<Roles>();
            newRoles.Role.Should().Be("Admin");
        }

        [Fact]
        public async Task Update_Roles_Returns_Roles_OK()
        {
            testData.CreateUserAndRoles();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var roles = new Roles { Id = 1, UserId = 1, Role = "Dev" };
            var json = JsonSerializer.Serialize<Roles>(roles);
            var data = new StringContent(json, Encoding.UTF8, "application/json");


            var response = await _client.PutAsync($"/api/Roles/{1}", data).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var upatedRoles = JsonSerializer.Deserialize<Roles>(res, options);

            upatedRoles.Should().NotBeNull();
            upatedRoles.Should().BeOfType<Roles>();
            upatedRoles.Role.Should().Be("Dev");
        }

        [Fact]
        public async Task Update_Non_Exisiting_Roles_Returns_NotFound()
        {
            // create a new role
            var roles = new Roles { Id = 101010, Role = "abcrole", UserId = 1 };
            var json = JsonSerializer.Serialize<Roles>(roles);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PutAsync($"/api/Roles/{roles.Id}", data).ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Deletes_New_Role_Returns_OK()
        {
            testData.CreateUserAndRoles();

            var response = await _client.DeleteAsync($"/api/Roles/{1}").ConfigureAwait(false);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}


