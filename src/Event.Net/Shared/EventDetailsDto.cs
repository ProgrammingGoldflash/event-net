﻿namespace Event.Net.Shared
{
    public class EventDetailsDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string? OrganizerId { get; set; }
        public int Rating { get; set; }
        public IEnumerable<ReviewDto> Reviews { get; set; }
    }
}
