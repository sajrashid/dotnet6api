using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;

using MyAPI.Models;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Xunit;

namespace TestProject.IntergrationTests.Controllers
{
    [Collection("Account Collection")]
    public class AccountControllerIntergrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        private readonly CustomWebApplicationFactory<Program> _factory;

        private readonly TestData testData;
        private readonly string token;
        public AccountControllerIntergrationTests(
        CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            testData = new TestData();
            // delete users 
            testData.DeleteAccounts();
            token = TokenFixture.GetToken();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Fact]
        public async Task Create_And_GetUserAccount_Returns_String_OK()
        {
            // Arrange
            Auth user = new() { Email = "testUser2@test.com", Password = "Password99" };

            var json = JsonSerializer.Serialize<Auth>(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync("/api/accounts/", data);
            // Assert
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            res.Should().BeOfType<string>();
            res.Should().Contain("Account Created, Please Login.");

            //Act
            response = await _client.GetAsync("/api/accounts/1");
            // Assert
            response.EnsureSuccessStatusCode();
            res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var userWithId = JsonSerializer.Deserialize<User>(res);
            userWithId.Should().BeOfType<User>();
        }

        [Fact]
        public async Task Create_Exisiting_Account_Returns_Conflict()
        {
            // Arrange
            Auth user = new() { Email = "testUser@test.com", Password = "Password99" };

            var json = JsonSerializer.Serialize<Auth>(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync("/api/accounts/", data);
            // Assert
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            res.Should().BeOfType<string>();
            res.Should().Contain("Account Created, Please Login.");
            // Assert
            response = await _client.PostAsync("/api/accounts/", data);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Delete_Existiting_Account_Returns_OK()
        {
            // Arrange
            Auth user = new Auth { Email = "testUser@test.com", Password = "Password99" };

            var json = JsonSerializer.Serialize<Auth>(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            //Act
            var response = await _client.PostAsync("/api/accounts/", data).ConfigureAwait(false);
            // Assert
            response.EnsureSuccessStatusCode();
            //Act
            response = await _client.DeleteAsync("/api/accounts/1");
            response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task Get_Non_Existiting_Account_Returns_Mot_Found()
        {
            var response = await _client.GetAsync("/api/accounts/1001");
            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task Post_Bad_Password_Returns_BadRequest()
        {
            Auth user = new() { Email = "testUser@test.com", Password = "" };

            var json = JsonSerializer.Serialize<Auth>(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync("/api/accounts/", data);
            // Assert
            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            var user2 = new Auth { Email = null, Password = null };

            json = JsonSerializer.Serialize<Auth>(user2);
            data = new StringContent(json, Encoding.UTF8, "application/json");
            response = await _client.PostAsync("/api/accounts/", data).ConfigureAwait(false);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetAll_UserAccounts_Returns_List_Of_Users_OK()
        {
            testData.CreateListTestUsers();

            //Act
            var response = await _client.GetAsync("/api/accounts/").ConfigureAwait(false);
            // Assert

            var res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var userList = JsonSerializer.Deserialize<List<User>>(res);

            response.EnsureSuccessStatusCode();
            userList.Should().NotBeNull();
            userList.Should().HaveCount(10);
            userList.Should().BeOfType<List<User>>();
        }
    }
}
