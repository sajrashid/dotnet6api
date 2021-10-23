// <copyright file="RolesRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Repository
{
    using System.Data;
    using Dapper;
    using MyAPI.Models;

    public class RolesRepository : IRolesRepository
    {
        private readonly IDbConnection conn;

        public RolesRepository(IDbConnection conn)
        {
            this.conn = conn;
        }

        public async Task<IEnumerable<Roles>> GetAllRoles()
        {
            using (this.conn)
            {
                return await this.conn.QueryAsync<Roles>("SELECT * FROM Roles;").ConfigureAwait(false);
            }
        }

        public async Task<Roles> GetRoles(int id)
        {
            var dictionary = new Dictionary<string, object>
            {
                { "@id", id },
            };

            using (this.conn)
            {
                var parameters = new DynamicParameters(dictionary);
                const string? sql = "SELECT * FROM Roles WHERE id = @id";
                return await this.conn.QueryFirstOrDefaultAsync<Roles>(sql, parameters).ConfigureAwait(false);
            }
        }

        public async Task<int> DeleteRoles(int id)
        {
            var dictionary = new Dictionary<string, object>
            {
                { "@id", id },
            };
            var parameters = new DynamicParameters(dictionary);
            const string? sql = "Delete FROM Roles WHERE id = @id";

            using (this.conn)
            {
                await this.conn.QueryFirstOrDefaultAsync<Roles>(sql, parameters).ConfigureAwait(false);
                return id;
            }
        }

        public async Task<int> UpdateRoles(int id, Roles role)
        {
            var parameters = role;
            const string? sql = "UPDATE Roles SET Role = @role,UserId =@userid WHERE id =@Id;";
            using (this.conn)
            {
                return await this.conn.ExecuteAsync(sql, parameters).ConfigureAwait(false);
            }
        }

        public async Task<Roles> InsertRoles(Roles role)
        {
            var parameters = role;
            const string? sql = "Insert INTO Roles SET Role = @role,UserId =@userid; select LAST_INSERT_ID();";
            using (this.conn)
            {
                role.Id = await this.conn.ExecuteScalarAsync<int>(sql, parameters).ConfigureAwait(false);
                return role;
            }
        }
    }
}
