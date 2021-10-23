// <copyright file="AccountsController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MyAPI.Models;
    using MyAPI.Repository;
    using MyAPI.Services;

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
            var users = await this.repo.GetAllUsers().ConfigureAwait(false);
            return this.Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            var user = await this.repo.GetUserById(id).ConfigureAwait(false);
            if (user != null)
            {
                return this.Ok(user);
            }

            return this.NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<User>> Post(Auth auth)
        {
            var existingUser = await this.repo.GetUserByEmail(auth.Email).ConfigureAwait(false);

            if (existingUser is null)
            {
                var salt = SecurityHelper.GenerateSalt();
#pragma warning disable CS8604 // Possible null reference argument. // not required validation rules ensure caanot be null or empty
                var hashed = SecurityHelper.GenerateHash(salt, auth.Password);
#pragma warning restore CS8604 // Possible null reference argument.

                await this.repo.CreateUser(salt, hashed, auth.Email).ConfigureAwait(false);
                return this.Ok("Account Created, Please Login.");
            }

            return this.Conflict("Account already Exists.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            await this.repo.DeleteUser(id).ConfigureAwait(false);
            return this.Ok(id);
        }
    }
}
