// <copyright file="LoginController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;
    using Microsoft.AspNetCore.Mvc;
    using MyAPI.Models;
    using MyAPI.Repository;
    using MyAPI.Services;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository repo;
        private readonly IConfiguration config;
        private readonly ITokenService tokenService;

        public LoginController(ILoginRepository repo, IConfiguration config, ITokenService tokenService)
        {
            this.repo = repo;
            this.tokenService = tokenService;
            this.config = config;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var users = await this.repo.GetAllUsers().ConfigureAwait(false);
            return this.Ok(users);
        }

        [HttpPost("")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Post([FromBody] Auth auth)
        {
            var user = await this.repo.GetUserByEmail(auth.Email).ConfigureAwait(false);
            if (user is not null)
            {
                // existing user lets test the password
                var salt = user.Salt;

                // we need to recreate the hash from the users saved salt
                string hashed = GetHash(auth, salt);

                // if the new hash matches the stored hash we are golden
                if (hashed == user.Hash)
                {
                    var rolesList = new List<string>
                    {
                        "XunitTestUser",
                        "TestUser",
                    };

                    // we have a match hooray
                    // create a token
                    var tokenString = this.tokenService.CreateToken(rolesList);

                    // return token
                    return this.Ok(new { Token = tokenString, Message = "Success" });
                }
            }

            // erm nope
            return this.StatusCode(403, "Login failed check your Email or Password");
        }

#pragma warning disable SA1011 // Closing square brackets should be spaced correctly
        private static string GetHash(Auth auth, byte[]? salt)
#pragma warning restore SA1011 // Closing square brackets should be spaced correctly
        {
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = System.Convert.ToBase64String(KeyDerivation.Pbkdf2(
#pragma warning disable CS8604 // Possible null reference argument.
                password: auth.Password,
                salt: salt,
#pragma warning restore CS8604 // Possible null reference argument.
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }
    }
}
