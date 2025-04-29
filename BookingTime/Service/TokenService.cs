using BookingTime.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookingTime.Service
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration) { 
            _configuration = configuration;
        }

        public string GenerateJwtToken(User user)
        {
            // Define the secret key and algorithm for signing the token
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenSecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Set the claims for the JWT token (user information)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),  // Subject: the user's email
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  // JWT ID (unique)
                new Claim(ClaimTypes.Name, user.Email),  // User's full name (example)
                new Claim("IsVerified", user.IsVerified.ToString())  // Additional claim indicating if the account is verified
             };

            // Create the JWT token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),  // Set the token expiry time
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return the JWT token as a string
            return tokenHandler.WriteToken(token);
        }
    }
}
