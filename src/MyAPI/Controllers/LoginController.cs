
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;

using MyAPI.Models;
using MyAPI.Repository;
using MyAPI.Services;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {
        private readonly ILoginRepository repo;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        public LoginController(ILoginRepository repo, IConfiguration config, ITokenService tokenService)
        {
            this.repo = repo;
            _tokenService = tokenService;
            _config = config;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var users = await repo.GetAllUsers().ConfigureAwait(false);
            return Ok(users);
        }

        [HttpPost("")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> Post([FromBody] Auth auth)
        {
            var user = await repo.GetUserByEmail(auth.Email).ConfigureAwait(false);
            if (user is not null)
            {
                // existing user lets test the password

                var salt = user.Salt;

                // we need to recreate the hash from the users saved salt
                string hashed = getHash(auth, salt);

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
                    var tokenString = _tokenService.CreateToken(rolesList);

                    // return token
                    return Ok(new { Token = tokenString, Message = "Success" });
                }
            }
            // erm nope
            return StatusCode(403, "Login failed check your Email or Password");
        }
        private static string getHash(Auth auth, byte[]? salt)
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
