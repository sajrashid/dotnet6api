﻿// <copyright file="AccountRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Repository
{
    using System.Data;
    using Dapper;
    using MyAPI.Models;

    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnection conn;

        public AccountRepository(IDbConnection conn)
        {
            this.conn = conn;
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

        public async Task<User> CreateUser(byte[] salt, string hash, string email)
        {
            var user = new User()
            {
                Email = email,
                Salt = salt,
                Hash = hash,
                LastVisit = DateTime.Now,
            };

            const string? sql = "Insert INTO Users SET Email = @email,Salt = @salt, Hash = @hash, LastVisit = @lastVisit;";
            using (this.conn)
            {
                return await this.conn.QueryFirstOrDefaultAsync<User>(sql, user).ConfigureAwait(false);
            }
        }

        public Task<bool> ResetPassword(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteUser(int id)
        {
            using (this.conn)
            {
                var dictionary = new Dictionary<string, object>
                {
                    { "@id", id },
                };
                var parameters = new DynamicParameters(dictionary);
                const string? sql = "Delete FROM Users WHERE id = @id";
                return await this.conn.QueryFirstOrDefaultAsync<int>(sql, parameters).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            using (this.conn)
            {
                return await this.conn.QueryAsync<User>("SELECT * FROM Users;").ConfigureAwait(false);
            }
        }

        public async Task<User> GetUserById(int id)
        {
            var dictionary = new Dictionary<string, object>
            {
                { "@id", id },
            };

            using (this.conn)
            {
                var parameters = new DynamicParameters(dictionary);
                const string? sql = "SELECT * FROM Users WHERE id = @id";
                return await this.conn.QueryFirstOrDefaultAsync<User>(sql, parameters).ConfigureAwait(false);
            }
        }
    }
}
