using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserValidationAPI.Models;

namespace UserValidationAPI.Models.AuthenticationManager
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        private readonly UsersAPIContext _context;
        private readonly string _key;
        public JwtAuthenticationManager(UsersAPIContext context, string key)
        {
            _context = context;
            _key = key;
        }

        public string Authentication(string username, string password)
        {
            if(!_context.Users.Any(x=> x.UserName.Equals(username) && x.Password.Equals(password)))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
