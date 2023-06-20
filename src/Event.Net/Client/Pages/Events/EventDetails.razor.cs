using Event.Net.Client.Shared;
using Event.Net.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Net.Http.Json;

namespace Event.Net.Client.Pages.Events
{
    public partial class EventDetails
    {
        private EventDetailsDto model = new EventDetailsDto();
        private ReviewDto review = new ReviewDto();
        private string currentUserId;
        private HttpClient @public;
        private HttpClient api;
        private List<BreadcrumbItem> _items = new List<BreadcrumbItem>
        {
            new BreadcrumbItem("Events", href: "/events", icon: Icons.Material.Filled.Event)
        };

        [Parameter] public int Id { get; set; }

        [Inject] public IHttpClientFactory HttpFactory { get; set; }
        [Inject] public UserProvider UserProvider { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] private IDialogService DialogService { get; set; }


        protected override async Task OnParametersSetAsync()
        {
            currentUserId = await UserProvider.GetUserIdAsync();

            @public = HttpFactory.CreateClient(Constants.PublicApi);
            api = HttpFactory.CreateClient(Constants.Api);
            await LoadEventDetails();

            _items.Add(new BreadcrumbItem(model.EventName, href: $"/events/{Id}", icon: Icons.Material.Filled.Info));
        }

        private async Task LoadEventDetails()
        {
            model = await @public.GetFromJsonAsync<EventDetailsDto>($"api/events/{Id}");
        }

        private async Task ReviewDialogExpanded(bool expanded)
        {
            if(expanded)
            {
                var isAuthenticated = await UserProvider.IsAuthenticatedAsync();
                
                if (isAuthenticated is false)
                    NavigationManager.NavigateTo($"authentication/login?returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}");
            }
        }

        private async Task AddReview()
        {
            review.EventId = Id;

            var res = await api.PostAsJsonAsync<ReviewDto>("api/reviews", review);
            res.EnsureSuccessStatusCode();
            
            await LoadEventDetails();
        }

        private async Task DeleteReview(int reviewId)
        {
            bool? result = await DialogService.ShowMessageBox(
                "Delete",
                "Deleting can not be undone!",
                yesText: "Delete!", cancelText: "Cancel");

            if (result.HasValue && result.Value is true)
            {
                var res = await api.DeleteAsync($"api/reviews/{reviewId}");

                res.EnsureSuccessStatusCode();
                await LoadEventDetails();
            }
        }

        private async Task DeleteEvent()
        {
            bool? result = await DialogService.ShowMessageBox(
                "Delete",
                "Deleting can not be undone!",
                yesText: "Delete!", cancelText: "Cancel");

            if(result.HasValue && result.Value is true)
            {
                var res = await api.DeleteAsync($"api/events/{Id}");

                res.EnsureSuccessStatusCode();

                NavigationManager.NavigateTo("events");
            }
        }
    }
}
