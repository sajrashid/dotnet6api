using MyAPI.Models;

namespace MyAPI.Repository
{
    public interface ILoginRepository
    {
        // Get single users/developers/admins etc
        public Task<User> GetUserByEmail(string email);

        public Task<IEnumerable<User>> GetAllUsers();
        
    }
}
