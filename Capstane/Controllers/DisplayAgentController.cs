using Microsoft.AspNetCore.Mvc;
using TicketManagement.Services;
using Microsoft.AspNetCore.Cors;
using Capstone.Models;

namespace TicketManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class DisplayAgentController : ControllerBase
    {
        private readonly DisplayAgentService _displayAgentService;

        public DisplayAgentController(DisplayAgentService displayAgentService)
        {
            _displayAgentService = displayAgentService;
        }

        // POST: api/displayagent
        [HttpPost]
        public IActionResult AddAgentToDisplayAgents([FromBody] string email)
        {
            var result = _displayAgentService.AddAgentToDisplayAgents(email);

            if (result.Contains("not found"))
            {
                return NotFound(result); // 404 Not Found
            }
            else if (result.Contains("already exists"))
            {
                return Conflict(result); // 409 Conflict
            }

            return Ok(result); // 200 OK
        }

        // GET: api/displayagent
        [HttpGet]
        public ActionResult<List<DisplayAgent>> GetAllDisplayAgents()
        {
            var displayAgents = _displayAgentService.GetAllDisplayAgents();
            return Ok(displayAgents); // 200 OK
        }

        // GET: api/displayagent/{id}
        [HttpGet("{id}")]
        public ActionResult<DisplayAgent> GetDisplayAgentById(long id)
        {
            var displayAgent = _displayAgentService.GetDisplayAgentById(id);
            if (displayAgent == null)
            {
                return NotFound(); // 404 Not Found
            }
            return Ok(displayAgent); // 200 OK
        }
    }
}
