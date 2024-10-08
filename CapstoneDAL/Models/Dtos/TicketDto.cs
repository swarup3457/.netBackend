using CapstoneDAL.Models.Dtos;
using System.ComponentModel.DataAnnotations;

public class TicketDto
{
    [Required]
    public string Id { get; set; } = Guid.NewGuid().ToString(); // Automatically generate Id

    [Required]
    public string Subject { get; set; }

    [Required]
    public string Priority { get; set; }

    public string Status { get; set; } = "NEW"; // Default status is "NEW"

    public string? AssignedAgent { get; set; } // Store the agent's name, now nullable

    public int? AssignedAgentId { get; set; } // Nullable Assigned Agent ID

    public DateTime CreatedAt { get; set; } // Date of creation

    public DateTime UpdatedAt { get; set; } // Date of last update

    public List<MessageDto>Messages { get; set; }

    [Required] // Assuming UserId should always be present
    public long UserId { get; set; }

    // Default constructor
    public TicketDto()
    {
        CreatedAt = DateTime.UtcNow; // Initialize CreatedAt with the current UTC time
        UpdatedAt = DateTime.UtcNow; // Initialize UpdatedAt with the current UTC time
    }

    // Constructor with subject, priority, and userId
    public TicketDto(string subject, string priority, long userId, string? assignedAgent = null)
    {
        Id = Guid.NewGuid().ToString(); // Automatically generate Id
        Subject = subject;
        Priority = priority;
        UserId = userId;
        AssignedAgent = assignedAgent; // Allow this to be null
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    // Constructor with all properties
    public TicketDto(string id, string subject, string priority, string status, string? assignedAgent,
                     int? assignedAgentId, DateTime createdAt, DateTime updatedAt, long userId)
    {
        Id = id;
        Subject = subject;
        Priority = priority;
        Status = status;
        AssignedAgent = assignedAgent; // Set agent name directly, can be null
        AssignedAgentId = assignedAgentId; // Use ID for the agent instead of full object
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        UserId = userId;
    }
}
