using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Controllers;
using Dapper;
using API.DTOs;

namespace Api.services
{
    public class CreateData
    {

		public async Task<ActionResult> GenerateProductsList()
		{
			var ProductList = new List<ProductsDto>();
			await Task.Run(() =>
			{
				for (int i = 0; i < 1000; i++)
				{
					var product = new ProductsDto();
					product.Company = Faker.CompanyFaker.Name();
					product.Phone = Faker.PhoneFaker.Phone();
					product.Price = Faker.NumberFaker.Number(1, 1000);
					product.InStock = Faker.BooleanFaker.Boolean();
					if (product.InStock == false)
					{
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


			var productList = await GenerateProductsList();

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
			//using (var connection = new MySqlConnection(this.connString))
			//{
			//	await connection.ExecuteAsync(createTableSql, CommandType.Text);
			//}
			return productList;
		}

		
	}
}
