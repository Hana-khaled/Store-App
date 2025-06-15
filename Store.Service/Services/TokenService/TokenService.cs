using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Store.Data.Entities.IdentityEntities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]));
        }
        public string GenerateToken(AppUser appUser)
        {
            // 1- handle claims user info(not sensitive) that will be in payload of token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, appUser.Email),
                new Claim(ClaimTypes.GivenName, appUser.DisplayName),
                new Claim("UserId", appUser.Id),// Custom Claims
                new Claim("UserName", appUser.UserName)
            };

            // 2- handle SigningCredentials (verify signature) using encoded key with encoding alg
            //    check key, diff key -> Unauthorized
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256);

            // 3- Token Descriptor(claims -> Subject(ClaimsIdentity), creds -> SigningCredientials, extra info)
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _configuration["Token:Issuer"],
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            // 4- token handler, defining type (JWT, Outh, ...)
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }
    }
}
