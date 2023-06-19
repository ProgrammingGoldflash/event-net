using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Net.Server.Models
{
    public class Event
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Created { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpdated { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
