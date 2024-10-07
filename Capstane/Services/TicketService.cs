using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Capstone.Models;
using Capstone.Models.Entities;
using CapstoneDAL;

namespace Capstone.Services
{
    public class TicketService : ITicketService
    {
        private readonly CapstoneDbContext _context;

        public TicketService(CapstoneDbContext context)
        {
            _context = context;
        }

        // Create a new ticket based on the provided TicketDto
        public async Task<TicketDto> CreateTicketAsync(TicketDto ticketDto)
        {
            // Initialize a new Ticket entity using the DTO values
            var ticket = new Ticket
            {
                Subject = ticketDto.Subject,
                Priority = ticketDto.Priority,
                Status = ticketDto.Status ?? "NEW", // Default status is "NEW" if none is provided
                AssignedAgent = null, // Map AssignedAgent (string) to AssignedAgent
                UserId = ticketDto.UserId // Set UserId from DTO
            };

            // Add the new ticket to the context and save changes to the database
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            // Convert the saved ticket back to a DTO and return it
            return ConvertToDto(ticket);
        }

        // Retrieve all tickets
        public async Task<List<TicketDto>> GetAllTicketsAsync()
        {
            // Include the AssignedAgent details in the result
            var tickets = await _context.Tickets
                .Include(t => t.AssignedAgentEntity) // Eager load the AssignedAgent navigation property
                .ToListAsync();

            // Convert each Ticket entity to a TicketDto and return the list
            return tickets.ConvertAll(ConvertToDto);
        }

        // Retrieve a specific ticket by ID
        public async Task<TicketDto?> GetTicketByIdAsync(string id)
        {
            // Include the AssignedAgent details in the result
            var ticket = await _context.Tickets
                .Include(t => t.AssignedAgentEntity) // Corrected property name
                .FirstOrDefaultAsync(t => t.Id == id);

            // Convert the entity to a DTO and return it, or return null if not found
            return ticket != null ? ConvertToDto(ticket) : null;
        }

        // Update a ticket with new values
        public async Task<TicketDto?> UpdateTicketAsync(string id, TicketDto updatedTicketDto)
        {
            // Find the ticket in the database
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return null; // Return null if the ticket is not found
            }

            // Update the ticket properties with values from the DTO
            ticket.Subject = updatedTicketDto.Subject;
            ticket.Priority = updatedTicketDto.Priority;
            ticket.Status = updatedTicketDto.Status ?? ticket.Status; // Use existing status if not provided
            ticket.AssignedAgent = updatedTicketDto.AssignedAgent; // Update the AssignedAgent
            ticket.UpdatedAt = DateTime.UtcNow; // Update the last modified timestamp

            // Save the changes to the database
            await _context.SaveChangesAsync();

            // Convert the updated ticket back to a DTO and return it
            return ConvertToDto(ticket);
        }

        // Delete a ticket by ID
        public async Task DeleteTicketAsync(string id)
        {
            // Find the ticket in the database
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                // Remove the ticket from the context and save changes
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
            }
        }

        // Convert Ticket entity to TicketDto
        private TicketDto ConvertToDto(Ticket ticket)
        {
            return new TicketDto
            {
                Id = ticket.Id,
                Subject = ticket.Subject,
                Priority = ticket.Priority,
                Status = ticket.Status,
                AssignedAgent = ticket.AssignedAgent, // Correctly mapping AssignedAgent
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                UserId = ticket.UserId
            };
        }

        // Convert TicketDto back to Ticket entity (optional, if needed)
        private Ticket ConvertToEntity(TicketDto ticketDto)
        {
            return new Ticket
            {
                Id = ticketDto.Id,
                Subject = ticketDto.Subject,
                Priority = ticketDto.Priority,
                Status = ticketDto.Status ?? "NEW",
                AssignedAgent = ticketDto.AssignedAgent, // Map AssignedAgent back
                UserId = ticketDto.UserId
            };
        }

        public Ticket GetById(String id) {
            return _context.Tickets.Find(id);
        }

        public  Ticket Updating(Ticket ticket)
        {
            // Update ticket and save changes
            _context.Tickets.Update(ticket);
             _context.SaveChanges();

            return ticket;
        }

    }
}
