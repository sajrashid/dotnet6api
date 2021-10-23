// <copyright file="VisitorRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Repository
{
    using System.Data;
    using Dapper;
    using MyAPI.Models;

    public class VisitorRepository : IVisitorRepository
    {
        private readonly IDbConnection conn;

        public VisitorRepository(IDbConnection conn)
        {
            this.conn = conn;
        }

        public async Task<int> DeleteVisitorById(int id)
        {
            var dictionary = new Dictionary<string, object>
            {
                { "@id", id },
            };
            var parameters = new DynamicParameters(dictionary);
            const string? sql = "Delete FROM Visitors WHERE Id = @Id";

            using (this.conn)
            {
                await this.conn.QueryFirstOrDefaultAsync<Visitor>(sql, parameters).ConfigureAwait(false);
                return id;
            }
        }

        public async Task<IEnumerable<Visitor>> GetAllVisitors()
        {
            using (this.conn)
            {
                return await this.conn.QueryAsync<Visitor>("SELECT * FROM Visitors;").ConfigureAwait(false);
            }
        }

        public async Task<Visitor> GetVisitor(string hash)
        {
            var dictionary = new Dictionary<string, object>
            {
                { "@hash", hash },
            };
            using (this.conn)
            {
                var parameters = new DynamicParameters(dictionary);
                const string? sql = "SELECT * FROM Visitors WHERE hash = @hash";
                return await this.conn.QueryFirstOrDefaultAsync<Visitor>(sql, parameters).ConfigureAwait(false);
            }
        }

        public async Task<Visitor> InsertVisitor(Visitor visitor)
        {
            const string sql = "INSERT INTO Visitors (Hash,UserAgent,IP,LastVisit,Count) VALUES (@Hash,@UserAgent,@IP,@LastVisit,@Count)";

            using (this.conn)
            {
                return await this.conn.QueryFirstOrDefaultAsync<Visitor>(sql, visitor).ConfigureAwait(false);
            }
        }

        public async Task<int> UpdateVisitor(int id, Visitor visitor)
        {
            var parameters = visitor;
            const string? sql = "UPDATE Visitors SET Hash=@Hash,UserAgent=@UserAgent,IP=@IP,LastVisit=@LastVisit,Count=@Count WHERE id=@id";
            using (this.conn)
            {
                return await this.conn.ExecuteAsync(sql, parameters).ConfigureAwait(false);
            }
        }
    }
}
