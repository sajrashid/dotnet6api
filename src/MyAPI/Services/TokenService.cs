using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyAPI.Services
{
    public class TokenService : ITokenService
    {

        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;
        }
        public string CreateToken(List<string> rolesList)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claimsList = new List<Claim>();
            foreach (var role in rolesList)
            {
                claimsList.Add(new Claim(ClaimTypes.Role, role));
            };

            var token = new JwtSecurityToken(
              issuer: _config["Jwt:Issuer"],
              audience: _config["Jwt:Issuer"],
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
