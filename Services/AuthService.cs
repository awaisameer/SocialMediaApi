using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SocialMediaApp.Models;

namespace SocialMediaApp.Services
{
    public class AuthService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public AuthService(IOptions<AppSettings> settings)
        {
            _secretKey = settings.Value.SecretKey;
            _issuer = settings.Value.Issuer;
            _audience = settings.Value.Audience;
        }

        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),  // Combine FirstName and LastName
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Standard NameIdentifier claim
        new Claim("id", user.Id.ToString()) // Explicit custom 'id' claim
    };

            // Generate a SymmetricSecurityKey with _secretKey
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
