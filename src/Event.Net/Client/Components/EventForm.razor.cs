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
                date = Model.EventDate;
        }

        private DateTime? date;
        private DateTime? Date
        {
            get => date;
            set
            {
                Model.EventDate = value.Value;
                date = value;
            }
        }
    }
}
