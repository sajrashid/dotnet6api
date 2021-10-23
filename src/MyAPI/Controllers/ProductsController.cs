// <copyright file="ProductsController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using MyAPI.Models;
    using MyAPI.Repository;

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
            var products = await this.repo.GetAllProducts().ConfigureAwait(false);
            return this.Ok(products);
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await this.repo.GetProduct(id).ConfigureAwait(false);
            if (product is null)
            {
                return this.NotFound();  // return 404
            }

            return this.Ok(product);
        }

        [HttpDelete("{id}")]

        // DELETE api/<ValuesController>/5
        public async Task<ActionResult<int>> Delete(int id)
        {
            await this.repo.DeleteProduct(id).ConfigureAwait(false);
            return this.Ok(id);
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<Product>> Post([FromBody] Product product)
        {
            var createdProduct = await this.repo.InsertProduct(product).ConfigureAwait(false);
            return this.StatusCode(201, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Put(int id, [FromBody] Product product)
        {
            var rowsAffected = await this.repo.UpdateProduct(id, product).ConfigureAwait(false);

            // check if product collection is not empty
            if (rowsAffected == 0)
            {
                return this.NotFound();
            }

            return this.Ok(product);
        }
    }
}
