using Microsoft.AspNetCore.Identity;

namespace Event.Net.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Event> Events { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}