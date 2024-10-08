using System.Collections.Generic;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.Models.Entities;
using DeleteTickets.Models.Entities;

namespace Capstone.Services
{
    public interface ITicketService
    {
        Task<TicketDto> CreateTicketAsync(TicketDto ticketDto);
        Task<List<TicketDto>> GetAllTicketsAsync();
        Task<TicketDto?> GetTicketByIdAsync(string id); // Marked as nullable
        Task<TicketDto> UpdateTicketAsync(string id, TicketDto updatedTicketDto);
        Task DeleteTicketAsync(string id);

		//Task<List<DeletedTicket>> GetAllDeletedTicketsAsync();
		//Task<DeletedTicket?> GetDeletedTicketByIdAsync(string id);

		Task<List<DeletedTicket>> GetAllDeletedTicketsAsync();
		Task<List<DeletedTicket>> GetDeletedTicketByIdAsync(long userId);





        Task<List<TicketDto>> GetTicketsByUserIdAsync(long userId);
        //Task<List<TicketDto>> GetTicketsByAgentIdAsync(long agentId);


    }
}