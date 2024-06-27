using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataAccess.Enities;
using Microsoft.IdentityModel.Tokens;
using Services.Configurations;

namespace Services.Auth
{
    public class JwtTokenService(JwtSettings jwtSettings)
    {
        private readonly JwtSettings jwtSettings = jwtSettings;

        public string GenerateToken(Employer employer)
        {

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, employer.Id.ToString()),
                new Claim(ClaimTypes.Name, employer.FirstName),
                new Claim(ClaimTypes.Email, employer.Email),
                new Claim(ClaimTypes.NameIdentifier, employer.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtSettings.ExpirationInMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
