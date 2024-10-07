using Capstane.Services;
using Capstone.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstane.Controllers
{
    [Route("api")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPut("{ticketId}/assign-agent/{agentId}")]
        public async Task<ActionResult<Ticket>> AssignAgentToTicket(string ticketId, long agentId)
        {
            try
            {
                var updatedTicket =  _adminService.assignAgentToTicket(ticketId, agentId);
                if (updatedTicket == null)
                {
                    return NotFound("Ticket or agent not found.");
                }
                return Ok(updatedTicket);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
