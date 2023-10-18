using BookingRoomHotel.Models.ModelsInterface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookingRoomHotel.Models
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateAccessToken(string id, string name, string role)
        {
            var claim = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, id),
                            new Claim(ClaimTypes.Name, name),
                            new Claim(ClaimTypes.Role, role)
                        };
            var claimsIdentity = new ClaimsIdentity(claim, "Admin");
            var tokenHandler = new JwtSecurityTokenHandler();
            var _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]));
            var tokenDecription = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDecription);
            return tokenHandler.WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            throw new NotImplementedException();
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            throw new NotSupportedException();
        }
    }
}
