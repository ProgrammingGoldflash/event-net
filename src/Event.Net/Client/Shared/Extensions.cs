using Microsoft.AspNetCore.Components.Authorization;

namespace Event.Net.Client.Shared
{
    public static class Extensions
    {
        public static bool IsAuthenticated(this AuthenticationState? state)
        {
            if (state?.User?.Identity == null || state.User.Identity.IsAuthenticated is false)
                return false;

            return true;
        }
    }
}
