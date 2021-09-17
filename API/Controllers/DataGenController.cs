namespace Api.Controllers
{
    using System;
    using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
using API.Controllers;
    using API.DTOs;

    using Dapper;

    using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;

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


        public  void BulkToMySQL()
        {
            this._logger.LogInformation(System.Reflection.MethodBase.GetCurrentMethod().Name);
            this._logger.LogInformation(this.connString);
            string ConnectionString = this.connString;
            StringBuilder sCommand = new StringBuilder("INSERT INTO Products (Company, Phone,Price,InStock,StockCount,NewStockDate) VALUES ");
            using (MySqlConnection mConnection = new MySqlConnection(ConnectionString))
            {
                List<string> Rows = new List<string>();
                for (int i = 0; i < 100000; i++)
                {

                    Rows.Add(string.Format("('{0}','{1}','{2}','{3}','{4}','{5}')"
                        , MySqlHelper.EscapeString(Faker.CompanyFaker.Name())
                        , MySqlHelper.EscapeString(Faker.PhoneFaker.Phone())
                        , Convert.ToDecimal(Faker.NumberFaker.Number(1, 1000))
                        , Faker.BooleanFaker.Boolean()
                        , Faker.NumberFaker.Number(1, 100)
                        , Faker.DateTimeFaker.DateTimeBetweenDays(1, 30)
                        ));
                }
                sCommand.Append(string.Join(",", Rows));
                sCommand.Append(";");
                try
                {
                    mConnection.Open();
                    this._logger.LogInformation("DataGen opening connection");
                    using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
                    {
                        myCmd.CommandType = CommandType.Text;
                        myCmd.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception e)
                {
                    this._logger.LogError("oops:" + e.Message);
                    throw;
                }
              
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            BulkToMySQL();
            return new StatusCodeResult(200);
        }
    }


   
}