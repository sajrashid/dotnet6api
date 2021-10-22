using Microsoft.AspNetCore.Builder;

using MyAPI.Services;

using System.Collections.Generic;

using Xunit;

namespace TestProject
{
    [CollectionDefinition(name: "TokenGenerator")]
    public class TokenGeneratorDefinition : ICollectionFixture<TokenFixture> { }
    public class TokenFixture
    {
        public string GetToken()
        {
            var builder = WebApplication.CreateBuilder();
            var config = builder.Configuration;
            // get a token for the testrun user
            TokenService tokenSvc = new(config);

            var roles = new List<string>
            {
                "Developer",
                "Adming"
            };

            return tokenSvc.CreateToken(roles);

        }
    }
}
