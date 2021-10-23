// <copyright file="ITokenService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Services
{
    public interface ITokenService
    {
        string CreateToken(List<string> rolesList);

        Task<bool> ValidateToke(string token);
    }
}
