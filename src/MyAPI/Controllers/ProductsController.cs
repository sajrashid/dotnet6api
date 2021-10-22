using Microsoft.AspNetCore.Mvc;

using MyAPI.Models;
using MyAPI.Repository;

namespace MyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository repo;

        public ProductsController(IProductRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var products = await repo.GetAllProducts().ConfigureAwait(false);
            return Ok(products);


        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await repo.GetProduct(id).ConfigureAwait(false);
            if (product is null)
            {
                return NotFound();  // return 404
            }
            return Ok(product);
        }


        [HttpDelete("{id}")]
        // DELETE api/<ValuesController>/5
        public async Task<ActionResult<int>> Delete(int id)
        {

            await repo.DeleteProduct(id).ConfigureAwait(false);
            return Ok(id);

        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<Product>> Post([FromBody] Product product)
        {
            var createdProduct = await repo.InsertProduct(product).ConfigureAwait(false);

            return StatusCode(201, createdProduct);

        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Put(int id, [FromBody] Product product)
        {
            var rowsAffected = await repo.UpdateProduct(id, product);
            // check if product collection is not empty
            if (rowsAffected == 0)
            {
                return NotFound();  // return 404        
            }
            return Ok(product);

        }
    }
}
