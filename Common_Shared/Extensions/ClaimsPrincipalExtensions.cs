using System.Security.Claims;

namespace Common_Shared.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(claim, out Guid userId) ? userId : Guid.Empty;
        }
    }
}