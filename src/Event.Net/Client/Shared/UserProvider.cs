using Microsoft.AspNetCore.Components.Authorization;

namespace Event.Net.Client.Shared
{
    public class UserProvider
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public UserProvider(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<string> GetUserIdAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

            if(authState.IsAuthenticated())
                return authState.User.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value;

            return string.Empty;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

            return authState.IsAuthenticated();
        }
    }
}
