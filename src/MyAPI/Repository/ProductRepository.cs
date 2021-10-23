// <copyright file="ProductRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Repository
{
    using System.Data;
    using Dapper;
    using MyAPI.Models;

    public class ProductRepository : IProductRepository
    {
        private readonly IDbConnection conn;

        public ProductRepository(IDbConnection conn)
        {
            this.conn = conn;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            using (this.conn)
            {
                return await this.conn.QueryAsync<Product>("SELECT * FROM Products;").ConfigureAwait(false);
            }
        }

        public async Task<Product> GetProduct(int id)
        {
            var dictionary = new Dictionary<string, object>
            {
                { "@id", id },
            };

            using (this.conn)
            {
                var parameters = new DynamicParameters(dictionary);
                const string? sql = "SELECT * FROM Products WHERE id = @id";
                return await this.conn.QueryFirstOrDefaultAsync<Product>(sql, parameters).ConfigureAwait(false);
            }
        }

        public async Task<int> DeleteProduct(int id)
        {
            var dictionary = new Dictionary<string, object>
            {
                { "@id", id },
            };
            var parameters = new DynamicParameters(dictionary);
            const string? sql = "Delete FROM Products WHERE id = @id";

            using (this.conn)
            {
                await this.conn.QueryFirstOrDefaultAsync<Product>(sql, parameters).ConfigureAwait(false);
                return id;
            }
        }

        public async Task<int> UpdateProduct(int id, Product productToUpDate)
        {
            var parameters = productToUpDate;
            const string? sql = "UPDATE Products SET Company = @company,Phone =@phone, Price = @price, InStock = @InStock,StockCount= @StockCount,NewStockDate= @NewStockDate WHERE id =@Id;";
            using (this.conn)
            {
                return await this.conn.ExecuteAsync(sql, parameters).ConfigureAwait(false);
            }
        }

        public async Task<Product> InsertProduct(Product productToInsert)
        {
            var parameters = productToInsert;
            const string? sql = "Insert INTO Products SET Company = @company,Phone =@phone, Price = @price, InStock = @InStock,StockCount= @StockCount,NewStockDate= @NewStockDate; select LAST_INSERT_ID();";
            using (this.conn)
            {
                productToInsert.Id = await this.conn.ExecuteScalarAsync<int>(sql, parameters).ConfigureAwait(false);
                return productToInsert;
            }
        }
    }
}
