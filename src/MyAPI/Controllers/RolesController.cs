using Microsoft.AspNetCore.Mvc;

using MyAPI.Models;
using MyAPI.Repository;

namespace MyAPI.Controllers
{
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
            var Roles = await repo.GetAllRoles().ConfigureAwait(false);
            return Ok(Roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Roles>> Get(int id)
        {
            var product = await repo.GetRoles(id).ConfigureAwait(false);
            if (product is null)
            {
                return NotFound();  // return 404
            }
            return Ok(product);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            await repo.DeleteRoles(id).ConfigureAwait(false);
            return Ok(id);
        }

        [HttpPost]
        public async Task<ActionResult<Roles>> Post([FromBody] Roles roles)
        {
            var createdRole = await repo.InsertRoles(roles).ConfigureAwait(false);
            return StatusCode(201, createdRole);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Put(int id, [FromBody] Roles roles)
        {
            var rowsAffected = await repo.UpdateRoles(id, roles);
            // check if product collection is not empty
            if (rowsAffected == 0)
            {
                return NotFound();  // return 404        
            }
            return Ok(roles);
        }
    }
}
