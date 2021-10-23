// <copyright file="IAccountRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Repository
{
    using MyAPI.Models;

    public interface IAccountRepository
    {
        public Task<User> GetUserById(int id);

        public Task<User> GetUserByEmail(string email);

        public Task<IEnumerable<User>> GetAllUsers();

        // Add a new account for a site admin developer etc
        public Task<User> CreateUser(byte[] salt, string hash, string email);

        public Task<int> DeleteUser(int id);
    }
}
