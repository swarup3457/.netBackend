using Capstone.Models;
using Capstone.Models.Entities;
using CapstoneDAL;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Repositories;

namespace TicketManagement.Services
{
    public class DisplayAgentService
    {
        private readonly IAgentRepository _agentRepository;
        private readonly IDisplayAgentRepository _displayAgentRepository;
        private readonly CapstoneDbContext _context;

        // Constructor injection for repositories
        public DisplayAgentService(AgentRepository agentRepository, DisplayAgentRepository displayAgentRepository, CapstoneDbContext _context)
        {
            _agentRepository = agentRepository;
            _displayAgentRepository = displayAgentRepository;
            this._context=_context;
        }

        // Add Agent to DisplayAgents
        public string AddAgentToDisplayAgents(string email)
        {
            // Fetch agent by email
            var agent = _agentRepository.FindByEmail(email);

            if (agent == null)
            {
                return $"Agent with email {email} not found.";
            }

            // Check if agent already exists in DisplayAgent
            var existingDisplayAgent = _displayAgentRepository.FindByEmail(email);
            if (existingDisplayAgent != null)
            {
                return $"Agent with email {email} is already in display_agents.";
            }

            // Create new DisplayAgent and map properties
            var displayAgent = new DisplayAgent
            {
                Id = agent.Id,
                Email = agent.Email,
                Name = agent.Name,
                Role = "Agent",
                AutoAssigned = false
            };

            // Save to the display agent repository
            _displayAgentRepository.Add(displayAgent);
            _displayAgentRepository.Save();

            return "Email successfully added";
        }

        // Get all DisplayAgents
        public List<DisplayAgent> GetAllDisplayAgents()
        {
            return _displayAgentRepository.GetAll().ToList();
        }

        // Get DisplayAgent by ID
        public DisplayAgent GetDisplayAgentById(long id)
        {
            var displayAgent = _displayAgentRepository.GetById(id);
            return displayAgent;
        }

        public async Task<List<Ticket>> GetTicketsByAgentIdAsync(long agentId)
        {
            return await _context.Tickets
                .Where(ticket => ticket.AssignedAgentEntity != null && ticket.AssignedAgentEntity.Id == agentId)
                .ToListAsync();
        }
    }
}
