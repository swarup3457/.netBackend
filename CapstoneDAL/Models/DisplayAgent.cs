using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;
using System.Text.Json.Serialization;
using Capstone.Models.Entities;

namespace Capstone.Models
{
    [Table("display_agents")]  // Map to the table name "display_agents"
    public class DisplayAgent
    {
        [Key]  // Specify this property as the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.None)]  // Prevent auto-generation
        public long Id { get; set; }

        [Required]
        [StringLength(100)]  // Assuming a max length for the name field
        public string? Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]  // Assuming a max length for the email field
        public string? Email { get; set; }

        [Required]
        [StringLength(50)]  // Assuming a max length for the role field
        public string Role { get; set; } = "Agent";

        public bool AutoAssigned { get; set; }

        // Navigation property to Tickets
        [JsonIgnore]
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>(); // Navigation property to Tickets

        // Default constructor
        public DisplayAgent()
        {
            Tickets = new HashSet<Ticket>();  // Initialize collection
        }

        // Constructor with id
        public DisplayAgent(long id, string name, string email, bool autoAssigned)
        {
            Id = id;
            Name = name;
            Email = email;
            AutoAssigned = autoAssigned;
            Tickets = new HashSet<Ticket>();
        }

        // Constructor without id (for new records)
        public DisplayAgent(string name, string email, bool autoAssigned)
        {
            Name = name;
            Email = email;
            AutoAssigned = autoAssigned;
            Tickets = new HashSet<Ticket>();
        }
    }
}
