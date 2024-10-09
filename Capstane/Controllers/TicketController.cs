using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.Services;
using Microsoft.EntityFrameworkCore;
using CapstoneDAL.Models.Dtos;

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


        [HttpGet]
        public async Task<ActionResult<List<TicketDto>>> GetAllTickets()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return Ok(tickets);
        }




        [HttpGet("{userId}/tickets")] // Updated route for getting tickets by user ID
        public async Task<ActionResult<List<TicketDto>>> GetTicketsByUserId(long userId)
        {
            var tickets = await _ticketService.GetTicketsByUserIdAsync(userId);
            if (tickets == null || tickets.Count == 0)
            {
                return NotFound();
            }
            return Ok(tickets);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(string id)
        {
            try
            {
                await _ticketService.DeleteTicketAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        // You can add more methods like GetAllTickets, UpdateTicket, DeleteTicket, etc.



        [HttpPost("{id}/messages")]
        public async Task<IActionResult> AddMessageToTicket(string id, [FromBody] MessageDto messageDto)
        {
            // Validate the messageDto input
            if (messageDto.Content == null && messageDto.Attachment == null)
            {
                return BadRequest();
            }

            try
            {
                // Call the async method from the service to add a message to the ticket
                var newMessage = await _ticketService.AddMessageToTicketAsync(id, messageDto);

                // Return a 201 Created response with the new message data
                return CreatedAtAction(nameof(AddMessageToTicket), new { id = newMessage.TicketId }, newMessage);
            }
            catch (Exception ex)
            {
                // Handle any potential errors, such as "Ticket not found"
                return NotFound(new { Message = ex.Message });
            }
        }


		[HttpGet("{id}/messages")]

		public async Task<IActionResult> GetMessagesForTicket(string id)

		{

			try

			{

				// Call the service layer to get the messages

				var messages = await _ticketService.GetMessagesForTicketAsync(id);

				return Ok(messages); // Return 200 OK with the list of messages

			}

			catch (Exception ex)

			{

				// Return 404 Not Found if ticket is not found

				return NotFound(new { Message = ex.Message });

			}

		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateTicket(string id, [FromBody] TicketDto updatedTicketDto)
		{
			// Check if both subject and priority are null, return BadRequest if true
			if (updatedTicketDto.Subject == null && updatedTicketDto.Priority == null)
			{
				return BadRequest(); // Equivalent to HttpStatus.BAD_REQUEST in Java
			}

			try
			{
				var updatedTicket = await _ticketService.UpdateTicketAsync(id, updatedTicketDto);

				// If the ticket is not found, return NotFound (equivalent to HttpStatus.NOT_FOUND)
				if (updatedTicket == null)
				{
					return NotFound();
				}

				// Return the updated ticket with status OK
				return Ok(updatedTicket); // Equivalent to new ResponseEntity<>(updatedTicket, HttpStatus.OK)
			}
			catch (Exception ex)
			{
				// Log the exception if needed (optional)
				return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the ticket.");
			}
		}



		[HttpGet("userfindbyticketname/{id}")]
		public async Task<IActionResult> pk(string id)
		{
			// Call the UserDet method to get the user's name associated with the ticket
			var userName = await _ticketService.UserDet(id);

			// Check if userName is null
			if (userName == null)
			{
				return NotFound(); // Return 404 if the user is not found
			}

			// Return the user's name in the response
			return Ok(userName); // Return 200 OK with the user name
		}





	}
}
