
using Dapper;

using MySql.Data.MySqlClient;

using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

using Xunit;

namespace TestProject
{
    [CollectionDefinition(name: "TokenGenerator")]
    public class DbFixtureDefinition : ICollectionFixture<DbFixture> { }
    public class DbFixture
    {
        const string host = "localhost";
        const string password = "Root0++";
        const string userid = "root";
        const string connectionString = $"server={host}; userid={userid};pwd={password};port='3306';database='testdb'";
        private string sqlscript = null;

        public DbFixture()
        {
            IDbConnection Conn = new MySqlConnection(connectionString);

            using (StreamReader reader = new("./sql-scripts/mysql-setup.sql"))
            {
                sqlscript = reader.ReadToEnd();
            }
            // run script create tables, db etc..
            using (Conn)
            {
             //   Conn.Open();
             //   Conn.ExecuteAsync(sqlscript);
            }
            // create test data 
            using (Conn)
            {
               // Conn.Open();
            //    var testData = new TestData();
                //testData.CreateDataAsync().Wait();
            }
            // return mysl connection
            Conn.Open();
        }

       
        public MySqlConnection Conn { get; private set; }
    }
}
