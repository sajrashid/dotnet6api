using Dapper;

using MyAPI.Models;
using MyAPI.Repository;
using MyAPI.Services;

using MySql.Data.MySqlClient;

using System;
using System.Data;
using System.Threading.Tasks;

namespace TestProject
{
    public class TestData : IDisposable
    {
        private readonly IDbConnection conn;
        public TestData()
        {
            string connectionString = SettingsFile.Instance.ConnString;

            conn = new MySqlConnection(connectionString);
        }
        public async Task CreateDataAsync()
        {
            // init account repo and db connection
            var accountRepo = new AccountRepository(conn);

            // Create User 1
            var user = CreateTestUser("someUser@someEmail.com", "Password99");
            await accountRepo.CreateUser(user.Salt, user.Hash, user.Email).ConfigureAwait(false);

            // Create User 2
            var user2 = CreateTestUser("OtherUSer@someEmail.co.uk", "password9");
            await accountRepo.CreateUser(user2.Salt, user2.Hash, user2.Email).ConfigureAwait(false);

            // init Visitor repo and db connection
            var visitorRepo = new VisitorRepository(conn);

            // Create Test Visitor 1
            var visitor = CreateTestVisitor("Chrome", "1.2.4.5");
            await visitorRepo.InsertVisitor(visitor).ConfigureAwait(false);

            // Create Test Visitor 2
            var visitor2 = CreateTestVisitor("Chrome", "111.222.224.5222");
            await visitorRepo.InsertVisitor(visitor2).ConfigureAwait(false);
        }

        public void CreateProducts()
        {
            var productsRepo = new ProductRepository(conn);

            for (int i = 0; i < 100; i++)
            {
                var product = new Product()
                {
                    Company = Faker.CompanyFaker.Name(),
                    Phone = Faker.PhoneFaker.InternationalPhone(),
                    Price = Faker.NumberFaker.Number(1, 100),
                    InStock = Faker.BooleanFaker.Boolean(),
                    StockCount = Faker.NumberFaker.Number(1, 100),
                    NewStockDate = Faker.DateTimeFaker.DateTimeBetweenDays(1, 30)
                };
                productsRepo.InsertProduct(product).ConfigureAwait(false);
            }
        }

        public void DeleteAccounts()
        {
            using (conn)
            {
                conn.Execute("SET foreign_key_checks = 0;TRUNCATE TABLE Users;SET foreign_key_checks = 1;");
            }
        }

        public void DeleteProducts()
        {
            using (conn)
            {
                conn.Execute("TRUNCATE TABLE Products;");
            }
        }

        public void DeleteVisitors()
        {
            using (conn)
            {
                conn.Execute("TRUNCATE TABLE Visitors");
            }
        }

        public void CreateUserAndRoles()
        {
            var accountRepo = new AccountRepository(conn);

            var salt = SecurityHelper.GenerateSalt();
            var hash = SecurityHelper.GenerateHash(salt, "Password99");
            var user = new User()
            {
                Email = "testEmail@email.com",
                Salt = salt,
                Hash = hash,
                LastVisit = DateTime.Now,
            };
            accountRepo.CreateUser(user.Salt, user.Hash, user.Email).ConfigureAwait(false);

            var rolesRepo = new RolesRepository(conn);
            var role = new Roles()
            {
                Role = "Admin",
                UserId = 1
            };
            rolesRepo.InsertRoles(role).ConfigureAwait(false);
            var role2 = new Roles()
            {
                Role = "Role2",
                UserId = 1
            };
            rolesRepo.InsertRoles(role).ConfigureAwait(false);
        }

        public void DeleteUserAndRoles()
        {
            using (conn)
            {
                using (conn)
                {
                    conn.Execute("SET foreign_key_checks = 0;TRUNCATE TABLE Users; TRUNCATE TABLE Roles;SET foreign_key_checks = 1;");
                }
            }
        }

        public User CreateTestUser(string email, string password)
        {
            var accountRepo = new AccountRepository(conn);

            // generate a 128-bit salt using a secure PRNG
            var salt = SecurityHelper.GenerateSalt();
            var hash = SecurityHelper.GenerateHash(salt, password);
            var user = new User()
            {
                Email = email,
                Salt = salt,
                Hash = hash,
                LastVisit = DateTime.Now,
            };
            accountRepo.CreateUser(user.Salt, user.Hash, user.Email).ConfigureAwait(false);
            return user;
        }

        public void CreateListTestUsers()
        {
            var accountsRepo = new AccountRepository(conn);
            for (int i = 0; i < 10; i++)
            {
                var email = Faker.InternetFaker.Email();
                var password = Faker.StringFaker.Randomize("PaS$w0rD99");

                var salt = SecurityHelper.GenerateSalt();
                var hash = SecurityHelper.GenerateHash(salt, password);
                accountsRepo.CreateUser(salt, hash, email).ConfigureAwait(false);
            }
        }
        public Visitor CreateTestVisitor(string userAgent, string ip)
        {
            return new Visitor()
            {
                UserAgent = userAgent,  //Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.71 Safari/537.36
                Hash = SecurityHelper.CreateMD5Hash(userAgent + ip),
                IP = ip,
                Count = 1,
                LastVisit = DateTime.Now,
            };
        }
        public void Dispose()
        {
            conn.Close();
        }
    }
}