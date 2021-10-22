using Microsoft.AspNetCore.Mvc;

using MyAPI.Models;

using MyAPI.Repository;
using MyAPI.Services;

namespace MyAPI.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository repo;
        public AccountsController(IAccountRepository repo)
        {
            this.repo = repo;
        }

        // GET:all user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            var users = await repo.GetAllUsers().ConfigureAwait(false);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await repo.GetUserById(id).ConfigureAwait(false);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }

        // Create account
        [HttpPost]
        public async Task<ActionResult<User>> Post(Auth auth)
        {
            var existingUser = await repo.GetUserByEmail(auth.Email).ConfigureAwait(false);

            if (existingUser is null)
            {
                var salt = SecurityHelper.GenerateSalt();
#pragma warning disable CS8604 // Possible null reference argument. // not required validation rules ensure caanot be null or empty
                var hashed = SecurityHelper.GenerateHash(salt, auth.Password);
#pragma warning restore CS8604 // Possible null reference argument.

                await repo.CreateUser(salt, hashed, auth.Email).ConfigureAwait(false);
                return Ok("Account Created, Please Login.");
            }

            return Conflict("Account already Exists.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            await repo.DeleteUser(id).ConfigureAwait(false);
            return Ok(id);
        }
    }
}
