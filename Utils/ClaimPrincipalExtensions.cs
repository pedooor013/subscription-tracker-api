using System.Security.Claims;

namespace StreamingSubscriptionTrackerAPI.Utils
{
    public static class ClaimPrincipalExtensions
    {
        public static long? GetOwnershipFilter(this ClaimsPrincipal user)
        {
            var userId = long.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));

            return user.IsInRole("Admin") ? null : userId;
        }
    }
}