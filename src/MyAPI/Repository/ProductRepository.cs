using Dapper;

using MyAPI.Models;

using System.Data;

namespace MyAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection _conn;

        public ProductRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            using (_conn)
            {
                return await _conn.QueryAsync<Product>("SELECT * FROM Products;").ConfigureAwait(false);
            }
        }

        public async Task<Product> GetProduct(int id)
        {
            var dictionary = new Dictionary<string, object>
            {
                { "@id", id }
            };

            using (_conn)
            {
                var parameters = new DynamicParameters(dictionary);
                const string? sql = "SELECT * FROM Products WHERE id = @id";
                return await _conn.QueryFirstOrDefaultAsync<Product>(sql, parameters).ConfigureAwait(false);
            }

        }
        public async Task<int> DeleteProduct(int Id)
        {
            var dictionary = new Dictionary<string, object>
            {
                { "@id",Id }
            };
            var parameters = new DynamicParameters(dictionary);
            const string? sql = "Delete FROM Products WHERE id = @id";

            using (_conn)
            {
                await _conn.QueryFirstOrDefaultAsync<Product>(sql, parameters).ConfigureAwait(false);
                return Id;
            }
        }

        public async Task<int> UpdateProduct(int id, Product productToUpDate)
        {
            var parameters = productToUpDate;
            const string? sql = "UPDATE Products SET Company = @company,Phone =@phone, Price = @price, InStock = @InStock,StockCount= @StockCount,NewStockDate= @NewStockDate WHERE id =@Id;";
            using (_conn)
            {
                return await _conn.ExecuteAsync(sql, parameters).ConfigureAwait(false);
            }
        }

        public async Task<Product> InsertProduct(Product productToInsert)
        {
            var parameters = productToInsert;
            const string? sql = "Insert INTO Products SET Company = @company,Phone =@phone, Price = @price, InStock = @InStock,StockCount= @StockCount,NewStockDate= @NewStockDate; select LAST_INSERT_ID();";
            using (_conn)
            {
                productToInsert.Id = await _conn.ExecuteScalarAsync<int>(sql, parameters).ConfigureAwait(false);
                return productToInsert;
            }
        }
    }
}
