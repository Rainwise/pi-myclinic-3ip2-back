using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PRA_1.Security
{
    public class JwtTokenProvider
    {
        public static string CreateToken(string secureKey, int expirationMinutes, string email, string role, string? username = null)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secureKey);
            var signingKey = new SymmetricSecurityKey(keyBytes);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Sub, email),
            new Claim(ClaimTypes.Name, username ?? email)
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                SigningCredentials = new SigningCredentials(
                    signingKey,
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
    }
}
