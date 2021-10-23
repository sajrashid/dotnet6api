// <copyright file="RolesController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MyAPI.Models;
    using MyAPI.Repository;

    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRolesRepository repo;

        public RolesController(IRolesRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Roles>>> Get()
        {
            var roles = await this.repo.GetAllRoles().ConfigureAwait(false);
            return this.Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Roles>> Get(int id)
        {
            var product = await this.repo.GetRoles(id).ConfigureAwait(false);
            if (product is null)
            {
                return this.NotFound();  // return 404
            }

            return this.Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            await this.repo.DeleteRoles(id).ConfigureAwait(false);
            return this.Ok(id);
        }

        [HttpPost]
        public async Task<ActionResult<Roles>> Post([FromBody] Roles roles)
        {
            var createdRole = await this.repo.InsertRoles(roles).ConfigureAwait(false);
            return this.StatusCode(201, createdRole);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Put(int id, [FromBody] Roles roles)
        {
            var rowsAffected = await this.repo.UpdateRoles(id, roles);

            // check if product collection is not empty
            if (rowsAffected == 0)
            {
                return this.NotFound();
            }

            return this.Ok(roles);
        }
    }
}
