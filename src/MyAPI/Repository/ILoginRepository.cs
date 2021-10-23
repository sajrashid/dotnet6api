// <copyright file="ILoginRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Repository
{
    using MyAPI.Models;

    public interface ILoginRepository
    {
        // Get single users/developers/admins etc
        public Task<User> GetUserByEmail(string email);

        public Task<IEnumerable<User>> GetAllUsers();
    }
}
