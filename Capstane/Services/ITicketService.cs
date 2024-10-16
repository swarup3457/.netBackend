using System.Collections.Generic;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.Models.Entities;
using CapstoneDAL.Models.Dtos;
using DeleteTickets.Models.Entities;

namespace Capstone.Services
{
    public interface ITicketService
    {
        Task<TicketDto> CreateTicketAsync(TicketDto ticketDto);
        Task<List<TicketDto>> GetAllTicketsAsync();
        Task<TicketDto?> GetTicketByIdAsync(string id); 
        Task<TicketDto> UpdateTicketAsync(string id, TicketDto updatedTicketDto);
        Task DeleteTicketAsync(string id);


		Task<List<DeletedTicket>> GetAllDeletedTicketsAsync();
		Task<List<DeletedTicket>> GetDeletedTicketByIdAsync(long userId);





        Task<List<TicketDto>> GetTicketsByUserIdAsync(long userId);

        Task<MessageDto> AddMessageToTicketAsync(string ticketId, MessageDto messageDto);


       Task<List<MessageDto>> GetMessagesForTicketAsync(string ticketId);



		public Task<string> UserDet(String id);

		Task<TicketDto> GetOldestTicketAsync(long agentId);




	}
}