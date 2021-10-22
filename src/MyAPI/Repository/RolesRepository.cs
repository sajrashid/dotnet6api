using Dapper;

using MyAPI.Models;

using System.Data;

namespace MyAPI.Repository
{
    public class RolesRepository : IRolesRepository
    {
        private readonly IDbConnection _conn;

        public RolesRepository(IDbConnection conn)
        {
            _conn = conn;
        }

        public async Task<IEnumerable<Roles>> GetAllRoles()
        {
            using (_conn)
            {
              return await _conn.QueryAsync<Roles>("SELECT * FROM Roles;").ConfigureAwait(false);
            }
        }

        public async Task<Roles> GetRoles(int id)
        {
            var dictionary = new Dictionary<string, object>
            {
                { "@id", id }
            };

            using (_conn)
            {
                var parameters = new DynamicParameters(dictionary);
                const string? sql = "SELECT * FROM Roles WHERE id = @id";
                return await _conn.QueryFirstOrDefaultAsync<Roles>(sql, parameters).ConfigureAwait(false);
            }

        }
        public async Task<int> DeleteRoles(int Id)
        {
            var dictionary = new Dictionary<string, object>
            {
                { "@id",Id }
            };
            var parameters = new DynamicParameters(dictionary);
            const string? sql = "Delete FROM Roles WHERE id = @id";

            using (_conn)
            {
                await _conn.QueryFirstOrDefaultAsync<Roles>(sql, parameters).ConfigureAwait(false);
                return Id;
            }
        }

        public async Task<int> UpdateRoles(int id, Roles role)
        {
            var parameters = role;
            const string? sql = "UPDATE Roles SET Role = @role,UserId =@userid WHERE id =@Id;";
            using (_conn)
            {
                return await _conn.ExecuteAsync(sql, parameters).ConfigureAwait(false);
            }
        }

        public async Task<Roles> InsertRoles(Roles role)
        {
            var parameters = role;
            const string? sql = "Insert INTO Roles SET Role = @role,UserId =@userid; select LAST_INSERT_ID();";
            using (_conn)
            {
                
                role.Id = await _conn.ExecuteScalarAsync<int>(sql, parameters).ConfigureAwait(false);
                return role;
            }
        }
    }
}
