﻿namespace Event.Net.Shared
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        public int EventId { get; set; }
        public string? UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
