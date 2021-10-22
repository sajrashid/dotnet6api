using Dapper;

using MyAPI.Models;

using System.Data;

namespace MyAPI.Repository
{
    public class VisitorRepository : IVisitorRepository
    {
        private readonly IDbConnection _conn;

        public VisitorRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        public async Task<int> DeleteVisitorById(int Id)
        {

            var dictionary = new Dictionary<string, object>
            {
                { "@id",Id }
            };
            var parameters = new DynamicParameters(dictionary);
            const string? sql = "Delete FROM Visitors WHERE Id = @Id";

            using (_conn)
            {
                await _conn.QueryFirstOrDefaultAsync<Visitor>(sql, parameters).ConfigureAwait(false);
                return Id;
            }

        }

        public async Task<IEnumerable<Visitor>> GetAllVisitors()
        {
            using (_conn)
            {
                return await _conn.QueryAsync<Visitor>("SELECT * FROM Visitors;").ConfigureAwait(false);
            }
        }

        public async Task<Visitor> GetVisitor(string hash)
        {
            var dictionary = new Dictionary<string, object>
            {
                { "@hash", hash }
            };
            using (_conn)
            {
                var parameters = new DynamicParameters(dictionary);
                const string? sql = "SELECT * FROM Visitors WHERE hash = @hash";
                return await _conn.QueryFirstOrDefaultAsync<Visitor>(sql, parameters).ConfigureAwait(false);
            }
        }

        public async Task<Visitor> InsertVisitor(Visitor visitor)
        {
            const string sql = "INSERT INTO Visitors (Hash,UserAgent,IP,LastVisit,Count) VALUES (@Hash,@UserAgent,@IP,@LastVisit,@Count)";

            using (_conn)
            {
                return await _conn.QueryFirstOrDefaultAsync<Visitor>(sql, visitor).ConfigureAwait(false);
            }
        }

        public async Task<int> UpdateVisitor(int id, Visitor visitor)
        {
            var parameters = visitor;
            const string? sql = "UPDATE Visitors SET Hash=@Hash,UserAgent=@UserAgent,IP=@IP,LastVisit=@LastVisit,Count=@Count WHERE id=@id";
            using (_conn)
            {
                return await _conn.ExecuteAsync(sql, parameters).ConfigureAwait(false);
            }
        }
    }
}
