using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Capstone.Models.Entities;
using CapstoneDAL.Models;

namespace Capstone.Models.Entities
{
    public class Ticket
    {
        [Key]
        [Column(TypeName = "nvarchar(36)")]
        public string Id { get; set; }

        [Required]
        public string Subject { get; set; }  // Ticket subject

        [Required]
        public string Priority { get; set; }  // Ticket priority

        [Required]
        public string Status { get; set; } = "NEW";  // Default status is set to "NEW"

        // Store agent name (optional)
        public string? AssignedAgent { get; set; }  // Make it nullable

        // Nullable AgentId for foreign key to DisplayAgent
        public long? AgentId { get; set; }

        public DateTime CreatedAt { get; set; } // Created timestamp

        public DateTime UpdatedAt { get; set; } // Updated timestamp

        public long UserId { get; set; } // User ID, no navigation property

        [JsonIgnore]
        public DisplayAgent AssignedAgentEntity { get; set; } // Navigation property to DisplayAgent


        [JsonIgnore]
        public virtual ICollection<MessageTable>? Messages { get; set; } = new List<MessageTable>();


        [ForeignKey("UserId")]
        public UserDetails User { get; set; }

        // Constructor to initialize default values
        public Ticket()
        {
            Id = GenerateAlphanumericID();  // Generate a unique alphanumeric ID
            CreatedAt = DateTime.UtcNow;  // Initialize CreatedAt to UTC now
            UpdatedAt = DateTime.UtcNow;  // Initialize UpdatedAt to UTC now
        }

        // Helper method to generate a random alphanumeric ID
        private string GenerateAlphanumericID()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        }

        // Method to update the UpdatedAt property whenever changes are made
        public void OnUpdate()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
