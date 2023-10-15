using System.Security.Claims;

namespace BookingRoomHotel.Models.ModelsInterface
{
    public interface ITokenService
    {
        string GenerateAccessToken(string id, string name, string role);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
