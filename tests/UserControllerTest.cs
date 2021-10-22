
using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System.Threading.Tasks;

using Xunit;

namespace TestProject
{
    public class UserControllerTest
    {
        private readonly IConfiguration _configuration;
        private ServiceProvider _serviceProvider;
        private UserController _userController;


        public UserControllerTest(TestSetup testSetup)
        {
            using var logFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = logFactory.CreateLogger<UserController>();

        }


        [Fact]
        public async Task GetImgShouldReturnOk()
        {
            // create a unit test for the controller

            var resp =await _userController.GetImg();
            // assert that the result is not null
            Assert.NotNull(resp);
            var result = Assert.IsType<OkObjectResult>(resp);
            // assert that the result is http status code 200
            Assert.Equal(200, result.StatusCode);

        }
    }
}
