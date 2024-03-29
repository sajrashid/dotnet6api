﻿// <copyright file="IRolesRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Repository
{
    using MyAPI.Models;

    public interface IRolesRepository
    {
        public Task<IEnumerable<Roles>> GetAllRoles();

        public Task<Roles> GetRoles(int id);

        public Task<Roles> InsertRoles(Roles role);

        public Task<int> UpdateRoles(int id, Roles role);

        public Task<int> DeleteRoles(int id);
    }
}
