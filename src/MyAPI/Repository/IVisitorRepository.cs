// <copyright file="IVisitorRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Repository
{
    using MyAPI.Models;

    public interface IVisitorRepository
    {
        public Task<IEnumerable<Visitor>> GetAllVisitors();

        public Task<Visitor> GetVisitor(string hash);

        public Task<Visitor> InsertVisitor(Visitor visitor);

        public Task<int> UpdateVisitor(int id, Visitor visitor);

        public Task<int> DeleteVisitorById(int id);
    }
}
