using System;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EVMDealerSystem.DataAccess.Models;

namespace EVMDealerSystem.BusinessLogic.Token
{
    public class ProvideToken
    {
        private readonly IConfiguration _configuration;


        public ProvideToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["AppSettings:SecretKey"];

            if (string.IsNullOrEmpty(secretKey))
            {

                throw new InvalidOperationException("SecretKey for JWT is not configured in AppSettings.");
            }

            var key = Encoding.ASCII.GetBytes(secretKey);


            var tokenExpires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["AppSettings:TokenDurationMinutes"] ?? "10"));

            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? ""),
            });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = tokenExpires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public DateTime GetTokenExpirationTime()
        {
            return DateTime.UtcNow.AddMinutes(double.Parse(_configuration["AppSettings:TokenDurationMinutes"] ?? "10"));
        }
    }
}