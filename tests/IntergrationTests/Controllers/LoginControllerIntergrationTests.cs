using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;

using MyAPI.Models;

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
    [Collection("Login Collection")]
    public class LoginControllerIntergrationTests : IClassFixture<CustomWebApplicationFactory<Program>>, IClassFixture<TokenFixture>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;
        private readonly TokenFixture _tokenFixture;
        readonly TestData testData;
        public LoginControllerIntergrationTests(
        CustomWebApplicationFactory<Program> factory, TokenFixture tokenFixture)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            _tokenFixture = tokenFixture;
            testData = new TestData();
            //  delete users 
            testData.DeleteAccounts();
        }

        [Fact]
        public async Task LogInUser_Returns_Token_And_Login_Success_Message()
        {
            testData.CreateTestUser("testUser@test.com", "Password99");
            // Arrange
            Auth user = new Auth { Email = "testUser@test.com", Password = "Password99" };

            var json = JsonSerializer.Serialize<Auth>(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync("/api/login/", data);

            // Assert
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var token = JsonSerializer.Deserialize<ApiToken>(res, options);
            token.Should().BeOfType<ApiToken>();
            token.Message.Should().Be("Success");
        }

        [Fact]
        public async Task Login_With_Incorrect_UserName_Returns_Forbidden()
        {
            testData.CreateTestUser("testUser@test.com", "Password99");
            // Arrange
            Auth user = new Auth { Email = "Incorrect@test.com", Password = "Password99" };

            var json = JsonSerializer.Serialize<Auth>(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync("/api/login/", data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Login_With_Incorrect_Password_Returns_Forbidden()
        {
            testData.CreateTestUser("testUser@test.com", "Password99");
            // Arrange
            Auth user = new Auth { Email = "testUser@test.com", Password = "Badpassword99" };

            var json = JsonSerializer.Serialize<Auth>(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync("/api/login/", data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Validate_With_Lowercase_Password_Returns_BadRequest()
        {
            testData.CreateTestUser("testUser@test.com", "Password99");
            // Arrange
            // test lower case without number should bad BadRequest
            Auth user = new Auth { Email = "testUser@test.com", Password = "lowercase" };
            var json = JsonSerializer.Serialize<Auth>(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            //Act
            var response = await _client.PostAsync("/api/login/", data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Validate_With_Lowercase_And_Number_Password_Returns_BadRequest()
        {
            testData.CreateTestUser("testUser@test.com", "Password99");
            // Arrange
            // test lower case without number should bad BadRequest
            Auth user = new Auth { Email = "testUser@test.com", Password = "lowercase99" };
            var json = JsonSerializer.Serialize<Auth>(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            //Act
            var response = await _client.PostAsync("/api/login/", data);
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Validate_With_Empty_Password_Returns_BadRequest()
        {
            testData.CreateTestUser("testUser@test.com", "Password99");
            // Arrange
            // test lower case without number should bad BadRequest
            Auth user = new Auth { Email = "testUser@test.com", Password = "" };
            var json = JsonSerializer.Serialize<Auth>(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            //Act
            var response = await _client.PostAsync("/api/login/", data);
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Validate_With_Bad_Domain_Returns_BadRequest()
        {
            testData.CreateTestUser("testUser@test.com", "Password99");
            // Arrange
            // test lower case without number should bad BadRequest
            Auth user = new Auth { Email = "testUser@test.", Password = "" };
            var json = JsonSerializer.Serialize<Auth>(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            //Act
            var response = await _client.PostAsync("/api/login/", data);
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Validate_With_Short_Password_Returns_BadRequest()
        {
            testData.CreateTestUser("testUser@test.com", "Password99");
            // Arrange
            // test lower case without number should bad BadRequest
            Auth user = new Auth { Email = "testUser@test.com", Password = "P1234" };
            var json = JsonSerializer.Serialize<Auth>(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            //Act
            var response = await _client.PostAsync("/api/login/", data);
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Validate_With_Invalid_Email_Returns_BadRequest()
        {
            testData.CreateTestUser("testUser@test.com", "Password99");
            // Arrange
            // test invalid email address
            Auth user = new Auth { Email = "testUser!test.com", Password = "Password99" };

            var json = JsonSerializer.Serialize<Auth>(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync("/api/login/", data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task Validate_With_Empty_Email_Returns_BadRequest()
        {
            testData.CreateTestUser("testUser@test.com", "Password99");
            // Arrange
            // test invalid email address
            Auth user = new Auth { Email = "", Password = "Password99" };

            var json = JsonSerializer.Serialize<Auth>(user);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            //Act
            var response = await _client.PostAsync("/api/login/", data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Get_Users_Returns_ListOfUsers_OK()
        {
            var token = TokenFixture.GetToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _client.GetAsync("/api/login/");
            response.EnsureSuccessStatusCode();
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var res = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var UserList = JsonSerializer.Deserialize<List<User>>(res);
            UserList.Should().NotBeNull();
            UserList.Should().BeOfType<List<User>>();
        }
    }

    public class ApiToken
    {
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
