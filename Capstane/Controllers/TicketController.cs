using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.Services;
using Microsoft.EntityFrameworkCore;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // Create a new ticket
        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] TicketDto ticketDto)
        {
            if (ticketDto == null)
            {
                return BadRequest("Ticket data is null.");
            }

            try
            {
                var createdTicket = await _ticketService.CreateTicketAsync(ticketDto);
                return CreatedAtAction(nameof(GetTicketById), new { id = createdTicket.Id }, createdTicket);
            }
            catch (DbUpdateException dbEx)
            {
                // Log the details of the database update exception
                return StatusCode(500, $"Database update error: {dbEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                // Log the general exception
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        // Get a ticket by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(string id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);

            if (ticket == null)
            {
                return NotFound($"Ticket with ID {id} not found.");
            }

            return Ok(ticket);
        }

        // You can add more methods like GetAllTickets, UpdateTicket, DeleteTicket, etc.
    }
}
