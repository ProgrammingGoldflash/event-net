using Event.Net.Client.Shared;
using Event.Net.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MudBlazor;
using System.Net.Http.Json;

namespace Event.Net.Client.Pages.Events
{
    public partial class Events : ComponentBase
    {
        private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Events", href: "/events", icon: Icons.Material.Filled.Event)
        };

        private EventDto[]? events;
        private string userId;

        //protected override async Task OnInitializedAsync()
        //{

        //}

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                await GetClaimsPrincipalData();
                Console.WriteLine(userId);

                var http = HttpFactory.CreateClient(Constants.PublicApi);

                events = await http.GetFromJsonAsync<EventDto[]>("api/events");
            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
            }
        }

        private async Task GetClaimsPrincipalData()
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                userId = user.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            }
        }
    }
}
