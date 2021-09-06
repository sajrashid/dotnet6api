using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using API.DTOs;
using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Logging;
using System.Formats.Asn1;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string connString;
        private readonly ILogger<UserController> _logger;

        public UserController(IConfiguration configuration, ILogger<UserController> logger)
        {
            _configuration = configuration;
            _logger = logger;
            var host = _configuration["DBHOST"] ?? "localhost";
            var port = _configuration["DBPORT"] ?? "3306";
            var password = _configuration["MYSQL_PASSWORD"] ?? _configuration.GetConnectionString("MYSQL_PASSWORD");
            var userid = _configuration["MYSQL_USER"] ?? _configuration.GetConnectionString("MYSQL_USER");
            var usersDataBase = _configuration["MYSQL_DATABASE"] ?? _configuration.GetConnectionString("MYSQL_DATABASE");

            connString = $"server={host}; userid={userid};pwd={password};port={port};database={usersDataBase}";
        }

        public UserController()
        {
        }

        private async Task<List<UsersDto>> DoesUserExist(string iP)
        {
            var users = new List<UsersDto>();
            string query = @"SELECT * FROM Users where IP='" + iP + "'";

            using (var connection = new MySqlConnection(this.connString))
            {
                var result = await connection.QueryAsync<UsersDto>(query, CommandType.Text);
                users = result.ToList();
            }

            return users;
        }

        [HttpGet("GetImg")]
        public async Task<ActionResult> GetImg()
        {
            var newUser = new UsersDto();
            var userAgent = this.HttpContext.Request.Headers["User-Agent"].ToString();
            newUser.UserAgent = userAgent;
            var remoteIpAddress = this.HttpContext.Connection.RemoteIpAddress;


            var usersList = await this.DoesUserExist(remoteIpAddress.ToString());
            using var connection = new MySqlConnection(this.connString);

            if (usersList.Count < 1)
            {
                // new user IP not found
                try
                {
                    string query = @"INSERT INTO Users (UserAgent,IP,LastVisit,Count) VALUES (@UserAgent,@IP,@LastVisit,@Count)";
                    var result = await connection.ExecuteAsync(query, new UsersDto() { UserAgent = userAgent, IP = remoteIpAddress.ToString(), LastVisit = DateTime.Now, Count = 1 });
                }
                catch (Exception e)
                {
                    var ee = e;
                    Console.WriteLine(ee.Message);
                    return this.StatusCode(500, "Unable To Process Request");
                }
            }
            else
            {
                var count = usersList[0].Count;
                count++;
                string query = @"Update Users Set Count=" + count + " WHERE Id=" + usersList[0].Id + ";";
                var result = await connection.ExecuteAsync(query);
            }

            return new StatusCodeResult(200);
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<List<UsersDto>>> GetAllUsers()
        {
            var users = new List<UsersDto>();
            try
            {
                string query = @"SELECT * FROM Users";
                using (var connection = new MySqlConnection(this.connString))
                {
                    var result = await connection.QueryAsync<UsersDto>(query, CommandType.Text);
                    users = result.ToList();
                }
                if (users.Count > 0)
                {
                    return Ok(users);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Unable To Process Request");
            }
        }


        // create a http put request to update a user
        [HttpPut("UpdateUser")] // update a user    
        public async Task<ActionResult<UsersDto>> UpdateUser(UsersDto user)
        {   // update a user
            try
            {
                string query = @"UPDATE Users SET UserAgent=@UserAgent,IP=@IP,LastVisit=@LastVisit,Count=@Count WHERE Id=@Id";
                using (var connection = new MySqlConnection(this.connString))
                {
                    var result = await connection.ExecuteAsync(query, user);
                }
                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500, "Unable To Process Request");
            }   
        }   





            


      //  [HttpPost("AddNewUser")]
        //public async Task<ActionResult<UsersDto>> AddNewUser(UsersDto user)
        //{
        //    var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();
        //    var newUser = new UsersDto();
        //    newUser.UserAgent = userAgent;

        //    try
        //    {
        //        string query = @"INSERT INTO Users (UserAgent,c) VALUES (@UserAgent,@IP)";
        //        var param = new DynamicParameters();
        //        param.Add("@UserAgent", user.UserAgent);
        //        param.Add("@IP", user.IP);
        //        using (var connection = new MySqlConnection(connString))
        //        {
        //            var result = await connection.ExecuteAsync(query, param, null, null, CommandType.Text);
        //            if (result > 0)
        //            {
        //                newUser = user;
        //            }
        //        }
        //        if (newUser != null)
        //        {
        //            return Ok(newUser);
        //        }
        //        else
        //        {
        //            return BadRequest("Unable To  User");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        var ee=e;
        //        return StatusCode(500, "Unable To Process Request");
        //    }
        //}
    }
}