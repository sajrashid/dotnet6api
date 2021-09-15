namespace Api.Controllers
{
    using System;
    using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
    using System.Text.Json;
    using System.Threading.Tasks;
using API.Controllers;
    using API.DTOs;

    using Dapper;

    using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using MySql.Data.MySqlClient;

    [Route("api/[controller]")]
    [ApiController]
    public class DataGenController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly string connString;
        private readonly ILogger<UserController> _logger;

        public DataGenController(IConfiguration configuration, ILogger<UserController> logger)
        {
            this.configuration = configuration;
            this._logger = logger;
            var host = this.configuration["DBHOST"] ?? "localhost";
            var port = this.configuration["DBPORT"] ?? "3306";
            var password = this.configuration["MYSQL_PASSWORD"] ?? this.configuration.GetConnectionString("MYSQL_PASSWORD");
            var userid = this.configuration["MYSQL_USER"] ?? this.configuration.GetConnectionString("MYSQL_USER");
            var usersDataBase = this.configuration["MYSQL_DATABASE"] ?? this.configuration.GetConnectionString("MYSQL_DATABASE");

            connString = $"server={host}; userid={userid};pwd={password};port={port};database={usersDataBase}";
        }

        private async Task<List<Product>> GenerateProductsList()
        {
            var ProductList = new List<Product>();
            await Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    var product = new Product();
                    product.Company = Faker.CompanyFaker.Name();
                    product.Phone = Faker.PhoneFaker.Phone();
                    product.Price = Faker.NumberFaker.Number(1, 1000);
                    product.InStock = Faker.BooleanFaker.Boolean();
                    if (product.InStock == false) {
                        product.StockCount = 0;
                    }
                    else
                    {
                        product.StockCount = Faker.NumberFaker.Number(1, 100);
                    }
                    product.NewStockDate = Faker.DateTimeFaker.DateTimeBetweenDays(1, 30);
                    ProductList.Add(product);
                }
            });


            return ProductList;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
           var productList=await GenerateProductsList();

            var createTableSql = @"CREATE TABLE IF NOT EXISTS Products (
                    Id INT AUTO_INCREMENT PRIMARY KEY,
                    Company VARCHAR(255) NOT NULL,
                    Price INT NOT NULL,
                    Phone VARCHAR(255) NOT NULL,
                    InStock BOOLEAN NOT NULL,
                    NewStockDate DATE NOT NULL,
                    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                );";

            //string query = $@"SELECT * FROM Users where IP= { iP }";
            //SqlBulkCopy objbulk = new SqlBulkCopy(this.connString);
            //objbulk.DestinationTableName = "tblTest";
            //objbulk.BatchSize(1000);
            //objbulk.SqlRowsCopied();
            using (var connection = new MySqlConnection(this.connString))
            {
                 await connection.ExecuteAsync(createTableSql, CommandType.Text);
            }
            return Ok (productList);
        }
    }


    public class Product
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string Phone {  get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public int StockCount { get; set; }
        public DateTime NewStockDate { get; set; }

    }
}