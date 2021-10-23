using Dapper;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using MySql.Data.MySqlClient;

using System.Data;
using System.IO;

using Xunit;
// Need to turn off test parallelization so we can validate the run order
[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestCollectionOrderer("XUnit.DisplayNameOrderer", "XUnit.Project")]

namespace TestProject.IntergrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            string sqlscript = string.Empty;
            using (StreamReader reader = new("./sql-scripts/mysql-setup.sql"))
            {
                sqlscript = reader.ReadToEnd();
            }
            var connectionString = SettingsFile.Instance.ConnString;
            builder.ConfigureServices(services =>
            {
                services.AddTransient(_ =>
                {
                    IDbConnection conn = new MySqlConnection(connectionString);

                    // run script create tables, db etc..
                    using (conn)
                    {
                        conn.Open();
                        conn.ExecuteAsync(sqlscript);
                    }
                    // create test data 

                    // return mysl connection
                    conn.Open();
                    return conn; // Replace SQL Lite with test DB
                });
            });
        }
    }
}
