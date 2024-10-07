using System.Collections.Generic;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.Models.Entities;

namespace Capstone.Services
{
    public interface ITicketService
    {
        Task<TicketDto> CreateTicketAsync(TicketDto ticketDto);
        Task<List<TicketDto>> GetAllTicketsAsync();
        Task<TicketDto?> GetTicketByIdAsync(string id); // Marked as nullable
        Task<TicketDto> UpdateTicketAsync(string id, TicketDto updatedTicketDto);
        Task DeleteTicketAsync(string id);
    }
}