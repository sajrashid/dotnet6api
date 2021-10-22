using FluentAssertions;

using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;

using MyAPI.Services;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

using Xunit;

using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace TestProject.UnitTests.Services
{
    /// <summary>
    ///    Tests the TokensService
    ///    Reads the same IConfiguration as the application, loading the same appsetting.json
    ///    As determined by your env
    ///    This test needs to remain
    /// </summary>
    public class TokenServiceUnitTest
    {

        private readonly IConfiguration config;

        public TokenServiceUnitTest()
        {

            var builder = WebApplication.CreateBuilder();
            config = builder.Configuration;
            builder.Build();
        }

        [Fact]
        public void IsTokenFactoryCreatingValidTokens()
        {
            // Arrange
            TokenService tokenservice = new(config);

            // set up token validation rules get from startup
            TokenValidationParameters validationParameters = new()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
            };

            // create some roles
            var rolesList = new List<string>
            {
                "XunitTestUser",
                "TestUser",
            };

            // ACT
            // get a new token from the factory
            var token = tokenservice.CreateToken(rolesList);

            //Assert
            token.Should().BeOfType<string>();
            token.Should().NotBeNull();

            // validate token is an actual token using the validatin paramters from the setup token validation above
            var claimsPrinicpal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            // apparently if it returns a principal it means it's valid maybe
            claimsPrinicpal.Should().BeOfType<ClaimsPrincipal>();

            // Test the roles , by getting the claims from the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

            //var claimsInToken = securityToken.Claims.ToList();
            var rolesInToken = securityToken.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();

            rolesInToken.Count.Should().Be(2);

            //The below tests fail unless we set all paramters in the token to the same on the server like issuer, audience etc.
            //& the list of inbuilt role types need to included for a total of 5

            //var claimsList = new List<Claim>();
            //foreach (var role in rolesList)
            //{
            //    claimsList.Add(new Claim(ClaimTypes.Role, role));
            //};

            //claimsInToken.Should().BeEquivalentTo(claimsList);

        }
    }
}
