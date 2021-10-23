// <copyright file="TokenService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace MyAPI.Services
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;

    public class TokenService : ITokenService
    {
        private readonly IConfiguration config;

        public TokenService(IConfiguration config)
        {
            this.config = config;
        }

        public string CreateToken(List<string> rolesList)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsList = new List<Claim>();
            foreach (var role in rolesList)
            {
                claimsList.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
              issuer: this.config["Jwt:Issuer"],
              audience: this.config["Jwt:Issuer"],
              claims: claimsList,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task<bool> ValidateToke(string token)
        {
            throw new NotImplementedException();
        }
    }
}
