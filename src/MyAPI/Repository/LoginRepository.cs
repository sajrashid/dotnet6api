// <copyright file="LoginRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Repository
{
    using System.Data;
    using Dapper;
    using MyAPI.Models;

    public class LoginRepository : ILoginRepository
    {
        private readonly IDbConnection conn;

        public LoginRepository(IDbConnection conn)
        {
            this.conn = conn;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            using (this.conn)
            {
                return await this.conn.QueryAsync<User>("SELECT * FROM Users;").ConfigureAwait(false);
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var dictionary = new Dictionary<string, object>
            {
                { "@email", email },
            };

            using (this.conn)
            {
                var parameters = new DynamicParameters(dictionary);
                const string? sql = "SELECT * FROM Users WHERE email = @email";
                return await this.conn.QueryFirstOrDefaultAsync<User>(sql, parameters).ConfigureAwait(false);
            }
        }
    }
}
