using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Capstone.Models;
using Capstone.Models.Entities;
using CapstoneDAL;
using DeleteTickets.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using CapstoneDAL.Models.Dtos;

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
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();  // Save first to generate the Ticket ID

            // Now that the ticket is saved, we have the Ticket ID
            if (ticketDto.Messages != null)
            {
                foreach (var messageDto in ticketDto.Messages)
                {
                    var message = new MessageTable
                    {
                        Content = messageDto.Content,
                        Attachment = messageDto.Attachment,
                        AttachmentType = messageDto.AttachmentType,
                        AttachmentName = messageDto.AttachmentName,
                        TicketId = ticket.Id // Now ticket.Id is assigned after saving
                    };

                    _context.message.Add(message);  // Add the message without saving yet
                }

                // Only save changes once, after all messages are added
                await _context.SaveChangesAsync();
            }

            // Convert the saved ticket back to a DTO and return it
            return ConvertToDto(ticket);
        }



        public async Task<List<TicketDto>> GetAllTicketsAsync()
        {
            return await _context.Tickets
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    Subject = t.Subject,
                    Priority = t.Priority,
                    Status = t.Status,
                    AssignedAgent = t.AssignedAgent,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,
                    UserId = t.UserId
                })
                .ToListAsync();
        }

        public async Task<TicketDto> GetTicketByIdAsync(string id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            return new TicketDto
            {
                Id = ticket.Id,
                Subject = ticket.Subject,
                Priority = ticket.Priority,
                Status = ticket.Status,
                AssignedAgent = ticket.AssignedAgent,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                UserId = ticket.UserId
            };
        }

        public async Task<List<TicketDto>> GetTicketsByUserIdAsync(long userId)
        {
            return await _context.Tickets
                .Where(t => t.UserId == userId)
                .Select(t => new TicketDto
                {
                    Id = t.Id,
                    Subject = t.Subject,
                    Priority = t.Priority,
                    Status = t.Status,
                    AssignedAgent = t.AssignedAgent,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,
                    UserId = t.UserId
                })
                .ToListAsync();
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
                UserId = ticket.UserId,
                Messages = ticket.Messages.Select(message => new MessageDto

                {

                    Content = message.Content,

                    Attachment = message.Attachment,

                    AttachmentType = message.AttachmentType,

                    AttachmentName = message.AttachmentName

                }).ToList()

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


        public async Task<List<Ticket>> FindTicketsByUserIdAndCreatedAtBetweenAsync(long userId, DateTime startDate, DateTime endDate)
        {
            var tickets = await _context.Tickets
                .Where(t => t.UserId == userId && t.CreatedAt >= startDate && t.CreatedAt <= endDate)
                .ToListAsync();

            return tickets;
        }

        // Method to find tickets by a date range (without UserId)
        public async Task<List<Ticket>> FindTicketsByCreatedAtBetweenAsync(DateTime startDate, DateTime endDate)
        {
            var tickets = await _context.Tickets
                .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
                .ToListAsync();

            return tickets;
        }





		public async Task<List<DeletedTicket>> GetAllDeletedTicketsAsync()
		{
			return await _context.DeletedTickets.ToListAsync();
		}

		public async Task<List<DeletedTicket>> GetDeletedTicketByIdAsync(long userId)
		{
			return await _context.DeletedTickets.Where(u => u.UserId == userId).ToListAsync(); ;
		}


        // Delete a ticket by ID

        public async Task DeleteTicketAsync(string id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null) throw new Exception("Ticket not found");

            var deletedTicket = new DeletedTicket(ticket);
            await _context.DeletedTickets.AddAsync(deletedTicket);

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<MessageDto> AddMessageToTicketAsync(string ticketId, MessageDto messageDto)

        {

            // Find the ticket asynchronously using the Tickets DbSet

            var existingTicket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);

            if (existingTicket == null)

            {

                throw new Exception("Ticket not found");

            }

            // Create a new message entity

            var newMessage = new MessageTable

            {

                Content = messageDto.Content,

                Attachment = messageDto.Attachment,

                AttachmentType = messageDto.AttachmentType,

                AttachmentName = messageDto.AttachmentName,

                TicketId = existingTicket.Id // Reference the ticket's ID

            };

            // Add the new message to the Messages DbSet (with correct name)

            await _context.message.AddAsync(newMessage);

            // Save changes to the database

            await _context.SaveChangesAsync();

            // Return the message DTO

            return ConvertToDto(newMessage);

        }

        public MessageDto ConvertToDto(MessageTable message)

        {

            return new MessageDto

            {

                Content = message.Content,

                Attachment = message.Attachment,

                AttachmentType = message.AttachmentType,

                AttachmentName = message.AttachmentName,

                TicketId = message.Ticket.Id.ToString()  // Assuming Ticket.Id is of type long

            };

        }


    }
}
