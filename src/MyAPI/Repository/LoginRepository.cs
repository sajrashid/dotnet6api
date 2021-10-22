using Dapper;

using MyAPI.Models;

using System.Data;

namespace MyAPI.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IDbConnection _conn;

        public LoginRepository(IDbConnection conn)
        {
            _conn = conn;
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            using (_conn)
            {
                return await _conn.QueryAsync<User>("SELECT * FROM Users;").ConfigureAwait(false);
            }
        }
        public async Task<User> GetUserByEmail(string email)
        {
            var dictionary = new Dictionary<string, object>
            {
                { "@email", email }
            };

            using (_conn)
            {
                var parameters = new DynamicParameters(dictionary);
                const string? sql = "SELECT * FROM Users WHERE email = @email";
                return await _conn.QueryFirstOrDefaultAsync<User>(sql, parameters).ConfigureAwait(false);
            }
        }

    }
}
