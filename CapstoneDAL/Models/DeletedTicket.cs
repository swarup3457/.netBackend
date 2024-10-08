using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Capstone.Models.Entities;

namespace DeleteTickets.Models.Entities
{
    [Table("trash_tickets")]
    public class DeletedTicket
    {
        [Key]
        [StringLength(36)]
        public string Id { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Priority { get; set; }

        [Required]
        public string Status { get; set; } = "delete";

        public string AssignedAgent { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime DeletedAt { get; set; }

        public long? UserId { get; set; }

        public long? AgentId { get; set; }

        public string UserName { get; set; }

        // Parameterless constructor for EF
        public DeletedTicket() { }

        // Constructor to initialize from Ticket entity
        public DeletedTicket(Ticket ticket)
        {
            Id = ticket.Id;
            Subject = ticket.Subject;
            Priority = ticket.Priority;
            AssignedAgent = ticket.AssignedAgent;
            CreatedAt = ticket.CreatedAt;
            DeletedAt = DateTime.UtcNow; // Set the current timestamp

            if (ticket.User != null)
            {
                UserName = ticket.User.Name;
                UserId = ticket.User.Id;
            }

            if (ticket.AssignedAgentEntity != null)
            {
                AgentId = ticket.AssignedAgentEntity.Id;
            }
        }
    }

    // Assuming you have a Ticket class that looks something like this
   
}
