using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Net.Server.Models
{
    public class Review
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public DateTime Created { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
