using Event.Net.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Event.Net.Client.Components
{
    public partial class EventForm : ComponentBase
    {
        [Parameter] public EventDto Model { get; set; } = new EventDto();
        [Parameter] public bool EditMode { get; set; }
        [Parameter] public EventCallback<EditContext> OnValidSubmit { get; set; }

        protected override void OnParametersSet()
        {
            if (Model != null && EditMode is true)
            {
                Date = Model.EventDate;
                Time = Model.EventDate.TimeOfDay;
            }
        }

        private async Task Submit(EditContext editContext)
        {
            Model.EventDate = Date.Value.Date + Time.Value;
            await OnValidSubmit.InvokeAsync(editContext);
        }

        private DateTime? Date { get; set; }
        private TimeSpan? Time { get; set; }
    }
}
