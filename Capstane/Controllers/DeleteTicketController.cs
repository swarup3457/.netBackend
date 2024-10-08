using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeleteTickets.Models.Entities; // Ensure this is correct
using Capstone.Services;

namespace DeleteTickets.Controllers
{
    [Route("api/deletedtickets")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")] // Ensure CORS is configured in Startup.cs
    public class DeletedTicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public DeletedTicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<DeletedTicket>>> GetAllDeletedTickets()
        {
            var deletedTickets = await _ticketService.GetAllDeletedTicketsAsync();
            return Ok(deletedTickets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeletedTicket>> GetDeletedTicketById(long id)
        {
            var deletedTicket = await _ticketService.GetDeletedTicketByIdAsync(id);
            if (deletedTicket == null)
            {
                return NotFound(); // Return 404 if not found
            }
            return Ok(deletedTicket);
        }
    }
}
